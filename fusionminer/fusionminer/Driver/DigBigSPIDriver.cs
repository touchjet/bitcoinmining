using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using FusionMiner.Thrift;
using RaspberryPiDotNet;
using RaspberryPiDotNet.MicroLiquidCrystal;

namespace FusionMiner
{
	public class DigBigSPIDriver : FDriver
	{
		#region Data_Constants

		private const uint SPI_PACK_SIZE = 128;
		private const int DB_DATA_SIZE = 52;
		private const byte DB_START_SENTINEL = 0xaa;
		private const byte DB_FINISH_SENTINEL = 0x55;
		private const byte DB_DATA_TYPE_CONFIG = 0x00;
		private const byte DB_DATA_TYPE_HASH_DATA = 0x01;
		private const byte DB_DATA_TYPE_HASH_RESULT = 0x10;
		private const byte NUMBER_OF_BOARD = 8;
		private const byte CHIP_PER_BOARD = 8;

		#endregion

		private byte[] Tx_Buffer = new byte[SPI_PACK_SIZE * DB_DATA_SIZE];
		private byte[] Rx_Buffer = new byte[SPI_PACK_SIZE * DB_DATA_SIZE];
		private int[] _tempCnt	= new int[NUMBER_OF_BOARD];
		private bool[,] _overHeated = new bool[NUMBER_OF_BOARD, CHIP_PER_BOARD];
		private DateTime[] _lastBoardConfigTime = new DateTime[NUMBER_OF_BOARD];

		private OverVoltData[] _overvolt = new OverVoltData[NUMBER_OF_BOARD];

		private int Tx_Pointer = 0;
		private object _spilock = new object ();

		public DigBigSPIDriver (HardwareConfig config)
		{
			_config = config;
			InitializeStatus (NUMBER_OF_BOARD, CHIP_PER_BOARD, 4);
			Start ();
		}

		private int _blinkCounter = 0;

		public override void Blink ()
		{
			_blinkCounter = 30;
		}

