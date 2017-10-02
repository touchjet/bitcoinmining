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
	public class DigBigSPIV2Driver : FDriver
	{
		#region Data_Constants

		private const uint SPI_PACK_SIZE = 192;
		private const int DB_DATA_SIZE = 52;

		private const byte DB_START_SENTINEL = 0xaa;
		private const byte DB_FINISH_SENTINEL = 0x55;

		private const byte DB_DATA_TYPE_CONFIG = 0x00;
		private const byte DB_DATA_TYPE_HASH_DATA = 0x01;
		private const byte DB_DATA_TYPE_HASH_DATA_REF = 0x02;
		private const byte DB_DATA_TYPE_HASH_RESULT = 0x10;
		private const byte DB_DATA_TYPE_REQUEST_DATA = 0x20;

		private const byte NUMBER_OF_BOARD = 12;
		private const byte CHIP_PER_BOARD = 20;

		#endregion

		private byte[] Tx_Buffer = new byte[SPI_PACK_SIZE * DB_DATA_SIZE];
		private byte[] Rx_Buffer = new byte[SPI_PACK_SIZE * DB_DATA_SIZE];
		private DateTime[] _lastBoardConfigTime = new DateTime[NUMBER_OF_BOARD];

		private OverVoltData[] _overvolt = new OverVoltData[NUMBER_OF_BOARD];
		private bool[] _boardBufferRefresh = new bool[NUMBER_OF_BOARD];

		private int Tx_Pointer = 0;
		private object _spilock = new object ();

		public DigBigSPIV2Driver (HardwareConfig config)
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
					Utility.Log (LogLevel.Info, "Connecting DigBig SPI V2 Miner, Voltage: {0}, Frequency: {1}", _config.Voltage, _config.Frequency);

					for (int i = 0; i < NUMBER_OF_BOARD; i++) {
						_lastBoardConfigTime [i] = DateTime.UtcNow.Subtract (new TimeSpan (0, 0, 1));
						_overvolt [i].PrevHWError = 0;
						_overvolt [i].PrevHWError = 0;
						_overvolt [i].PrevErrRate = 0.0f;
						_overvolt [i].VoltOffset = 0;
						_overvolt [i].Stop = false;
						_boardBufferRefresh[i] = true;
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
							DateTime overvoltNextCheck = DateTime.UtcNow.AddMinutes (6);

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

								using (SPI spi = new SPI (32)) {//4Mbps
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
												Utility.Log (LogLevel.Warning, "No Data Received From Hash Board.");
												InitializeStatus (NUMBER_OF_BOARD, CHIP_PER_BOARD, 4);
												break;
											}
										} else {
//										Utility.Log (LogLevel.Debug, "Rx Pack: {0}", i / DB_DATA_SIZE);
											idleCounter = 0;
										}

										int procTime = (int)DateTime.UtcNow.Subtract (tc).TotalMilliseconds;

										Thread.Sleep (procTime < 250 ? 250 - procTime : 1);
										tc = DateTime.UtcNow;

										if (DateTime.UtcNow > overvoltNextCheck) {
											overvoltNextCheck = DateTime.UtcNow.AddMinutes (6);
											for (byte board = 0; board < NUMBER_OF_BOARD; board++) {
												if (!_overvolt [board].Stop) {
													long nonceFound = Status.Board [board].Chip.Sum (c => c.Core.Sum (o => o.NonceFound));
													long hwError = Status.Board [board].Chip.Sum (c => c.Core.Sum (o => o.HardwareError));
													long a = nonceFound - _overvolt [board].PrevNonceFound;
													long e = hwError - _overvolt [board].PrevHWError;
													float errRate = 0;
													if (a + e > 0) {
														errRate = (float)e * 100.0f / (float)(a + e);
													}
													if (errRate > 5.0f) {
														_overvolt [board].VoltOffset = 3;
														Utility.Log (LogLevel.Warning, "High HW Error Rate {0}% (E{1}/A{2}), Increase Voltage By 0.025V", errRate, e, a + e);
														SendConfig (board);
														_overvolt [board].Stop = true;
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
												if (MiningController.HostConnected) {
													lcd.Write (String.Format ("{0}   MH/S ", speedstr));
												} else {
													lcd.Write (String.Format ("{0}   MH/S.", speedstr));
												}
												lcd.SetCursorPosition (0, 1);
												if (lcdIndex == 0) {
													Utility.Log(LogLevel.Debug,"{0}   MH/S ", speedstr);
												}
												string msgToShow;
												if ((!_messageToShow.IsEmpty) && (_messageToShow.TryDequeue (out msgToShow))) {
													lcd.Write (msgToShow.PadRight (16));
													lcdNextRefresh = DateTime.UtcNow.AddSeconds (20);
												} else {
													switch (lcdIndex) {
													case 0: 
														lcd.Write (String.Format ("Max Temp: {0:F1}", TempChipMax).PadRight (16));
														break;
													case 1: 
														lcd.Write (String.Format ("SN: {0}", Config.Data.SN.PadLeft (12)));
														break;
													case 2: 
														lcd.Write (String.Format ("Max Temp: {0:F1}", TempChipMax).PadRight (16));
														break;
													case 3: 
														lcd.Write (String.Format ("Accepted: {0:D6}", NonceFound));
														break;
													case 4: 
														lcd.Write (String.Format ("Max Temp: {0:F1}", TempChipMax).PadRight (16));
														break;
													case 5: 
														lcd.Write (String.Format ("HW Error: {0:D6}", HardwareError));
														break;
													case 6: 
														lcd.Write (String.Format ("Max Temp: {0:F1}", TempChipMax).PadRight (16));
														break;
													case 7: 
														lcd.Write (MiningController.LocalIP.PadRight (16));
														break;
													case 8: 
														lcd.Write (String.Format ("Max Temp: {0:F1}", TempChipMax).PadRight (16));
														break;
													case 9: 
														string url = MiningController.CurrentPool.Url;
														lcd.Write (url.Length > 16 ? url.Substring (url.Length - 16) : url.PadLeft (16));
														break;
													case 10: 
														lcd.Write (String.Format ("Max Temp: {0:F1}", TempChipMax).PadRight (16));
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
			uint nonce = 0;
			byte rounds;
			byte board;

			if (Rx_Buffer [baseptr + 1] == DB_DATA_TYPE_REQUEST_DATA) {
				/*
				typedef __packed struct {
					uint8_t StartSentinel;	//0xaa
					uint8_t DataType;		//0x20
					uint8_t CoreResult[40];	//Nonce not found number for 80 cores.
					uint8_t NeedConfig;			//Require Configuration Data
					uint8_t Reserved[7];	//Reserved for future use
					uint8_t SN;
					uint8_t FinishSentinel;	//0x55
				} StructRequestData;
				*/
				try {
					board = Rx_Buffer [baseptr + DB_DATA_SIZE - 1];
					if (board >= NUMBER_OF_BOARD) {
						throw new Exception ("Incorrect Board Number: " + board.ToString ());
					}
					if (Rx_Buffer [baseptr + 42] == 0x01) {
						Utility.Log (LogLevel.Warning, "Board {0} Needs Config", board);
						SendConfig (board);
						return;
					}
					for (int i = 0; i < 40; i++) {
						byte cr = Rx_Buffer [baseptr + 2 + i];
						rounds = (byte)(cr & 0x0f);
						_hardwareStatus.Board [board].Chip [i >> 1].Core [(i << 1) % 4].NonceNotFound += (long)rounds;
						_megahashes += (long)rounds * 4096;
						rounds = (byte)(cr >> 4);
						_hardwareStatus.Board [board].Chip [i >> 1].Core [((i << 1) % 4) + 1].NonceNotFound += (long)rounds;
						_megahashes += (long)rounds * 4096;
					}
					SendBlockHashData (board, 0);
				} catch (Exception e) {
					byte[] dump = new byte[DB_DATA_SIZE];
					Buffer.BlockCopy (Rx_Buffer, baseptr, dump, 0, DB_DATA_SIZE);
					Utility.Log (LogLevel.Error, "{0}\n{1}", Utility.ByteArrayToHexString (dump), e.ToString ());
				}
			} else if (Rx_Buffer [baseptr + 1] == DB_DATA_TYPE_HASH_RESULT) {
				/*
				typedef __packed struct {
					uint8_t StartSentinel;		//0xaa
					uint8_t DataType;			//0x10
					uint32_t UniqueId;			//32 bit unique Hash Job Id
					uint32_t Nonce;				//Nonce
					uint32_t Difficulty;		//Share difficulty calculated with the result Nonce. 0 means invalid result or no result.
					uint8_t Board;				//Board Number of the result.
					uint8_t Chip;				//Chip Number of the result.
					uint8_t Core;				//Core Number of the result.
					uint8_t BoardTemp;			//Board temprature
					uint8_t ChipTemp[20];		//Chip tempratures
					uint8_t NeedConfig;			//Require Configuration Data
					uint8_t Volt;          		//Voltage
					uint8_t Reserved[10];		//Reserved for future use
					uint8_t SN;
					uint8_t FinishSentinel;		//0x55
				} StructHashResult;
				*/
				try {
					board = Rx_Buffer [baseptr + DB_DATA_SIZE - 1];
					if (board >= NUMBER_OF_BOARD) {
						throw new Exception ("Incorrect Board Number: " + board.ToString ());
					}
					if (Rx_Buffer [baseptr + 28] == 0x01) {
						Utility.Log (LogLevel.Warning, "Board {0} Needs Config", board);
						SendConfig (board);
						return;
					}
					uint uniqueId = ((uint)Rx_Buffer [baseptr + 5] << 24) + ((uint)Rx_Buffer [baseptr + 4] << 16) + ((uint)Rx_Buffer [baseptr + 3] << 8) + ((uint)Rx_Buffer [baseptr + 2]);
					uint resultDiff = ((uint)Rx_Buffer [baseptr + 13] << 24) + ((uint)Rx_Buffer [baseptr + 12] << 16) + ((uint)Rx_Buffer [baseptr + 11] << 8) + ((uint)Rx_Buffer [baseptr + 10]);
					byte core = Rx_Buffer [baseptr + 16];
					int chip = core / 4;
					CoreStatus corestat = _hardwareStatus.Board [board].Chip [chip].Core [core % 4];
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
						
					chip = baseptr;
					foreach (var c in _hardwareStatus.Board[board].Chip) {
						if (Rx_Buffer [18 + chip] != 0) {
							c.Temperature = ((double)Rx_Buffer [18 + chip] / 2) + 25.0;
						}
						chip++;
					}
					if (Rx_Buffer [baseptr + 17] != 0) {
						_hardwareStatus.Board [board].Temperature = ((double)Rx_Buffer [baseptr + 17] / 2) + 25.0;
					}
					
					HashResultReceived (board, core, uniqueId, nonce, resultDiff);
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

							/*
							typedef __packed struct {
								uint8_t StartSentinel;		//0xaa
								uint8_t DataType;			//0x00
								uint8_t PLL;				//ASIC PLL Setting  PLL = Frequency(MHz) / 16  For example to set the frequency to 400MHz, PLL = 400/16 = 25
								uint8_t Voltage1;			//ASIC Core Voltage Voltage = V * 100  For example, to set the core voltage to 0.9V, Voltage = 90
								uint8_t Voltage2;			//ASIC Core Voltage Voltage = V * 100  For example, to set the core voltage to 0.9V, Voltage = 90
								uint8_t Difficulty[4];		//Share Difficulty, should always be power of 2 (Valid numbers are 1,2,4,8,16,etc.)
								uint8_t Reserved[42];		//Reserved for future use
								uint8_t FinishSentinel;		//0x55
							} StructHardwareConfig;
							*/

							Tx_Buffer [tmpPtr] = DB_START_SENTINEL;
							Tx_Buffer [tmpPtr + 1] = DB_DATA_TYPE_CONFIG;
							Tx_Buffer [tmpPtr + 2] = (byte)(_config.Frequency >> 4);
							Tx_Buffer [tmpPtr + 3] = (byte)(_config.Voltage + _overvolt [board].VoltOffset);
							Tx_Buffer [tmpPtr + 4] = (byte)(_config.Voltage + _overvolt [board].VoltOffset);
							Tx_Buffer [tmpPtr + 5] = (byte)((_difficulty >> 24) & 0xff);
							Tx_Buffer [tmpPtr + 6] = (byte)((_difficulty >> 16) & 0xff);
							Tx_Buffer [tmpPtr + 7] = (byte)((_difficulty >> 8) & 0xff);
							Tx_Buffer [tmpPtr + 8] = (byte)((_difficulty >> 0) & 0xff);
							for (int i = 9; i < DB_DATA_SIZE - 1; i++) {
								Tx_Buffer [tmpPtr + i] = 0;
							}
							Tx_Buffer [tmpPtr + DB_DATA_SIZE - 1] = board;

//							byte[] dmp = new byte[DB_DATA_SIZE];
//							Buffer.BlockCopy (Tx_Buffer, Tx_Pointer, dmp, 0, DB_DATA_SIZE);
//							Utility.Log (LogLevel.Debug, "Tx--{0}", Utility.ByteArrayToHexString (dmp));

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
			/*
			typedef __packed struct {
				uint8_t StartSentinel;		//0xaa
				uint8_t BlockType;			//0x01 not refresh, 0x02 refresh
				uint32_t UniqueId;			//32 bit unique Hash Job Id
				uint8_t MidState[32];		//Bitcoin Block Header Midstate
				uint8_t RestData[12];		//Bitcoin Block Header rest data
				uint8_t MaxRoll;			//Max roll count for NTime rolling forward
				uint8_t FinishSentinel;		//0x55
			} StructHashData;
			*/
			if (_state != HardwareState.Stopping) {
				if (refresh) {
					for (int i = 0; i < NUMBER_OF_BOARD; i++) {
						_boardBufferRefresh[i] = true;
					}
				}
				lock (_spilock) {
					if (Tx_Pointer < SPI_PACK_SIZE * DB_DATA_SIZE) {
						Tx_Buffer [Tx_Pointer] = DB_START_SENTINEL;
						if (_boardBufferRefresh [board]) {
							Tx_Buffer [Tx_Pointer + 1] = DB_DATA_TYPE_HASH_DATA_REF;
							_boardBufferRefresh [board] = false;
						} else {
							Tx_Buffer [Tx_Pointer + 1] = DB_DATA_TYPE_HASH_DATA;
						}
						Tx_Buffer [Tx_Pointer + DB_DATA_SIZE - 2] = maxRoll;
						Tx_Buffer [Tx_Pointer + 5] = (byte)((Data.UniqueId >> 24) & 0xff);
						Tx_Buffer [Tx_Pointer + 4] = (byte)((Data.UniqueId >> 16) & 0xff);
						Tx_Buffer [Tx_Pointer + 3] = (byte)((Data.UniqueId >> 08) & 0xff);
						Tx_Buffer [Tx_Pointer + 2] = (byte)((Data.UniqueId >> 00) & 0xff);
						Buffer.BlockCopy (Data.MidStateBin, 0, Tx_Buffer, Tx_Pointer + 6, 32);
						Buffer.BlockCopy (Data.BlockHeaderBin, 64, Tx_Buffer, Tx_Pointer + 38, 12);
						Tx_Buffer [Tx_Pointer + DB_DATA_SIZE - 1] = board;

//						byte[] dmp = new byte[DB_DATA_SIZE];
//						Buffer.BlockCopy (Tx_Buffer, Tx_Pointer, dmp, 0, DB_DATA_SIZE);
//						Utility.Log (LogLevel.Debug, "Tx--{0}", Utility.ByteArrayToHexString (dmp));

						Tx_Pointer += DB_DATA_SIZE;
					} else {
						Utility.Log (LogLevel.Error, "SPI TX Buffer Overflow!");
					}
				}
			}
		}
	}

}

