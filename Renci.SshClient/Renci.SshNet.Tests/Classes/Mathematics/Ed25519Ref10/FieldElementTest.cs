using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Renci.SshNet.Mathematics;

namespace Renci.SshNet.Tests.Classes.Mathematics.Ed25519Ref10
{
    [TestClass]
    public class FieldElementTest
    {
        [TestMethod]
        public void FieldElementAdd()
        {
            var f1 = new FieldElement(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            var f2 = new FieldElement(11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            var output = new FieldElement(12, 14, 16, 18, 20, 22, 24, 26, 28, 30);

            var actual = f1 + f2;
            Assert.AreEqual(output, actual);
        }

        [TestMethod]
        public void FieldElementReplace()
        {
            var f1 = new FieldElement(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            var f2 = new FieldElement(11, 12, 13, 14, 15, 16, 17, 18, 19, 20);

            var actual = f1.Replace(f2, 0);
            Assert.AreEqual(f1, actual);
            actual = f1.Replace(f2, 1);
            Assert.AreEqual(f2, actual);
        }

        [TestMethod]
        public void FieldElementSwap()
        {
            var f1 = new FieldElement(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            var f2 = new FieldElement(11, 12, 13, 14, 15, 16, 17, 18, 19, 20);

            var out1 = new FieldElement(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            var out2 = new FieldElement(11, 12, 13, 14, 15, 16, 17, 18, 19, 20);

            var actual = f1.Swap(ref out2, 0);
            Assert.AreEqual(f1, actual);
            Assert.AreEqual(f2, out2);

            actual = f1.Swap(ref out2, 1);
            Assert.AreEqual(f2, actual);
            Assert.AreEqual(f1, out2);
        }

        [TestMethod]
        public void FieldElementFromToArray()
        {
            var f1 = new FieldElement(2 * 2, - 4 * 4, 8 * 8, - 16 * 16, 32 * 32, - 64 * 64, 128 * 128, - 256 * 256, 512 * 512, - 1024 * 1024);

            byte[] data = new byte[32];
            f1.CopyTo(data, 0);
            var actual = FieldElement.FromData(data, 0);

            Assert.AreEqual(f1, actual);
        }
    }
}