		public override bool Start ()
		{
			try {
				if (_state != HardwareState.Running) {
					Utility.Log (LogLevel.Info, "Connecting DigBig SPI Miner, Voltage: {0}, Frequency: {1}", _config.Voltage, _config.Frequency);

					for (int i = 0; i < NUMBER_OF_BOARD; i++) {
						_lastBoardConfigTime [i] = DateTime.UtcNow.Subtract (new TimeSpan (0, 0, 1));
						_tempCnt [i] = 0;
						for (int j = 0; j < CHIP_PER_BOARD; j++) {
							_overHeated [i, j] = false;
						}
						_overvolt [i].PrevHWError = 0;
						_overvolt [i].PrevHWError = 0;
						_overvolt [i].PrevErrRate = 0.0f;
						_overvolt [i].VoltOffset = 0;
						_overvolt [i].Stop = false;
					}

					new Thread (new ThreadStart (() => {
						try {
							int i;
							uint ledCounter = 0;
							bool ledOn = false;
							bool lcdBlOn = true;
							const uint ledCycle = 5;
							DateTime tc = DateTime.UtcNow;
							int idleCounter = 0;
							int lcdIndex = 0;
							DateTime lcdNextRefresh = DateTime.UtcNow.AddSeconds (3);
							DateTime overvoltNextCheck = DateTime.UtcNow.AddMinutes (2);

							using (GPIOMem ioLED = new GPIOMem (GPIOPins.V2_Pin_P1_22))
							using (GPIOMem ioLCDBL = new GPIOMem (GPIOPins.V2_Pin_P1_16))
							using (RaspPiGPIOMemLcdTransferProvider lcdProvider = new RaspPiGPIOMemLcdTransferProvider (
								                                                      GPIOPins.V2_Pin_P1_13,
								                                                      GPIOPins.V2_Pin_P1_15,
								                                                      GPIOPins.V2_Pin_P1_03,
								                                                      GPIOPins.V2_Pin_P1_05,
								                                                      GPIOPins.V2_Pin_P1_07,
								                                                      GPIOPins.V2_Pin_P1_11)) {


								_state = HardwareState.Running;
								Lcd lcd = new Lcd (lcdProvider);
								//Initialize LCD twice as it may fail sometime
								lcd.Begin (16, 2);
								lcd.Clear ();
								lcd.Begin (16, 2);
								lcd.Clear ();
								lcd.SetCursorPosition (0, 0);
								lcd.Write ("  GEVEY DIGBIG  ");
								lcd.SetCursorPosition (0, 1);
								lcd.Write (">>Excavator 1T<<");
								ioLCDBL.Write (lcdBlOn);
								Thread.Sleep (5000);

								using (SPI spi = new SPI (64)) {//4Mbps
									while (_state != HardwareState.Stopping) {
										lock (_spilock) {
											spi.Transfer (Tx_Buffer, Rx_Buffer, SPI_PACK_SIZE * DB_DATA_SIZE);
											Tx_Pointer = 0;
											for (i = 0; i < Tx_Buffer.Length; i++) {
												Tx_Buffer [i] = 0x88;
											}
										}
										i = 0;
										while (i < (SPI_PACK_SIZE * DB_DATA_SIZE)) {
											if (Rx_Buffer [i] == DB_START_SENTINEL) {
												ParseRxData (i);
												if (i == ((SPI_PACK_SIZE - 1) * DB_DATA_SIZE)) {
													Utility.Log (LogLevel.Warning, "SPI RX Buffer Full!");
												}
											} else {
												break;
											}
											i += DB_DATA_SIZE;
										}
										if (i == 0) {
											idleCounter++;
											if (idleCounter > 200) {
												InitializeStatus (NUMBER_OF_BOARD, CHIP_PER_BOARD, 4);
												break;
											}
										} else {
//										Utility.Log(LogLevel.Debug,"Rx Pack: {0}",i/DB_DATA_SIZE);
											idleCounter = 0;
										}

										int procTime = (int)DateTime.UtcNow.Subtract (tc).TotalMilliseconds;

										Thread.Sleep (procTime < 250 ? 250 - procTime : 1);
										tc = DateTime.UtcNow;

										if (DateTime.UtcNow > overvoltNextCheck) {
											overvoltNextCheck = DateTime.UtcNow.AddMinutes (5);
											for (byte board = 0; board < NUMBER_OF_BOARD; board++) {
												if (!_overvolt [board].Stop) {
													long nonceFound = Status.Board [board].Chip.Sum (c => c.Core.Sum (o => o.NonceFound));
													long hwError = Status.Board [board].Chip.Sum (c => c.Core.Sum (o => o.HardwareError));
													long a = nonceFound - _overvolt [board].PrevNonceFound;
													long e = hwError - _overvolt [board].PrevHWError;
													float errRate = 0;
													if (a + e > 0) {
														errRate = e * 100 / (a + e);
													}
													if (errRate > 5.0f) {
														switch (_overvolt [board].VoltOffset) {
														case 0: 
															_overvolt [board].VoltOffset = 3;
															Utility.Log (LogLevel.Warning, "High HW Error Rate {0}% (E{1}/A{2}), Increase Voltage By 0.025V", errRate, e, a + e);
															SendConfig (board);
															break;
														case 3: 
															_overvolt [board].VoltOffset = 5;
															Utility.Log (LogLevel.Warning, "High HW Error Rate {0}% (E{1}/A{2}), Increase Voltage By 0.05V", errRate, e, a + e);
															SendConfig (board);
															break;
														case 5: 
															if (errRate > _overvolt [board].PrevErrRate * 0.6) {
																_overvolt [board].VoltOffset = 0;
																Utility.Log (LogLevel.Warning, "Increase Voltage Does NOT Reduce HW Error Rate ({0}%->{1}%) (E{2}/A{3}), Set Voltage Back", new object[] {
																	_overvolt [board].PrevErrRate,
																	errRate,
																	e,
																	a + e
																});
																SendConfig (board);
															}
															_overvolt [board].Stop = true;
															break;
														}
													}
													_overvolt [board].PrevNonceFound = nonceFound;
													_overvolt [board].PrevHWError = hwError;
													_overvolt [board].PrevErrRate = errRate;
												}
											}
										}

										if (DateTime.UtcNow > lcdNextRefresh) {
											lcdNextRefresh = DateTime.UtcNow.AddSeconds (3);
											lcd.SetCursorPosition (0, 0);
											if ((MiningController.CurrentPool == null) || (!MiningController.CurrentPool.OK)) {
												if ((Config.Data.Pools != null) && (Config.Data.Pools.Count > 0)) {
													lcd.Write ("Connecting...   ");
													lcd.SetCursorPosition (0, 1);
													lcd.Write (MiningController.LocalIP.PadRight (16));
												} else {
													lcd.Write ("  Please Config ");
													lcd.SetCursorPosition (0, 1);
													lcd.Write ("   Mining Pool  ");
												}
											} else {
												string speedstr = String.Format ("{0:0,0}", SpeedCur).PadLeft (8);
												if (TempChipMax > 125.0) {
													lcd.Write (" !Over Heated!! ");
												} else {
													lcd.Write (String.Format ("{0}   MH/S.", speedstr));
												}
												lcd.SetCursorPosition (0, 1);
												string msgToShow;
												if ((!_messageToShow.IsEmpty) && (_messageToShow.TryDequeue (out msgToShow))) {
													lcd.Write (msgToShow.PadRight (16));
													lcdNextRefresh = DateTime.UtcNow.AddSeconds (20);
												} else {
													string shortIp = MiningController.LocalIP.Remove (0, MiningController.LocalIP.IndexOf ('.') + 1);
													shortIp = shortIp.Remove (0, shortIp.IndexOf ('.') + 1).PadRight (7);
													switch (lcdIndex) {
													case 0: 
														lcd.Write (String.Format ("Max Temp: {0:F1}C", TempChipMax).PadRight (16));
														break;
													case 1: 
														lcd.Write (String.Format ("SN: {0}", Config.Data.SN.PadLeft (12)));
														break;
													case 2: 
														lcd.Write (String.Format ("Max Temp: {0:F1}C", TempChipMax).PadRight (16));
														break;
													case 3: 
														lcd.Write (String.Format ("Accepted: {0:D6}", NonceFound));
														break;
													case 4: 
														lcd.Write (String.Format ("Max Temp: {0:F1}C", TempChipMax).PadRight (16));
														break;
													case 5: 
														lcd.Write (String.Format ("HW Error: {0:D6}", HardwareError));
														break;
													case 6: 
														lcd.Write (MiningController.LocalIP.PadRight (16));
														break;
													case 7: 
														lcd.Write (MiningController.LocalIP.PadRight (16));
														break;
													case 8: 
														lcd.Write (MiningController.LocalIP.PadRight (16));
														break;
													case 9: 
														lcd.Write (MiningController.LocalIP.PadRight (16));
														break;
													case 10: 
														string url = MiningController.CurrentPool.Url;
														lcd.Write (url.Length > 16 ? url.Substring (url.Length - 16) : url.PadLeft (16));
														break;
													}
													if (lcdIndex < 10) {
														lcdIndex++;
													} else {
														lcdIndex = 0;
													}
												}
											}
										}

										ledCounter++;
										if (ledCounter == ledCycle) {
											ledCounter = 0;
											ledOn = !ledOn;
											ioLED.Write (ledOn);
											if (_blinkCounter > 0) {
												_blinkCounter--;
												lcdBlOn = !lcdBlOn;
												ioLCDBL.Write (lcdBlOn);
											}
										}
									}
								}
							}
							Utility.Log (LogLevel.Warning, "DIGBIG SPI Stopped.");
							_state = HardwareState.Stopped;
						} catch (Exception e) {
							Utility.Log (LogLevel.Error, e.ToString ());
							_state = HardwareState.Stopping;
						}
					})).Start ();
				}
			} catch (Exception e) {
				Utility.Log (LogLevel.Error, e.ToString ());
			}
			return true;
		}

