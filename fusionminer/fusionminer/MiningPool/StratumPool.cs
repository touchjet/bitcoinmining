using System;
using System.Threading;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Net;
using ServiceStack;
using ServiceStack.Text;

namespace FusionMiner
{
	enum RequestType
	{
		Subscribe,
		Authorize,
	}

	public class StratumPool: MiningPool
	{
		private ManualResetEvent _connectDone = new ManualResetEvent (false);
		private string _extraNonce1;
		private int _extraNonce2 = 0;
		private int _extraNonce2Len = 4;
		private int _extraNonce2Pos = 0;
		private readonly object _lockObject = new object ();
		private StratumWork _work = new StratumWork ();
		private int _id = 0;
		private Dictionary<int,RequestType> _requestQueue = new Dictionary<int,RequestType> ();
		private string _alternateServer = "";
		private int _alternatePort = 0;
		private Socket _client;

		public StratumPool (string url, int port, string user, string password)
		{
			_url = url;
			_port = port;
			_user = user;
			_password = password;
			new Thread (new ThreadStart (() => {
				ConnectPool ();
			})).Start ();
		}

		private int GetNewId ()
		{
			_id++;
			return _id;
		}

		public override void ConnectPool ()
		{
			_lastConnectionTime = DateTime.UtcNow;
			string url = _alternateServer == "" ? _url : _alternateServer;
			try {
				_requestQueue.Clear ();
				_status = PoolConnectionStatus.Connect;
				Utility.Log (LogLevel.Debug, "Connecting {0}", url);
				IPHostEntry ipHostInfo = Dns.GetHostEntry (url);
				IPAddress ipAddress = ipHostInfo.AddressList [0];
				IPEndPoint remoteEP = new IPEndPoint (ipAddress, _alternatePort == 0 ? _port : _alternatePort);		
				_client = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				_client.ReceiveTimeout = 15000;
				_client.SendTimeout = 15000;
				// Connect to the remote endpoint.
				_connectDone.Reset ();
				_client.BeginConnect (remoteEP, new AsyncCallback (ConnectCallback), _client);
				if (!_connectDone.WaitOne (20000)) {
					Utility.Log (LogLevel.Error, "Connection timeout!");
					Disconnect ();
					return;
				}
				Receive (_client);

				// Send test data to the remote device.
				JsConfig.EmitLowercaseNames = true;
				StratumRequest req = new StratumRequest ();
				req.Id = GetNewId ();
				req.Method = "mining.subscribe";
				req.Params = new List<string> ();
				_requestQueue.Add (req.Id, RequestType.Subscribe);
				Send (_client, JsonSerializer.SerializeToString<StratumRequest> (req) + "\n");
				Utility.Log (LogLevel.Debug, "Subscribe");

				req.Id = GetNewId ();
				req.Method = "mining.authorize";
				req.Params.Add (_user);
				req.Params.Add (_password);
				_requestQueue.Add (req.Id, RequestType.Authorize);
				Send (_client, JsonSerializer.SerializeToString<StratumRequest> (req) + "\n");
				Utility.Log (LogLevel.Debug, "Authorizing {0}", url);
				_connectionFailCount = 0;
			} catch (Exception e) {
				Utility.Log (LogLevel.Error, e.ToString ());
				Disconnect ();
			}
		}

		private void Disconnect ()
		{
			_status = PoolConnectionStatus.Dead;
			_connectionFailCount++;
			Utility.Log (LogLevel.Info, "Disconnecting from pool: {0}", _url);
			try {
				if (_client != null) {
					_client.Shutdown (SocketShutdown.Both);
					_client.Close ();
					_client = null;
				}
			} catch {
			}
		}

		private void ConnectCallback (IAsyncResult ar)
		{
			try {
				// Retrieve the socket from the state object.
				Socket client = (Socket)ar.AsyncState;
				if (client != null) {
					// Complete the connection.
					client.EndConnect (ar);
				}
				// Signal that the connection has been made.
				if (_connectDone != null) {
					_connectDone.Set ();
				}
			} catch (Exception e) {
				Utility.Log (LogLevel.Error, e.ToString ());
				Disconnect ();
			}
		}

