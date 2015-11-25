using Renci.SshNet.Security.Cryptography.Ciphers;
using Renci.SshNet.Security.Cryptography.Ciphers.Modes;
using Renci.SshNet.Security.Cryptography.Ciphers.Paddings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Renci.SshNet.Security.Cryptography.KeyDerivationFunctions
{
    /// <summary>
    /// Implementation of the OpenBSD bcrypt_pbkdf function.
    /// </summary>
    public class BCryptPBKDF : DeriveBytes
    {
        /// <summary>
        /// Hash size of the BCrypt function in bytes.
        /// </summary>
        private const int BCRYPT_HASHSIZE = 32;

        /// <summary>
        /// Password hash field.
        /// </summary>
        private readonly byte[] _sha512password;

        /// <summary>
        /// Iteration count field.
        /// </summary>
        private int _iterationCount;

        /// <summary>
        /// Salt field.
        /// </summary>
        private byte[] _salt;

        /// <summary>
        /// Derived bytes field.
        /// </summary>
        private byte[] _derivedBytes;

        /// <summary>
        /// Derived bytes index.
        /// </summary>
        private int _derivedBytesIndex = 0;

        /// <summary>
        /// Bytes of the magic string.
        /// </summary>
        private static readonly byte[] _magic;

        /// <summary>
        /// Static constructor. sets up magic string.
        /// </summary>
        static BCryptPBKDF()
        {
            _magic = Encoding.ASCII.GetBytes("OxychromaticBlowfishSwatDynamite");
        }

        /// <summary>
        /// Gets or Sets the Iteration Count.
        /// </summary>
        public int IterationCount
        {
            get
            {
                return _iterationCount;
            }

            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "Needs to be positive");
                _iterationCount = value;
                Initialize();
            }
        }

        /// <summary>
        /// Gets or Sets the Salt.
        /// </summary>
        public byte[] Salt
        {
            get
            {
                return _salt;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (value.Length < 1)
                    throw new ArgumentException("value", "Salt length too small.");
                _salt = value;
                Initialize();
            }
        }

        /// <summary>
        /// Created a new instance of the <see cref="BCryptPBKDF"/> class.
        /// </summary>
        /// <param name="password">The password phrase to use</param>
        /// <param name="salt">The salt to use.</param>
        /// <param name="iterationCount">The number of rounds of BCrypt_PBKDF that need to be supported.</param>
        /// <param name="totalDerivedLength">Total derived length.</param>
        public BCryptPBKDF(string password, byte[] salt, int iterationCount, int totalDerivedLength)
        {
            if (iterationCount <= 0)
                throw new ArgumentOutOfRangeException("iterationCount", "Needs to be positive");
            if (salt == null)
                throw new ArgumentNullException("salt");
            if (salt.Length < 1)
                throw new ArgumentException("salt", "Salt length needs to be at least 1.");
            if (password == null)
                throw new ArgumentNullException("password");
            if (password.Length < 1)
                throw new ArgumentException("password", "Password length needs to be at least 1.");
            if (totalDerivedLength < 1)
                throw new ArgumentOutOfRangeException("totalDerivedLength", "Needs to be at least 1.");

            var sha512Hash = new SHA512Hash();
            _sha512password = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            _salt = salt;
            _iterationCount = iterationCount;
            _derivedBytes = new byte[totalDerivedLength];
            Initialize();
        }

        /// <summary>
        /// Derives bytes from the password based on the initialization arguments.
        /// </summary>
        /// <param name="numberOfBytes">The number of bytes to derive</param>
        /// <returns>An array of bytes of the specified length.</returns>
        public override byte[] GetBytes(int numberOfBytes)
        {
            if (numberOfBytes > _derivedBytes.Length - _derivedBytesIndex)
                throw new ArgumentOutOfRangeException("numberOfBytes", string.Format("Number of bytes ({0}) not available anymore."));

            var result = new byte[numberOfBytes];

            Buffer.BlockCopy(_derivedBytes, _derivedBytesIndex, result, 0, numberOfBytes);
            _derivedBytesIndex += numberOfBytes;

            return result;
        }

        /// <summary>
        /// Resets the state of operation.
        /// </summary>
        public override void Reset()
        {
            Initialize();
        }

        /// <summary>
        /// Implementation of the BCrypt hashing function.
        /// </summary>
        /// <seealso href="https://github.com/dchest/bcrypt_pbkdf" />
        /// <seealso href="http://www.tedunangst.com/flak/post/bcrypt-pbkdf" />
        /// <seealso href="http://cvsweb.openbsd.org/cgi-bin/cvsweb/src/lib/libutil/bcrypt_pbkdf.c" />
        /// <param name="shaPassword">The SHA512 derived password to use.</param>
        /// <param name="shaSalt">The SHA512 derived salt to use.</param>
        internal static byte[] BCryptHash(byte[] shaPassword, byte[] shaSalt)
        {
            var result = new byte[BCRYPT_HASHSIZE];

            var blowfishCipher = new BlowfishCipher(shaPassword, shaSalt, null, null);

            for (int i = 0; i < 64; i++)
            {
                blowfishCipher.ExpandKey(shaSalt, null);
                blowfishCipher.ExpandKey(shaPassword, null);
            }

            Buffer.BlockCopy(_magic, 0, result, 0, BCRYPT_HASHSIZE);

            for (int i = 0; i < BCRYPT_HASHSIZE; i += 8)
            {
                for (int j = 0; j < 64; j++)
                {
                    blowfishCipher.EncryptBlock(result, i, 8, result, i);
                }
            }

            for (int i = 0; i < BCRYPT_HASHSIZE; i += 4)
            {
                byte swap;

                swap = result[i + 3];
                result[i + 3] = result[i];
                result[i] = swap;
                swap = result[i + 2];
                result[i + 2] = result[i + 1];
                result[i + 1] = swap;
            }

            return result;
        }

        /// <summary>
        /// Initializes the Key Derivation Function
        /// </summary>
        private void Initialize()
        {
            _derivedBytesIndex = 0;

            byte[] output = new byte[BCRYPT_HASHSIZE];
            byte[] tmp;
            var cnt = new byte[4];
            var sha512Hash = new SHA512Hash();

            int numberOfBlocks = (_derivedBytes.Length + BCRYPT_HASHSIZE - 1) / BCRYPT_HASHSIZE;

            // result array

            byte[] key = new byte[numberOfBlocks * BCRYPT_HASHSIZE];

            for (int b = 1; b <= numberOfBlocks; b++)
            {
                sha512Hash.Initialize();
                sha512Hash.TransformBlock(this.Salt, 0, this.Salt.Length, this.Salt, 0);
                cnt[0] = (byte)(b >> 24);
                cnt[1] = (byte)(b >> 16);
                cnt[2] = (byte)(b >> 8);
                cnt[3] = (byte)(b);
                sha512Hash.TransformFinalBlock(cnt, 0, cnt.Length);
                tmp = BCryptHash(_sha512password, sha512Hash.Hash);
                Buffer.BlockCopy(tmp, 0, output, 0, BCRYPT_HASHSIZE);

                for (int i = 2; i <= this.IterationCount; i++)
                {
                    sha512Hash.Initialize();
                    tmp = BCryptHash(_sha512password, sha512Hash.ComputeHash(tmp));
                    for (int j = 0; j < BCRYPT_HASHSIZE; j++)
                    {
                        output[j] ^= tmp[j];
                    }

                    for (int k = 0; k < BCRYPT_HASHSIZE; k++)
                    {
                        key[k * numberOfBlocks + (b - 1)] = output[k];
                    }
                }
            }

            Buffer.BlockCopy(key, 0, _derivedBytes, 0, _derivedBytes.Length);
        }

    }

}
