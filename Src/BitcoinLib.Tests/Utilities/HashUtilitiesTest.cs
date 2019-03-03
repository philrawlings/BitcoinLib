using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace BitcoinLib.Tests
{
    [TestClass]
    public class HashUtilitiesTest
    {
        [TestMethod]
        public void Sha256Test()
        {
            var data = Encoding.ASCII.GetBytes("hello");
            var sha256Hash = HashUtilities.Sha256(data);

            var actual = HexUtilities.BytesToHexString(sha256Hash);
            var expected = "2cf24dba5fb0a30e26e83b2ac5b9e29e1b161e5c1fa7425e73043362938b9824";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DoubleSha256Test()
        {
            var data = Encoding.ASCII.GetBytes("hello");
            var dblSha256Hash = HashUtilities.DoubleSha256(data);

            var actual = HexUtilities.BytesToHexString(dblSha256Hash);
            var expected = "9595c9df90075148eb06860365df33584b75bff782a510c6cd4883a419833d50";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RipeMd160Test()
        {
            var data = Encoding.ASCII.GetBytes("hello");
            var ripeMd160Hash = HashUtilities.RipeMd160(data);

            var actual = HexUtilities.BytesToHexString(ripeMd160Hash);
            var expected = "108f07b8382412612c048d07d13f814118445acd";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Hash160Test()
        {
            var data = Encoding.ASCII.GetBytes("hello");
            var hash160 = HashUtilities.Hash160(data);

            var actual = HexUtilities.BytesToHexString(hash160);
            var expected = "b6a9c8c230722b7c748331a8b450f05566dc7d0f";

            Assert.AreEqual(expected, actual);
        }
    }
}