		private void Receive (Socket client)
		{
			try {
				// Create the state object.
				StateObject state = new StateObject ();
				state.workSocket = client;

				// Begin receiving the data from the remote device.
				client.BeginReceive (state.buffer, 0, StateObject.BufferSize, 0,
					new AsyncCallback (ReceiveCallback), state);
			} catch (Exception e) {
				Utility.Log (LogLevel.Error, e.ToString ());
				Disconnect ();
			}
		}

		private void ReceiveCallback (IAsyncResult ar)
		{
			try {
				if ((ar != null) && (ar.AsyncState != null)) {
					// Retrieve the state object and the client socket 
					// from the asynchronous state object.
					StateObject state = (StateObject)ar.AsyncState;
					Socket client = state.workSocket;
					// Read data from the remote device.
					int bytesRead = client.EndReceive (ar);

					if (bytesRead > 0) {
						for (int i = 0; i < bytesRead; i++) {
							char chr = (char)state.buffer [i];
							state.sb.Append (chr);
							if (chr == '\n') {
								if (state.sb.Length > 1) {
									if (!ProcessResponse (state.sb.ToString ())) {
										Disconnect ();
										ConnectPool ();
										return;
									}
								}
								state.sb.Clear ();
							}
						}
						_lastRecepitonTime = DateTime.UtcNow;
						client.BeginReceive (state.buffer, 0, StateObject.BufferSize, 0,
							new AsyncCallback (ReceiveCallback), state);
					} else {
						Utility.Log (LogLevel.Error, "Disconnected By Server");
						Disconnect ();
						ConnectPool();
					}
				}
			} catch (Exception e) {
				Utility.Log (LogLevel.Error, e.ToString ());
				Disconnect ();
				ConnectPool();
			}
		}

		private void Send (Socket client, String data)
		{
//			Utility.Log (LogLevel.Debug, "- Stratum ->  " + data);
			try {
				byte[] byteData = Encoding.ASCII.GetBytes (data);
				client.BeginSend (byteData, 0, byteData.Length, 0,
					new AsyncCallback (SendCallback), client);
			} catch (Exception e) {
				Utility.Log (LogLevel.Error, e.ToString ());
				Disconnect ();
			}
		}

		private int _error = 0;
		private DateTime _lastErrorTime = DateTime.UtcNow;

