using System;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;

namespace FusionMiner
{
	public static class Aes
	{
		private static readonly byte[] SALT = new byte[] {
			0xDB, 0x4D, 0x47, 0x01, 0xE9, 0xF1, 0xB9, 0x70, 0x29, 0x71, 0x81, 0xD7, 0x49, 0x09, 0xF1, 0x11
		};
		private static readonly string ID = "ZL1JI";

		public static byte[] Encrypt (byte[] plain)
		{
			MemoryStream memoryStream;
			CryptoStream cryptoStream;
			Rijndael rijndael = Rijndael.Create ();
			Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes (ID, SALT);
			rijndael.Key = pdb.GetBytes (32);
			rijndael.IV = pdb.GetBytes (16);
			memoryStream = new MemoryStream ();
			cryptoStream = new CryptoStream (memoryStream, rijndael.CreateEncryptor (), CryptoStreamMode.Write);
			cryptoStream.Write (plain, 0, plain.Length);
			cryptoStream.Close ();
			return memoryStream.ToArray ();
		}

		public static byte[] Decrypt (byte[] cipher)
		{
			MemoryStream memoryStream;
			CryptoStream cryptoStream;
			Rijndael rijndael = Rijndael.Create ();
			Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes (ID, SALT);
			rijndael.Key = pdb.GetBytes (32);
			rijndael.IV = pdb.GetBytes (16);
			memoryStream = new MemoryStream ();
			cryptoStream = new CryptoStream (memoryStream, rijndael.CreateDecryptor (), CryptoStreamMode.Write);
			cryptoStream.Write (cipher, 0, cipher.Length);
			cryptoStream.Close ();
			return memoryStream.ToArray ();
		}

		public static string EncryptString (string toEncrypt)
		{
			if ((toEncrypt!=null)&&(toEncrypt.Length > 0)) {
				byte[] resultArray = Encrypt (UTF8Encoding.UTF8.GetBytes (toEncrypt));
				return Convert.ToBase64String (resultArray, 0, resultArray.Length);
			} else {
				return "";
			}
		}

		public static string DecryptString (string cipherString)
		{
			if ((cipherString != null)&&(cipherString.Length > 0)) {
				byte[] resultArray = Decrypt (Convert.FromBase64String (cipherString));
				return UTF8Encoding.UTF8.GetString (resultArray);
			} else {
				return "";
			}
		}
	}
}

