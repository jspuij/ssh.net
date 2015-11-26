using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Renci.SshNet.Common;
using Renci.SshNet.Security.Cryptography;

namespace Renci.SshNet.Security.Cryptography
{
    /// <summary>
    /// Contains an Ed25519 Key
    /// </summary>
    public class Ed25519Key : Key, IDisposable
    {
        /// <summary>
        /// The expanded private key.
        /// </summary>
        private byte[] _privateKey;

        /// <summary>
        /// The public key.
        /// </summary>
        private byte[] _publicKey;

        /// <summary>
        /// ED25519 Key Length
        /// </summary>
        private const int ED25519_KEYLENGTH = 32;

        /// <summary>
        /// Creates a new instance of the <see cref="Ed25519Key"/> class.
        /// </summary>
        /// <param name="seedData">The seed data to expand the private key from.</param>
        public Ed25519Key(byte[] seedData)
        {
            if (seedData == null)
                throw new ArgumentNullException("seedData");
            if (seedData.Length != ED25519_KEYLENGTH)
                throw new ArgumentException("seedData");

            var publicKey = new byte[ED25519_KEYLENGTH];
            var privateKey = new byte[ED25519_KEYLENGTH * 2];

            //TODO: Ed25519Operations.crypto_sign_keypair(publicKey, 0, privateKey, 0, seedData, 0);
            _publicKey = publicKey;
            _privateKey = privateKey;
        }



        public override int KeyLength
        {
            get
            {
                return ED25519_KEYLENGTH;
            }
        }

        public override BigInteger[] Public
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        private Ed25519DigitalSignature _digitalSignature;

        /// <summary>
        /// Reference to the digital signature algorithm that
        /// this key is for.
        /// </summary>
        protected override DigitalSignature DigitalSignature
        {
            get
            {
                if (this._digitalSignature == null)
                {
                    this._digitalSignature = new Ed25519DigitalSignature(this);
                }
                return this._digitalSignature;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Disposes the key.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_privateKey != null)
                    {
                        Array.Clear(_privateKey, 0, _privateKey.Length);
                        _privateKey = null;
                        Array.Clear(_publicKey, 0, _publicKey.Length);
                        _privateKey = null;
                    }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