		private bool ProcessResponse (string str)
		{
			try {
//				Utility.Log (LogLevel.Debug, "<- Stratum -  " + str);
				StratumResponse resp = JsonSerializer.DeserializeFromString<StratumResponse> (str);
				if (resp.Method != null) {
					switch (resp.Method) {
					case "mining.notify":
						if ((resp.Params != null) && (resp.Params.Count >= 9))
							lock (_lockObject) {
								if (_status == PoolConnectionStatus.Authorized) {
									_status = PoolConnectionStatus.Active;
								}
								_work.JobId = resp.Params [0];
								_work.PrevHash = resp.Params [1];
								Utility.HexStringToByteArray (_work.PrevHash, _work.PrevHashBin, 0);
								_work.CoinB1 = resp.Params [2];
								_work.CoinB2 = resp.Params [3];
								int cb1len = _work.CoinB1.Length >> 1;
								int cb2len = _work.CoinB2.Length >> 1;
								int en1len = _extraNonce1.Length >> 1;
								_extraNonce2Pos = cb1len + en1len;
								int cblen = cb1len + en1len + _extraNonce2Len + cb2len;
								if ((_work.CoinBaseBin == null) || (_work.CoinBaseBin.Length != (cblen))) {
									_work.CoinBaseBin = new byte[cblen];
								}
								Utility.HexStringToByteArray (_work.CoinB1, _work.CoinBaseBin, 0);
								Utility.HexStringToByteArray (_extraNonce1, _work.CoinBaseBin, cb1len);
								Utility.HexStringToByteArray (_work.CoinB2, _work.CoinBaseBin, _extraNonce2Pos + _extraNonce2Len);

								_work.MerkleBranchBin.Clear ();
								foreach (var s in resp.Params[4].FromJson<string[]>()) {
									_work.MerkleBranchBin.Add (Utility.HexStringToByteArray (s));
								}
								Utility.HexStringToByteArray (resp.Params [5], _work.VersionBin, 0);
								Utility.HexStringToByteArray (resp.Params [6], _work.NBitsBin, 0);
								_work.NTimeInt = uint.Parse (resp.Params [7], System.Globalization.NumberStyles.HexNumber);
								bool cleanJobs = resp.Params [8].ToLower ().Equals ("true");
								Utility.ReverseWord (_work.VersionBin);
								Utility.ReverseWord (_work.PrevHashBin);
								Utility.ReverseWord (_work.NBitsBin);
								if (cleanJobs) {
									RefreshQueue ();
								}
							}
						break;
					case "mining.set_difficulty":
						if ((resp.Params != null) && (resp.Params.Count == 1)) {
							_difficulty = Convert.ToDouble (resp.Params [0]);
							_ndifficulty = 0;
							Utility.Log (LogLevel.Info, "Difficulty Set To: " + Difficulty);
						}
						break;
					case "client.reconnect":
						_alternateServer = resp.Params [0];
						_alternatePort = Convert.ToInt32 (resp.Params [1]);
						return false;
					case "client.get_version":
						break;
					default:
						Utility.Log (LogLevel.Error, "Unknown Data: " + str);
						break;
					}
				} else if (resp.Id != null) {
					if (_requestQueue.ContainsKey ((int)resp.Id)) {
						switch (_requestQueue [(int)resp.Id]) {
						case RequestType.Authorize:
							if ((resp.Result != null) && (resp.Result.Count > 0)) {
								if (resp.Result [0].ToLower ().Equals ("true")) {
									_status = PoolConnectionStatus.Authorized;
									Utility.Log (LogLevel.Info, "Pool Available");
								} else {
									Utility.Log (LogLevel.Warning, str);
									Utility.Log (LogLevel.Warning, "Authorization Failed");
									return false;
								}
							}
							break;
						case RequestType.Subscribe:
							if (resp.Result != null) {
								_extraNonce1 = resp.Result [resp.Result.Count - 2];
								_extraNonce2Len = Convert.ToInt32 (resp.Result [resp.Result.Count - 1]);
								Utility.Log (LogLevel.Debug, "Extra Nonce 1 Set To: {0}    Extra nonce 2 Length: {1}", _extraNonce1, _extraNonce2Len);
							}
							break;
						default:
							break;
						}
						_requestQueue.Remove ((int)resp.Id);
					} else {  //No valid response id
						if ((resp.Result != null) && (resp.Result.Count > 1) && (resp.Result [0].ToLower ().Contains ("mining.notify"))) {
							_extraNonce1 = resp.Result [resp.Result.Count - 2];
							_extraNonce2Len = Convert.ToInt32 (resp.Result [resp.Result.Count - 1]);
							Utility.Log (LogLevel.Debug, "Extra Nonce 1 Set To: {0}    Extra nonce 2 Length: {1}", _extraNonce1, _extraNonce2Len);
						} else if ((resp.Result != null) && (resp.Result.Count > 0) && (resp.Result [0].ToLower ().Equals ("true"))) {
							_accepted++;
						} else {
							_rejected++;
							if ((resp.Error != null) && (resp.Error.Length > 0)) {
								if (resp.Error.ToLower ().Contains ("stale")) {
									_stale++;
								} else {
									if (DateTime.UtcNow.Subtract (_lastErrorTime).TotalSeconds > 30) {
										_error = 0;
									}
									_error++;
									_lastErrorTime = DateTime.UtcNow;
									Utility.Log (LogLevel.Warning, str);
									if (_error > 20) {
										_error = 0;
										Utility.Log (LogLevel.Warning, "Too Many Stratum Errors");
										return false;
									}
								}
							}
						}

					}
				} else {
					throw new Exception ("Response Id Not Exist");
				}
			} catch (Exception e) {
				Utility.Log (LogLevel.Error, "{0}\n{1}", str, e.ToString ());
			}
			return true;
		}

		private void SendCallback (IAsyncResult ar)
		{
		}

		private string GetExtraNonce2Str ()
		{
			string result = _extraNonce2.ToString ("x8");
			int strlen = _extraNonce2Len * 2;
			result = result.Substring (8 - strlen, strlen);
			_extraNonce2++;
			return result;
		}

		private byte[] _merkleRootBin = new byte[64];

