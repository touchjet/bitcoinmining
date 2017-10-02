using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System;

namespace FusionMiner
{
	[ComVisible (true)]
	public class SHA256Extended : SHA256
	{
		private const int BLOCK_SIZE_BYTES = 64;
		private uint[] _H;
		private ulong count;
		private byte[] _ProcessingBuffer;
		// Used to start data when passed less than a block worth.
		private int _ProcessingBufferCount;
		// Counts how much data we have stored that still needs processed.
		private uint[] buff;

		public SHA256Extended ()
		{
			_H = new uint [8];
			_ProcessingBuffer = new byte [BLOCK_SIZE_BYTES];
			buff = new uint[64];
			Initialize ();
		}

		public void DoubleSha256 (byte[] buffer, int length, byte[] result)
		{
			HashCore (buffer, 0, length);
			HashFinal (result);
			Initialize ();
			HashCore (result, 0, 32);
			HashFinal (result);
			Initialize ();
		}

		public void ComputeMidstate (byte[] buffer, byte[] midstate)
		{
			int i, j;
			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			HashCore (buffer, 0, 64);
			for (i = 0; i < 8; i++) {
				for (j = 0; j < 4; j++) {
					midstate [i * 4 + j] = (byte)(_H [i] >> (24 - j * 8));
				}
			}

			State = 0;
			Initialize ();
		}

		protected override void HashCore (byte[] rgb, int ibStart, int cbSize)
		{
			int i;
			State = 1;

			if (_ProcessingBufferCount != 0) {
				if (cbSize < (BLOCK_SIZE_BYTES - _ProcessingBufferCount)) {
					System.Buffer.BlockCopy (rgb, ibStart, _ProcessingBuffer, _ProcessingBufferCount, cbSize);
					_ProcessingBufferCount += cbSize;
					return;
				} else {
					i = (BLOCK_SIZE_BYTES - _ProcessingBufferCount);
					System.Buffer.BlockCopy (rgb, ibStart, _ProcessingBuffer, _ProcessingBufferCount, i);
					ProcessBlock (_ProcessingBuffer, 0);
					_ProcessingBufferCount = 0;
					ibStart += i;
					cbSize -= i;
				}
			}

			for (i = 0; i < cbSize - cbSize % BLOCK_SIZE_BYTES; i += BLOCK_SIZE_BYTES) {
				ProcessBlock (rgb, ibStart + i);
			}

			if (cbSize % BLOCK_SIZE_BYTES != 0) {
				System.Buffer.BlockCopy (rgb, cbSize - cbSize % BLOCK_SIZE_BYTES + ibStart, _ProcessingBuffer, 0, cbSize % BLOCK_SIZE_BYTES);
				_ProcessingBufferCount = cbSize % BLOCK_SIZE_BYTES;
			}
		}

		protected override byte[] HashFinal ()
		{
			byte[] hash = new byte[32];
			int i, j;

			ProcessFinalBlock (_ProcessingBuffer, 0, _ProcessingBufferCount);

			for (i = 0; i < 8; i++) {
				for (j = 0; j < 4; j++) {
					hash [i * 4 + j] = (byte)(_H [i] >> (24 - j * 8));
				}
			}

			State = 0;
			return hash;
		}

		private void HashFinal (byte[] hash)
		{
			int i, j;

			ProcessFinalBlock (_ProcessingBuffer, 0, _ProcessingBufferCount);

			for (i = 0; i < 8; i++) {
				for (j = 0; j < 4; j++) {
					hash [i * 4 + j] = (byte)(_H [i] >> (24 - j * 8));
				}
			}

			State = 0;
		}

		public override void Initialize ()
		{
			count = 0;
			_ProcessingBufferCount = 0;

			_H [0] = 0x6A09E667;
			_H [1] = 0xBB67AE85;
			_H [2] = 0x3C6EF372;
			_H [3] = 0xA54FF53A;
			_H [4] = 0x510E527F;
			_H [5] = 0x9B05688C;
			_H [6] = 0x1F83D9AB;
			_H [7] = 0x5BE0CD19;
		}

