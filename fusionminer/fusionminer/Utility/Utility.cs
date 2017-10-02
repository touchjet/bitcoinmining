using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace FusionMiner
{
	public enum LogLevel
	{
		Debug,
		Info,
		Warning,
		Error}
	;

	public static class Utility
	{
		public static bool DebugMode = false;
		public static bool SilentMode = false;

		public static bool IsIPv4 (string ipAddress)
		{
			return Regex.IsMatch (ipAddress, @"^\d{1,3}(\.\d{1,3}){3}$") &&
			ipAddress.Split ('.').SingleOrDefault (s => int.Parse (s) > 255) == null;
		}

		public static string LocalIPAddress ()
		{
			IPHostEntry host;
			string localIP = "";
			try {
				host = Dns.GetHostEntry (Dns.GetHostName ());
				foreach (IPAddress ip in host.AddressList) {
					if (ip.AddressFamily == AddressFamily.InterNetwork) {
						localIP = ip.ToString ();
						break;
					}
				}
			} catch {
			}
			return localIP;
		}

		public static string InternetIPAddress ()
		{
			string internetIP = "";
			try {
				WebRequest wrGETURL = WebRequest.Create ("http://ip-api.com/line");
				Stream objStream = wrGETURL.GetResponse ().GetResponseStream ();
				using (StreamReader objReader = new StreamReader (objStream)) {
					string ipLine = "";
					while (ipLine != null) {
						ipLine = objReader.ReadLine ();
						if (ipLine != null)
							internetIP = ipLine;
					}
				}
				if (!IsIPv4 (internetIP)) {
					internetIP = "";
				}
			} catch {
			}
			return internetIP;
		}

		private static Dictionary<string,string> _ipLocation;

		public static string FindCityByIP (string ip)
		{
			string city = "";
			if (!IsIPv4 (ip)) {
				return city;
			}
			if (_ipLocation == null) {
				_ipLocation = new Dictionary<string, string> ();
			}
			if (_ipLocation.ContainsKey (ip)) {
				return _ipLocation [ip];
			}
			try {
				WebRequest wrGETURL = WebRequest.Create ("http://ip-api.com/csv/" + ip);
				Stream objStream = wrGETURL.GetResponse ().GetResponseStream ();
				using (StreamReader objReader = new StreamReader (objStream)) {
					string ipLine = objReader.ReadLine ();
					if (ipLine != null) {
						if (ipLine.StartsWith("success")) {
							for (int i=0;i<5;i++) {
								ipLine = ipLine.Substring(ipLine.IndexOf(',')+1);
							}
							city = ipLine.Substring(0,ipLine.IndexOf(','));
							_ipLocation.Add(ip,city);
						}
					}
				}
			} catch {
			}
			return city;
		}

		public static string ReverseWord (string input)
		{
			string result = "";
			int len = input.Length;
			for (int j = 0; j < len / 8; j++) {
				result += input.Substring ((j * 8) + 6, 2);
				result += input.Substring ((j * 8) + 4, 2);
				result += input.Substring ((j * 8) + 2, 2);
				result += input.Substring ((j * 8) + 0, 2);
			}
			return result;
		}

		public static void ReverseWord (byte[] input)
		{
			int len = input.Length;
			byte t;
			for (int j = 0; j < len / 4; j++) {
				t = input [j * 4 + 3];
				input [j * 4 + 3] = input [j * 4 + 0];
				input [j * 4 + 0] = t;
				t = input [j * 4 + 2];
				input [j * 4 + 2] = input [j * 4 + 1];
				input [j * 4 + 1] = t;
			}
		}

		public static string ReverseAll (string input)
		{
			string result = "";
			int len = input.Length;
			for (int j = 0; j < len / 2; j++) {
				result += input.Substring (len - (j * 2) - 2, 2);
			}
			return result;
		}

		public static void ReverseAll (ref byte[] input)
		{
			byte t;
			int len = input.Length;
			for (int j = 0; j < len / 2; j++) {
				t = input [len - 1 - j];
				input [len - 1 - j] = input [j];
				input [j] = t;
			}
		}
		//		public static byte[] HexStringToByteArray (string hex)
		//		{
		//			return Enumerable.Range (0, hex.Length)
		//				.Where (x => x % 2 == 0)
		//					.Select (x => Convert.ToByte (hex.Substring (x, 2), 16))
		//					.ToArray ();
		//		}
		public static byte[] HexStringToByteArray (string hex)
		{
			if (hex.Length % 2 == 1)
				throw new Exception ("The binary key cannot have an odd number of digits");

			byte[] arr = new byte[hex.Length >> 1];

			for (int i = 0; i < hex.Length >> 1; ++i) {
				arr [i] = (byte)((GetHexVal (hex [i << 1]) << 4) + (GetHexVal (hex [(i << 1) + 1])));
			}

			return arr;
		}

		public static void HexStringToByteArray (string hex, byte[] arr, int binoffset, int binlen = 0)
		{
			if (hex.Length % 2 == 1)
				throw new Exception ("The binary key cannot have an odd number of digits");

			if (binlen == 0) {
				binlen = hex.Length >> 1;
			}
			if (arr.Length < (binlen + binoffset))
				throw new Exception ("Length mismatch");

			for (int i = 0; i < binlen; ++i) {
				arr [i + binoffset] = (byte)((GetHexVal (hex [i << 1]) << 4) + (GetHexVal (hex [(i << 1) + 1])));
			}
		}

		public static int GetHexVal (char hex)
		{
			int val = (int)hex;
			//For uppercase A-F letters:
			//return val - (val < 58 ? 48 : 55);
			//For lowercase a-f letters:
			//return val - (val < 58 ? 48 : 87);
			//Or the two combined, but a bit slower:
			return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
		}
		//		public static string ByteArrayToHexString (byte[] ba)
		//		{
		//			string hex = BitConverter.ToString (ba);
		//			return hex.Replace ("-", "").ToLower ();
		//		}
		/// <summary>
		/// Hex string lookup table.
		/// </summary>
		private static readonly string[] HexStringTable = new string[] {
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0a", "0b", "0c", "0d", "0e", "0f",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1a", "1b", "1c", "1d", "1e", "1f",
			"20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2a", "2b", "2c", "2d", "2e", "2f",
			"30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3a", "3b", "3c", "3d", "3e", "3f",
			"40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4a", "4b", "4c", "4d", "4e", "4f",
			"50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5a", "5b", "5c", "5d", "5e", "5f",
			"60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6a", "6b", "6c", "6d", "6e", "6f",
			"70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7a", "7b", "7c", "7d", "7e", "7f",
			"80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8a", "8b", "8c", "8d", "8e", "8f",
			"90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9a", "9b", "9c", "9d", "9e", "9f",
			"a0", "a1", "a2", "a3", "a4", "a5", "a6", "a7", "a8", "a9", "aa", "ab", "ac", "ad", "ae", "af",
			"b0", "b1", "b2", "b3", "b4", "b5", "b6", "b7", "b8", "b9", "ba", "bb", "bc", "bd", "be", "bf",
			"c0", "c1", "c2", "c3", "c4", "c5", "c6", "c7", "c8", "c9", "ca", "cb", "cc", "cd", "ce", "cf",
			"d0", "d1", "d2", "d3", "d4", "d5", "d6", "d7", "d8", "d9", "da", "db", "dc", "dd", "de", "df",
			"e0", "e1", "e2", "e3", "e4", "e5", "e6", "e7", "e8", "e9", "ea", "eb", "ec", "ed", "ee", "ef",
			"f0", "f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8", "f9", "fa", "fb", "fc", "fd", "fe", "ff"
		};

		/// <summary>
		/// Returns a hex string representation of an array of bytes.
		/// </summary>
		/// <param name="value">The array of bytes.</param>
		/// <returns>A hex string representation of the array of bytes.</returns>
		public static string ByteArrayToHexString (this byte[] value)
		{
			StringBuilder stringBuilder = new StringBuilder ();
			if (value != null) {
				foreach (byte b in value) {
					stringBuilder.Append (HexStringTable [b]);
				}
			}

			return stringBuilder.ToString ();
		}

		private static object loglock = new object ();

		public static void Log (LogLevel level, string message)
		{
			Log (level, message, null);
		}

		public static void Log (LogLevel level, string message, object arg1)
		{
			Log (level, message, new object[1]{ arg1 });
		}

		public static void Log (LogLevel level, string message, object arg1, object arg2)
		{
			Log (level, message, new object[2]{ arg1, arg2 });
		}

		public static void Log (LogLevel level, string message, object arg1, object arg2, object arg3)
		{
			Log (level, message, new object[3]{ arg1, arg2, arg3 });
		}

		public static void Log (LogLevel level, string message, object[] values)
		{
			if ((!DebugMode) && (level < LogLevel.Warning)) {
				return;
			}
			string logStr;
			if ((values == null) || (values.Length == 0)) {
				logStr = String.Format ("[{0} {1}] - {2}", DateTime.UtcNow.ToShortDateString(), DateTime.UtcNow.ToLongTimeString (), message);
			} else {
				logStr = String.Format ("[{0} {1}] - {2}", DateTime.UtcNow.ToShortDateString(), DateTime.UtcNow.ToLongTimeString (), String.Format (message, values));
			}

			if (!SilentMode) {
				Console.WriteLine (logStr);
			}
			lock (loglock) {
				try {
					using (StreamWriter w = File.AppendText ("/var/log/fusionminer.log")) {
						w.WriteLine (logStr);
						w.Close ();
					}
				} catch {
				}
			}
		}

		public static void PurgeLog ()
		{
			try {
				if (File.Exists ("/var/log/fusionminer.log")) {
					var lines = File.ReadLines ("/var/log/fusionminer.log").ToList ();
					if (lines.Count > 2000) {
						File.WriteAllLines ("/var/log/fusionminer.log", lines.Skip (lines.Count - 1500).ToArray ());
					}
				}
			} catch {
			}
		}

		public static string GetMd5Hash (byte[] input)
		{
			using (MD5 md5Hash = MD5.Create ()) {
				// Convert the input string to a byte array and compute the hash. 
				byte[] data = md5Hash.ComputeHash (input);

				// Create a new Stringbuilder to collect the bytes 
				// and create a string.
				StringBuilder sBuilder = new StringBuilder ();

				// Loop through each byte of the hashed data  
				// and format each one as a hexadecimal string. 
				for (int i = 0; i < data.Length; i++) {
					sBuilder.Append (data [i].ToString ("x2"));
				}

				// Return the hexadecimal string. 
				return sBuilder.ToString ();
			}
		}

		public static bool VerifyMd5Hash (byte[] input, string hash)
		{
			// Hash the input. 
			string hashOfInput = GetMd5Hash (input);

			// Create a StringComparer an compare the hashes.
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;

			if (0 == comparer.Compare (hashOfInput, hash)) {
				return true;
			} else {
				return false;
			}
		}

	}
}