		public override bool GetHashJob (ref HashData hashData)
		{
			try {
				if (_status == PoolConnectionStatus.Active) {
					hashData.ExtraNonce2 = GetExtraNonce2Str ();
					lock (_lockObject) {
						Utility.HexStringToByteArray (hashData.ExtraNonce2, _work.CoinBaseBin, _extraNonce2Pos, _extraNonce2Len);
						Sha256LibManaged.DoubleSha256 (_work.CoinBaseBin, _work.CoinBaseBin.Length, _merkleRootBin);
						foreach (var b in _work.MerkleBranchBin) {
							Buffer.BlockCopy (b, 0, _merkleRootBin, 32, 32);
							Sha256LibManaged.DoubleSha256 (_merkleRootBin, 64, _merkleRootBin);
						}
						Buffer.BlockCopy (_work.VersionBin, 0, hashData.BlockHeaderBin, 0, 4);
						Buffer.BlockCopy (_work.PrevHashBin, 0, hashData.BlockHeaderBin, 4, 32);
						Buffer.BlockCopy (_merkleRootBin, 0, hashData.BlockHeaderBin, 36, 32);
						Sha256LibManaged.CalcMidstate (hashData.BlockHeaderBin, hashData.MidStateBin);
						Buffer.BlockCopy (_work.NBitsBin, 0, hashData.BlockHeaderBin, 72, 4);
						hashData.JobId = _work.JobId;
						hashData.NTime = _work.NTimeInt;
						hashData.BlockHeaderBin [68] = (byte)(hashData.NTime & 0x000000ff);
						hashData.BlockHeaderBin [69] = (byte)((hashData.NTime >> 8) & 0x000000ff);
						hashData.BlockHeaderBin [70] = (byte)((hashData.NTime >> 16) & 0x000000ff);
						hashData.BlockHeaderBin [71] = (byte)((hashData.NTime >> 24) & 0x000000ff);
						return true;
					}
				} else {
					Utility.Log (LogLevel.Error, "Pool Not Ready.");
				}
			} catch (Exception e) {
				Utility.Log (LogLevel.Error, e.ToString ());
			}
			return false;
		}

		public override void SubmitHashResult (HashData hashData)
		{
			StratumResult result = new StratumResult ();
			result.Id = GetNewId ();
			result.Method = "mining.submit";
			result.Params = new List<string> ();
			result.Params.Add (_user);
			result.Params.Add (hashData.JobId);
			result.Params.Add (hashData.ExtraNonce2);
			result.Params.Add (hashData.NTime.ToString ("X"));
			result.Params.Add (Utility.ReverseWord (Convert.ToString (hashData.Nonce, 16).ToLower ().PadLeft (8, '0')));

//			Utility.Log (LogLevel.Debug, JsonSerializer.SerializeToString<StratumResult> (result));
			if (_client != null) {
				Send (_client, JsonSerializer.SerializeToString<StratumResult> (result) + "\n");
			}
		}

	}
	// State object for receiving data from remote device.
	public class StateObject
	{
		// Client socket.
		public Socket workSocket = null;
		// Size of receive buffer.
		public const int BufferSize = 4096;
		// Receive buffer.
		public byte[] buffer = new byte[BufferSize];
		// Received data string.
		public StringBuilder sb = new StringBuilder ();
	}

	public class StratumRequest
	{
		public int Id { get; set; }

		public string Method { get; set; }

		public List<string> Params { get; set; }
	}

	public class StratumResult
	{
		public List<string> Params { get; set; }

		public int Id { get; set; }

		public string Method { get; set; }
	}

	public class StratumResponse
	{
		public int? Id { get; set; }

		public string Method { get; set; }

		public string Error { get; set; }

		public List<string> Result { get; set; }

		public List<string> Params { get; set; }
	}

	public class StratumWork
	{
		public string JobId;
		public string PrevHash;
		public byte[] PrevHashBin = new byte[32];
		public string CoinB1;
		public string CoinB2;
		public byte[] CoinBaseBin;
		public List<byte[]> MerkleBranchBin = new List<byte[]> ();
		public byte[] VersionBin = new byte[4];
		public byte[] NBitsBin = new byte[4];
		public uint NTimeInt;
	}
}