		private void ProcessBlock (byte[] inputBuffer, int inputOffset)
		{
			uint a, b, c, d, e, f, g, h;
			uint t1, t2;
			int i;
			uint[] K1 = SHAConstants.K1;
			uint[] buff = this.buff;

			count += BLOCK_SIZE_BYTES;

			for (i = 0; i < 16; i++) {
				buff [i] = (uint)(((inputBuffer [inputOffset + 4 * i]) << 24)
				| ((inputBuffer [inputOffset + 4 * i + 1]) << 16)
				| ((inputBuffer [inputOffset + 4 * i + 2]) << 8)
				| ((inputBuffer [inputOffset + 4 * i + 3])));
			}


			for (i = 16; i < 64; i++) {
				t1 = buff [i - 15];
				t1 = (((t1 >> 7) | (t1 << 25)) ^ ((t1 >> 18) | (t1 << 14)) ^ (t1 >> 3));

				t2 = buff [i - 2];
				t2 = (((t2 >> 17) | (t2 << 15)) ^ ((t2 >> 19) | (t2 << 13)) ^ (t2 >> 10));
				buff [i] = t2 + buff [i - 7] + t1 + buff [i - 16];
			}

			a = _H [0];
			b = _H [1];
			c = _H [2];
			d = _H [3];
			e = _H [4];
			f = _H [5];
			g = _H [6];
			h = _H [7];

			for (i = 0; i < 64; i++) {
				t1 = h + (((e >> 6) | (e << 26)) ^ ((e >> 11) | (e << 21)) ^ ((e >> 25) | (e << 7))) + ((e & f) ^ (~e & g)) + K1 [i] + buff [i];

				t2 = (((a >> 2) | (a << 30)) ^ ((a >> 13) | (a << 19)) ^ ((a >> 22) | (a << 10)));
				t2 = t2 + ((a & b) ^ (a & c) ^ (b & c));
				h = g;
				g = f;
				f = e;
				e = d + t1;
				d = c;
				c = b;
				b = a;
				a = t1 + t2;
			}

			_H [0] += a;
			_H [1] += b;
			_H [2] += c;
			_H [3] += d;
			_H [4] += e;
			_H [5] += f;
			_H [6] += g;
			_H [7] += h;
//			foreach (var t in _H) {
//				Console.Write("{0:X} ", t);
//			}
//			Console.WriteLine("");
		}

		private void ProcessFinalBlock (byte[] inputBuffer, int inputOffset, int inputCount)
		{
			ulong total = count + (ulong)inputCount;
			int paddingSize = (56 - (int)(total % BLOCK_SIZE_BYTES));

			if (paddingSize < 1)
				paddingSize += BLOCK_SIZE_BYTES;

			byte[] fooBuffer = new byte[inputCount + paddingSize + 8];

			for (int i = 0; i < inputCount; i++) {
				fooBuffer [i] = inputBuffer [i + inputOffset];
			}

			fooBuffer [inputCount] = 0x80;
			for (int i = inputCount + 1; i < inputCount + paddingSize; i++) {
				fooBuffer [i] = 0x00;
			}

			// I deal in bytes. The algorithm deals in bits.
			ulong size = total << 3;
			AddLength (size, fooBuffer, inputCount + paddingSize);
			ProcessBlock (fooBuffer, 0);

			if (inputCount + paddingSize + 8 == 128) {
				ProcessBlock (fooBuffer, 64);
			}
		}

		internal void AddLength (ulong length, byte[] buffer, int position)
		{
			buffer [position++] = (byte)(length >> 56);
			buffer [position++] = (byte)(length >> 48);
			buffer [position++] = (byte)(length >> 40);
			buffer [position++] = (byte)(length >> 32);
			buffer [position++] = (byte)(length >> 24);
			buffer [position++] = (byte)(length >> 16);
			buffer [position++] = (byte)(length >> 8);
			buffer [position] = (byte)(length);
		}
	}