		private void ParseRxData (int baseptr)
		{
			//			typedef __packed struct {
			//				uint8_t StartSentinel;		//0xaa
			//				uint8_t DataType;			//0x10
			//				uint32_t UniqueId;			//32 bit unique Hash Job Id
			//				uint32_t Nonce;				//Nonce
			//				uint32_t Difficulty;		//Share difficulty calculated with the result Nonce. 0 means invalid result or no result.
			//				uint8_t Board;				//Board Number of the result.
			//				uint8_t Chip;				//Chip Number of the result.
			//				uint8_t Core;				//Core Number of the result.
			//				uint8_t BoardTemp;			//Board temprature
			//				uint8_t ChipTemp[8];		//Chip tempratures
			//				uint8_t FanSpeed[2];		//Fan speeds
			//				uint8_t NeedConfig;			//Require Configuration Data
			//				uint8_t Chips[8];			//Chip Status
			//				uint8_t Reserved[14];		//Reserved for future use
			//				uint8_t FinishSentinel;		//0x55
			//			} StructHashResult;
			uint nonce = 0;
			if (Rx_Buffer [baseptr + 1] == DB_DATA_TYPE_HASH_RESULT) {
				try {
					byte board = Rx_Buffer [baseptr + DB_DATA_SIZE - 1];
					if (board >= NUMBER_OF_BOARD) {
						throw new Exception ("Incorrect Board Number: " + board.ToString ());
					}
					if (Rx_Buffer [baseptr + 28] == 0x01) {
						Utility.Log (LogLevel.Warning, "Board {0} Needs Config", board);
						SendConfig (board);
						return;
					}
					uint uniqueId = ((uint)Rx_Buffer [baseptr + 2] << 24) + ((uint)Rx_Buffer [baseptr + 3] << 16) + ((uint)Rx_Buffer [baseptr + 4] << 8) + ((uint)Rx_Buffer [baseptr + 5]);
					uint resultDiff = ((uint)Rx_Buffer [baseptr + 13] << 24) + ((uint)Rx_Buffer [baseptr + 12] << 16) + ((uint)Rx_Buffer [baseptr + 11] << 8) + ((uint)Rx_Buffer [baseptr + 10]);
					byte core = Rx_Buffer [baseptr + 16];
					int chip = core / 4;
					CoreStatus corestat = _hardwareStatus.Board [board].Chip [chip].Core [core % 4];
					if (uniqueId > 0) {
						if (resultDiff == 0) {
							corestat.NonceNotFound++;
							_megahashes += 4096;
						} else {
							if (resultDiff != _difficulty) {
								SendConfig (board);
							} 
							nonce = ((uint)Rx_Buffer [baseptr + 9] << 24) + ((uint)Rx_Buffer [baseptr + 8] << 16) + ((uint)Rx_Buffer [baseptr + 7] << 8) + ((uint)Rx_Buffer [baseptr + 6]);
							corestat.NonceFound++;
							if (nonce >= 0xe0000000) {
								_megahashes += (uint)(((nonce - 0xe0000000 + 1) * 8) >> 20);
							} else if (nonce > 0xc0000000) {
								_megahashes += (uint)(((nonce - 0xc0000000 + 1) * 8) >> 20);
							} else if (nonce > 0xa0000000) {
								_megahashes += (uint)(((nonce - 0xa0000000 + 1) * 8) >> 20);
							} else if (nonce > 0x80000000) {
								_megahashes += (uint)(((nonce - 0x80000000 + 1) * 8) >> 20);
							} else if (nonce > 0x60000000) {
								_megahashes += (uint)(((nonce - 0x60000000 + 1) * 8) >> 20);
							} else if (nonce > 0x40000000) {
								_megahashes += (uint)(((nonce - 0x40000000 + 1) * 8) >> 20);
							} else if (nonce > 0x20000000) {
								_megahashes += (uint)(((nonce - 0x20000000 + 1) * 8) >> 20);
							} else {
								_megahashes += (uint)(((nonce + 1) * 8) >> 20);
							}
						}
						if (_tempCnt [board] == 0) {
							chip = baseptr;
							foreach (var c in _hardwareStatus.Board[board].Chip) {
								if (Rx_Buffer [18 + chip] != 0) {
									c.Temperature = ((double)Rx_Buffer [18 + chip] / 2) + 25.0;
				
									if (c.Temperature > 123) {
										if (!_overHeated [board, chip - baseptr]) {
											_overHeated [board, chip - baseptr] = true;
										}
									} else if ((_overHeated [board, chip - baseptr]) && (c.Temperature < 100)) {
										if (_overHeated [board, chip - baseptr]) {
											_overHeated [board, chip - baseptr] = false;
										}
									}
								}
								chip++;
							}
							if (Rx_Buffer [baseptr + 17] != 0) {
								_hardwareStatus.Board [board].Temperature = ((double)Rx_Buffer [baseptr + 17] / 2) + 25.0;
							}
						}
						_tempCnt [board] = (_tempCnt [board] + 1) % 64;
					}
					HashResultReceived (board, core, uniqueId, nonce, resultDiff);
					SendSingleHashData (board, core);
				} catch (Exception e) {
					byte[] dump = new byte[DB_DATA_SIZE];
					Buffer.BlockCopy (Rx_Buffer, baseptr, dump, 0, DB_DATA_SIZE);
					Utility.Log (LogLevel.Error, "{0}\n{1}", Utility.ByteArrayToHexString (dump), e.ToString ());
				}
			} else {
				Utility.Log (LogLevel.Error, "Incorrect Rx Data Type: {0}", Rx_Buffer [baseptr + 1]);
			}
		}

