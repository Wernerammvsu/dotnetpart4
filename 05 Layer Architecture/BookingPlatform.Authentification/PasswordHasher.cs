using BookingPlatform.Domain.Service;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BookingPlatform.Authentification
{
	public class PasswordHasher : IPasswordHasher
	{
		private readonly RandomNumberGenerator _randomNumberGenerator;
		private readonly KeyDerivationPrf _keyDerivationPrf;

		private readonly int _iterCount;
		private readonly int _saltSize;
		private readonly int _numBytesRequested;

		public PasswordHasher()
		{
			_randomNumberGenerator = RandomNumberGenerator.Create();
			_keyDerivationPrf = KeyDerivationPrf.HMACSHA256;
			_iterCount = 10000;
			_saltSize = 128 / 8;
			_numBytesRequested = 256 / 8;
		}

		public string HashPassword(string password)
		{
			// Produce a version 3 (see comment above) text hash.
			byte[] salt = new byte[_saltSize];
			_randomNumberGenerator.GetBytes(salt);
			byte[] subkey = KeyDerivation.Pbkdf2(password, salt, _keyDerivationPrf, _iterCount, _numBytesRequested);

			var outputBytes = new byte[13 + salt.Length + subkey.Length];
			outputBytes[0] = 0x01; // format marker
			WriteNetworkByteOrder(outputBytes, 1, (uint)_keyDerivationPrf);
			WriteNetworkByteOrder(outputBytes, 5, (uint)_iterCount);
			WriteNetworkByteOrder(outputBytes, 9, (uint)_saltSize);
			Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
			Buffer.BlockCopy(subkey, 0, outputBytes, 13 + _saltSize, subkey.Length);
			return Convert.ToBase64String(outputBytes);
		}

		public bool VerifyHashedPassword(string hashedPasswordString, string password)
		{
			byte[] hashedPassword = Convert.FromBase64String(hashedPasswordString);
			try
			{
				// Read header information
				KeyDerivationPrf prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
				int iterCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
				int saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

				if (iterCount < _iterCount)
				{
					return false;
				}

				// Read the salt: must be >= 128 bits
				if (saltLength < 128 / 8)
				{
					return false;
				}
				byte[] salt = new byte[saltLength];
				Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

				// Read the subkey (the rest of the payload): must be >= 128 bits
				int subkeyLength = hashedPassword.Length - 13 - salt.Length;
				if (subkeyLength < 128 / 8)
				{
					return false;
				}
				byte[] expectedSubkey = new byte[subkeyLength];
				Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

				// Hash the incoming password and verify it
				byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subkeyLength);
				return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
			}
			catch
			{
				// This should never occur except in the case of a malformed payload, where
				// we might go off the end of the array. Regardless, a malformed payload
				// implies verification failed.
				return false;
			}
		}

		private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
		{
			buffer[offset + 0] = (byte)(value >> 24);
			buffer[offset + 1] = (byte)(value >> 16);
			buffer[offset + 2] = (byte)(value >> 8);
			buffer[offset + 3] = (byte)(value >> 0);
		}

		private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
		{
			return ((uint)(buffer[offset + 0]) << 24)
				| ((uint)(buffer[offset + 1]) << 16)
				| ((uint)(buffer[offset + 2]) << 8)
				| ((uint)(buffer[offset + 3]));
		}
	}
}