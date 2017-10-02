using System;
using System.Linq;
using FusionMiner.Thrift;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;

namespace FusionMiner
{
	public delegate void HashResultHandler (FDriver miner, byte board, byte core, UInt32 uniqueId, UInt32 nonce, UInt32 difficulty);
	public delegate void GetSingleHashDataHandler (FDriver miner, byte board, byte core, ref HashData hashData, out bool success);
	public delegate void GetBlockHashDataHandler (FDriver miner, byte board, byte core, ref HashData hashData, out byte maxRoll, out bool refresh, out bool success);

	public enum HardwareState
	{
		Running,
		Stopping,
		Stopped
	}

	public abstract class FDriver
	{
		public event HashResultHandler OnHashResult;
		public event GetSingleHashDataHandler OnGetSingleHashData;
		public event GetBlockHashDataHandler OnGetBlockHashData;

		protected UInt32 _difficulty = 64;
		protected HardwareState _state = HardwareState.Stopped;
		protected HardwareConfig _config;
		protected long _megahashes = 0;
		protected long _lastMegahashes = 0;
		protected DateTime _lastStatusTime = DateTime.UtcNow;
		protected HardwareStatus _hardwareStatus;
		protected DateTime _initialTime = DateTime.MinValue;
		protected ConcurrentQueue<string> _messageToShow = new ConcurrentQueue<string> ();

		protected void HashResultReceived (byte board, byte core, UInt32 uniqueId, UInt32 nonce, UInt32 difficulty)
		{
			if (OnHashResult != null) {
				if ((_initialTime == DateTime.MinValue) && (difficulty != 0)) {
					_initialTime = DateTime.UtcNow;
				}
				OnHashResult (this, board, core, uniqueId, nonce, difficulty);
			}
		}

		protected void SendSingleHashData (byte board, byte core)
		{
			if (OnGetSingleHashData != null) {
				bool success;
				OnGetSingleHashData (this, board, core, ref TxHashData, out success);
				if (success) {
					SendData (board, core, TxHashData, 1,false);
				}
			}
		}

		protected void SendBlockHashData (byte board, byte core)
		{
			if (OnGetBlockHashData != null) {
				bool refresh;
				bool success;
				byte maxRoll;
				OnGetBlockHashData (this, board, core, ref TxHashData, out maxRoll, out refresh, out success);
				if (success) {
					SendData (board, core, TxHashData, maxRoll,refresh);
				}
			}
		}

		public void ShowMessage (string message)
		{
			_messageToShow.Enqueue (message);
		}

		public HashData RxHashData = new HashData ();
		public HashData TxHashData = new HashData ();

		public HardwareState State { get { return _state; } }

		public HardwareStatus Status { get { return _hardwareStatus; } }

		public long NonceFound { get { return _hardwareStatus.Board.Sum (b => b.Chip.Sum (c => c.Core.Sum (o => o.NonceFound))); } }

		public long NonceNotFound { get { return _hardwareStatus.Board.Sum (b => b.Chip.Sum (c => c.Core.Sum (o => o.NonceNotFound))); } }

		public long HardwareError { get { return _hardwareStatus.Board.Sum (b => b.Chip.Sum (c => c.Core.Sum (o => o.HardwareError))); } }

		public long MegaHashes { get { return _megahashes; } }

		public int SpeedAvg { get { return _initialTime == DateTime.MinValue ? 0 : (int)(_megahashes * 1.02 / DateTime.UtcNow.Subtract (_initialTime).TotalSeconds); } } //1.048576

		public double TempBoardMax { get { return Status.Board.Max (b => b.Temperature); } }

		public double TempChipMax{ get { return Status.Board.Max (b => b.Chip.Max (c => c.Temperature)); } }

		private int _speedCur = 0;

		public int SpeedCur {
			get { 
				if (DateTime.UtcNow.Subtract (_lastStatusTime).TotalSeconds >= 10) {
					_speedCur = (int)((_megahashes - _lastMegahashes) * 1.02 / DateTime.UtcNow.Subtract (_lastStatusTime).TotalSeconds); //1.048576
					_lastMegahashes = _megahashes;
					_lastStatusTime = DateTime.UtcNow;
				}
				return _speedCur;
			}
		}

		public abstract bool Start ();

		public abstract void SendData (byte board, byte core, HashData Data, byte maxRoll, bool refresh);

		public abstract void SendConfig (byte board);

		public abstract void Stop ();

		public abstract void Blink ();

		public virtual string GetStatusString ()
		{
			StringBuilder sb = new StringBuilder ();
			sb.AppendLine (String.Format ("#{0:D2} -- {1}", _config.DeviceNumber, _config.DeviceName));
			int boardNumber = 0;
			foreach (var b in _hardwareStatus.Board) {
				if (b.Chip.Sum (c => c.Core.Sum (o => o.NonceNotFound)) > 0) {
					sb.AppendLine (String.Format ("Board: {0:D2}", boardNumber));
					sb.Append ("    ");
					foreach (var c in b.Chip) {
						sb.Append (String.Format ("[{0:D3}@{1:N1}] ", c.Core.Sum (o => o.NonceFound), c.Temperature));
					}
					sb.AppendLine ();
				}
				boardNumber++;
			}
			return sb.ToString ();
		}

		protected void InitializeStatus (int board, int chip, int core)
		{
			_hardwareStatus = new HardwareStatus ();
			_hardwareStatus.Board = new List<BoardStatus> ();
			for (int i = 0; i < board; i++) {
				var boardstatus = new BoardStatus ();
				boardstatus.Chip = new List<ChipStatus> ();
				boardstatus.Temperature = 0.0;
				for (int j = 0; j < chip; j++) {
					var chipstatus = new ChipStatus ();
					chipstatus.Core = new List<CoreStatus> ();
					chipstatus.Temperature = 0.0;
					for (int k = 0; k < core; k++) {
						var corestatus = new CoreStatus ();
						corestatus.NonceFound = 0;
						corestatus.NonceNotFound = 0;
						corestatus.Available = true;
						chipstatus.Core.Add (corestatus);
					}
					boardstatus.Chip.Add (chipstatus);
				}
				_hardwareStatus.Board.Add (boardstatus);
			}
		}

		public void UpdateDifficulty (UInt32 difficulty)
		{
			if (_difficulty != difficulty) {
				_difficulty = difficulty;
				_hardwareStatus.Difficulty = (int)_difficulty;
				SendConfig (0xff);
			}
		}
	}
}