		public override void Stop ()
		{
			_state = HardwareState.Stopping;

			while (_state != HardwareState.Stopped) {
				Thread.Sleep (100);
			}
		}

		public override void SendConfig (byte board)
		{
			if (board == 0xff) {
				for (byte i = 0; i < NUMBER_OF_BOARD; i++) {
					SendConfig (i);
				}
				return;
			}
			if (_state != HardwareState.Stopping) {
				if (DateTime.UtcNow.Subtract (_lastBoardConfigTime [board]).TotalSeconds > 1) { 
					_lastBoardConfigTime [board] = DateTime.UtcNow;
					Utility.Log (LogLevel.Warning, "Send Config to board {0} Difficulty {1} Frequency {2} Voltage {3}", new object[] {
						board,
						_difficulty,
						_config.Frequency,
						_config.Voltage + _overvolt [board].VoltOffset
					});
					lock (_spilock) {
						if (Tx_Pointer < SPI_PACK_SIZE * DB_DATA_SIZE) {
							int tmpPtr = 0;
							//Check if there's already a config or data pack for the board in the Tx Buffer
							while (tmpPtr < Tx_Pointer) {
								if (Tx_Buffer [tmpPtr + DB_DATA_SIZE - 1] == board) {
									break;
								}
								tmpPtr += DB_DATA_SIZE;
							}

							Tx_Buffer [tmpPtr] = DB_START_SENTINEL;
							Tx_Buffer [tmpPtr + 1] = DB_DATA_TYPE_CONFIG;
							Tx_Buffer [tmpPtr + 2] = (byte)(_config.Frequency >> 4);
							Tx_Buffer [tmpPtr + 3] = (byte)(_config.Voltage + _overvolt [board].VoltOffset);
							Tx_Buffer [tmpPtr + 4] = (byte)(_difficulty >> 24 & 0xff);
							Tx_Buffer [tmpPtr + 5] = (byte)(_difficulty >> 16 & 0xff);
							Tx_Buffer [tmpPtr + 6] = (byte)(_difficulty >> 8 & 0xff);
							Tx_Buffer [tmpPtr + 7] = (byte)(_difficulty >> 0 & 0xff);
							for (int i = 8; i < DB_DATA_SIZE - 1; i++) {
								Tx_Buffer [tmpPtr + i] = 0;
							}
							Tx_Buffer [tmpPtr + DB_DATA_SIZE - 1] = board;
							if (tmpPtr == Tx_Pointer) {
								Tx_Pointer += DB_DATA_SIZE;
							}
						} else {
							Utility.Log (LogLevel.Error, "SPI TX Buffer Overflow!");
						}
					}
				}
			}
		}