	internal static class SHAConstants
	{
		// SHA-256 Constants
		// Represent the first 32 bits of the fractional parts of the
		// cube roots of the first sixty-four prime numbers
		public readonly static uint[] K1 = {
			0x428A2F98, 0x71374491, 0xB5C0FBCF, 0xE9B5DBA5,
			0x3956C25B, 0x59F111F1, 0x923F82A4, 0xAB1C5ED5,
			0xD807AA98, 0x12835B01, 0x243185BE, 0x550C7DC3,
			0x72BE5D74, 0x80DEB1FE, 0x9BDC06A7, 0xC19BF174,
			0xE49B69C1, 0xEFBE4786, 0x0FC19DC6, 0x240CA1CC,
			0x2DE92C6F, 0x4A7484AA, 0x5CB0A9DC, 0x76F988DA,
			0x983E5152, 0xA831C66D, 0xB00327C8, 0xBF597FC7,
			0xC6E00BF3, 0xD5A79147, 0x06CA6351, 0x14292967,
			0x27B70A85, 0x2E1B2138, 0x4D2C6DFC, 0x53380D13,
			0x650A7354, 0x766A0ABB, 0x81C2C92E, 0x92722C85,
			0xA2BFE8A1, 0xA81A664B, 0xC24B8B70, 0xC76C51A3,
			0xD192E819, 0xD6990624, 0xF40E3585, 0x106AA070,
			0x19A4C116, 0x1E376C08, 0x2748774C, 0x34B0BCB5,
			0x391C0CB3, 0x4ED8AA4A, 0x5B9CCA4F, 0x682E6FF3,
			0x748F82EE, 0x78A5636F, 0x84C87814, 0x8CC70208,
			0x90BEFFFA, 0xA4506CEB, 0xBEF9A3F7, 0xC67178F2
		};
		// SHA-384 and SHA-512 Constants
		// Represent the first 64 bits of the fractional parts of the
		// cube roots of the first sixty-four prime numbers
		public readonly static ulong[] K2 = {
			0x428a2f98d728ae22L, 0x7137449123ef65cdL, 0xb5c0fbcfec4d3b2fL, 0xe9b5dba58189dbbcL,
			0x3956c25bf348b538L, 0x59f111f1b605d019L, 0x923f82a4af194f9bL, 0xab1c5ed5da6d8118L,
			0xd807aa98a3030242L, 0x12835b0145706fbeL, 0x243185be4ee4b28cL, 0x550c7dc3d5ffb4e2L,
			0x72be5d74f27b896fL, 0x80deb1fe3b1696b1L, 0x9bdc06a725c71235L, 0xc19bf174cf692694L,
			0xe49b69c19ef14ad2L, 0xefbe4786384f25e3L, 0x0fc19dc68b8cd5b5L, 0x240ca1cc77ac9c65L,
			0x2de92c6f592b0275L, 0x4a7484aa6ea6e483L, 0x5cb0a9dcbd41fbd4L, 0x76f988da831153b5L,
			0x983e5152ee66dfabL, 0xa831c66d2db43210L, 0xb00327c898fb213fL, 0xbf597fc7beef0ee4L,
			0xc6e00bf33da88fc2L, 0xd5a79147930aa725L, 0x06ca6351e003826fL, 0x142929670a0e6e70L,
			0x27b70a8546d22ffcL, 0x2e1b21385c26c926L, 0x4d2c6dfc5ac42aedL, 0x53380d139d95b3dfL,
			0x650a73548baf63deL, 0x766a0abb3c77b2a8L, 0x81c2c92e47edaee6L, 0x92722c851482353bL,
			0xa2bfe8a14cf10364L, 0xa81a664bbc423001L, 0xc24b8b70d0f89791L, 0xc76c51a30654be30L,
			0xd192e819d6ef5218L, 0xd69906245565a910L, 0xf40e35855771202aL, 0x106aa07032bbd1b8L,
			0x19a4c116b8d2d0c8L, 0x1e376c085141ab53L, 0x2748774cdf8eeb99L, 0x34b0bcb5e19b48a8L,
			0x391c0cb3c5c95a63L, 0x4ed8aa4ae3418acbL, 0x5b9cca4f7763e373L, 0x682e6ff3d6b2b8a3L,
			0x748f82ee5defb2fcL, 0x78a5636f43172f60L, 0x84c87814a1f0ab72L, 0x8cc702081a6439ecL,
			0x90befffa23631e28L, 0xa4506cebde82bde9L, 0xbef9a3f7b2c67915L, 0xc67178f2e372532bL,
			0xca273eceea26619cL, 0xd186b8c721c0c207L, 0xeada7dd6cde0eb1eL, 0xf57d4f7fee6ed178L,
			0x06f067aa72176fbaL, 0x0a637dc5a2c898a6L, 0x113f9804bef90daeL, 0x1b710b35131c471bL,
			0x28db77f523047d84L, 0x32caab7b40c72493L, 0x3c9ebe0a15c9bebcL, 0x431d67c49c100d4cL,
			0x4cc5d4becb3e42b6L, 0x597f299cfc657e2aL, 0x5fcb6fab3ad6faecL, 0x6c44198c4a475817L
		};
	}
}

