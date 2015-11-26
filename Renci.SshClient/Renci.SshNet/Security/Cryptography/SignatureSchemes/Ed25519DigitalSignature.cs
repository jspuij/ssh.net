using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renci.SshNet.Security.Cryptography
{
    /// <summary>
    /// Implements the Ed25519 Digital Signature algoritm.
    /// </summary>
    public class Ed25519DigitalSignature : DigitalSignature
    {
        private Ed25519Key ed25519Key;

        public Ed25519DigitalSignature(Ed25519Key ed25519Key)
        {
            this.ed25519Key = ed25519Key;
        }

        public override byte[] Sign(byte[] input)
        {
            throw new NotImplementedException();
        }

        public override bool Verify(byte[] input, byte[] signature)
        {
            throw new NotImplementedException();
        }
    }
}