		public override void SendData (byte board, byte core, HashData Data, byte maxRoll, bool refresh)
		{
			if (_state != HardwareState.Stopping) {
				if (!_overHeated [board, core / 4]) {
					lock (_spilock) {
						if (Tx_Pointer < SPI_PACK_SIZE * DB_DATA_SIZE) {
							Tx_Buffer [Tx_Pointer] = DB_START_SENTINEL;
							Tx_Buffer [Tx_Pointer + 1] = DB_DATA_TYPE_HASH_DATA;
							Tx_Buffer [Tx_Pointer + DB_DATA_SIZE - 2] = core;
							Tx_Buffer [Tx_Pointer + 2] = (byte)((Data.UniqueId >> 24) & 0xff);
							Tx_Buffer [Tx_Pointer + 3] = (byte)((Data.UniqueId >> 16) & 0xff);
							Tx_Buffer [Tx_Pointer + 4] = (byte)((Data.UniqueId >> 08) & 0xff);
							Tx_Buffer [Tx_Pointer + 5] = (byte)((Data.UniqueId >> 00) & 0xff);
							Buffer.BlockCopy (Data.MidStateBin, 0, Tx_Buffer, Tx_Pointer + 6, 32);
							Buffer.BlockCopy (Data.BlockHeaderBin, 64, Tx_Buffer, Tx_Pointer + 38, 12);
							Tx_Buffer [Tx_Pointer + DB_DATA_SIZE - 1] = board;
							Tx_Pointer += DB_DATA_SIZE;
						} else {
							Utility.Log (LogLevel.Error, "SPI TX Buffer Overflow!");
						}
					}
				}
			}
		}
	}

	public struct OverVoltData
	{
		public long PrevNonceFound;
		public long PrevHWError;
		public float PrevErrRate;
		public int VoltOffset;
		public bool Stop;
	}
}

