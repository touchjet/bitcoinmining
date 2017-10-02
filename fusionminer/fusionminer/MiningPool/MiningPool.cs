using System;
using System.Collections.Concurrent;

namespace FusionMiner
{
	public enum PoolConnectionStatus
	{
		Dead,
		Connect,
		Authorized,
		Active
	}

	public delegate void PoolRefreshHandler (MiningPool pool);

	public abstract class MiningPool
	{
		protected string _url;

		public string Url { get { return _url; } }

		protected int _port;
		protected string _user;

		public string User { get { return _user; } }

		protected string _password;
		protected PoolConnectionStatus _status = PoolConnectionStatus.Dead;
		protected DateTime _lastRecepitonTime = DateTime.UtcNow;

		public event PoolRefreshHandler OnPoolRefresh;

		public bool OK {
			get { return (_status == PoolConnectionStatus.Active) && (!ReceiveTimeOut); }
		}

		public bool Dead {
			get { return (_status == PoolConnectionStatus.Dead) || ReceiveTimeOut; }
		}

		public bool ReceiveTimeOut {
			get { return (DateTime.UtcNow.Subtract (_lastRecepitonTime).TotalSeconds >= 90); }
		}

		protected DateTime _lastConnectionTime = DateTime.UtcNow;

		public DateTime LastConnectionTime {
			get {
				return _lastConnectionTime;
			}
		}

		protected int _connectionFailCount = 0;

		public int ConnectionFailCount {
			get {
				return _connectionFailCount;
			}
		}

		protected double _difficulty = 64.0;
		protected uint _ndifficulty = 0;

		public double Difficulty { get { return _difficulty; } }

		public uint NominizedDifficulty {
			get {
				int rdiff = (int)Math.Round (_difficulty);
				if (_ndifficulty == 0) {
					if (rdiff < 64) {
						_ndifficulty = 64;
					} else {
						_ndifficulty = 0x00000200;
						while (_ndifficulty > rdiff) {
							_ndifficulty >>= 1;
						}
					}
				}
				return _ndifficulty;
			}
		}

		protected int _accepted = 0;

		public int Accepted { get { return _accepted; } }

		protected int _rejected = 0;

		public int Rejected { get { return _rejected; } }

		protected int _stale = 0;

		public int Stale { get { return _stale; } }

		public abstract bool GetHashJob (ref HashData hashData);

		public abstract void ConnectPool ();

		public abstract void SubmitHashResult (HashData hashData);

		protected void RefreshQueue ()
		{
			if (OnPoolRefresh != null) {
				OnPoolRefresh (this);
			}
		}
	}
}

