using System;
using FusionMiner.Thrift;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace FusionMiner
{
	public class HostInterface
	{
		private bool _connected = false;
		private X509Certificate2 _cert;
		private TProtocol _protocol;
		private TTransport _transport;
		private HostService.Client _client;

		public bool Connected { get { return _connected; } }

		public HostInterface (bool secured = false)
		{
			if (secured) {
				_cert = new X509Certificate2 ("client.p12", Aes.DecryptString ("")); 
				_transport = new TTLSSocket (Aes.DecryptString (""), 7802, _cert); 
				((TTLSSocket)_transport).Timeout = 10000;
			} else {
				_transport = new TSocket (Aes.DecryptString (""), 7709);
				((TSocket)_transport).Timeout = 10000;
				_transport = new TFramedTransport (_transport);
			}
			_protocol = new TBinaryProtocol (_transport);
			_client = new HostService.Client (_protocol);
		}

		public string Ping ()
		{
			string result = "";
			try {
				_transport.Open ();
				result = _client.Ping ();
			} catch (Exception e) {
				Utility.Log (LogLevel.Debug, e.ToString ());
			} finally {
				if (_transport.IsOpen) {
					_transport.Close ();
				}
			}
			return result;
		}

		public string GetSN (MinerInfo minerInfo)
		{
			string result = "";
			Utility.Log (LogLevel.Debug, "Getting SN");
			try {
				_transport.Open ();
				result = _client.GetSN (minerInfo);
			} catch (Exception e) {
				Utility.Log (LogLevel.Debug, e.ToString ());
			} finally {
				if (_transport.IsOpen) {
					_transport.Close ();
				}
			}
			return result;
		}

		public Int64 GetUniqueId (MinerInfo minerInfo)
		{
			Int64 result = 0;
			Utility.Log (LogLevel.Debug, "Getting UniqueId");
			try {
				_transport.Open ();
				result = _client.GetUniqueId (minerInfo);
			} catch (Exception e) {
				Utility.Log (LogLevel.Debug, e.ToString ());
			} finally {
				if (_transport.IsOpen) {
					_transport.Close ();
				}
			}
			return result;
		}

		private void ExecuteMaintenance (Maintenance maint)
		{
			Utility.Log (LogLevel.Info, "ExecuteMaintenance Version:{0}", maint.SoftwareVersion);
			foreach (var t in maint.Steps) {
				switch (t.Type) {
				case MaintStepType.AddFile:
					Utility.Log (LogLevel.Warning, "Add File: {0}", t.Path + t.Command);
					if (Utility.VerifyMd5Hash (t.FileData, t.Md5)) {
						File.WriteAllBytes (t.Path + t.Command, t.FileData);
					} else {
						Utility.Log (LogLevel.Error, "File: {0} MD5 Checksum Error!", t.Path + t.Command);
					}
					break;
				case MaintStepType.RemoveFile:
					Utility.Log (LogLevel.Warning, "Remove File: {0}", t.Path + t.Command);
					File.Delete (t.Path + t.Command);
					break;
				case MaintStepType.MinerCommand:
					switch (t.Command) {
					case "reboot":
						Utility.Log (LogLevel.Warning, "Reboot");
						ProgramLauncher.Execute ("/usr/bin/reboot", "");
						break;
					}
					break;
				case MaintStepType.SystemCommand:
					Utility.Log (LogLevel.Info, "Execute System Command: {0}{1}", t.Path, t.Command);
					ProgramLauncher.Execute (t.Path, t.Command);
					break;
				}
			}
		}

		public void QueryMaintenanceTask (MinerInfo minerInfo)
		{
			try {
				_transport.Open ();
				Maintenance maint = _client.QueryMaintenanceTask (minerInfo.UniqueId, minerInfo.Version, "");
				if ((maint != null) && (maint.Checksum != null)) {
					if (Aes.DecryptString (maint.Checksum) == Config.Data.MAC) {
						ExecuteMaintenance (maint);
					}
				}
			} catch (Exception e) {
				Utility.Log (LogLevel.Debug, e.ToString ());
			} finally {
				if (_transport.IsOpen) {
					_transport.Close ();
				}
			}
		}
	}
}

