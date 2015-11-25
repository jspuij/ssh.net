using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Renci.SshNet.Tests.Common;
using Renci.SshNet.Security.Cryptography.KeyDerivationFunctions;

namespace Renci.SshNet.Tests.Classes.Security.Cryptography.KeyDerivationFunctions
{
    [TestClass]
    public class BCryptPBKDFTest : TestBase
    {
        [TestMethod]
        public void BCryptTest()
        {
            int rounds = 12;
            string password = "password";
            byte[] salt = Encoding.UTF8.GetBytes("salt");
            var output = new byte[] {
                0x1a, 0xe4, 0x2c, 0x05, 0xd4, 0x87, 0xbc, 0x02,
                0xf6, 0x49, 0x21, 0xa4, 0xeb, 0xe4, 0xea, 0x93,
                0xbc, 0xac, 0xfe, 0x13, 0x5f, 0xda, 0x99, 0x97,
                0x4c, 0x06, 0xb7, 0xb0, 0x1f, 0xae, 0x14, 0x9a
            };

            var bcrypt = new BCryptPBKDF(password, salt, rounds, output.Length);

            var actual = bcrypt.GetBytes(4);
        }

        [TestMethod]
        public void BcryptHashTest()
        {
            var output = new byte[] {
                0x87, 0x90, 0x48, 0x70, 0xee, 0xf9, 0xde, 0xdd,
                0xf8, 0xe7, 0x61, 0x1a, 0x14, 0x01, 0x06, 0xe6,
                0xaa, 0xf1, 0xa3, 0x63, 0xd9, 0xa2, 0xc5, 0x04,
                0xdb, 0x35, 0x64, 0x43, 0x72, 0x1e, 0xb5, 0x55
            };

            var password = new byte[64];
            var salt = new byte[64];
            var actual = new byte[32];

            for (int i = 0; i < 64; i++)
            {
                password[i] = (byte)i;
                salt[i] = (byte)(i + 64);
            }

            actual = BCryptPBKDF.BCryptHash(password, salt);

            Assert.IsTrue(output.SequenceEqual(actual));
        }

    }
}
