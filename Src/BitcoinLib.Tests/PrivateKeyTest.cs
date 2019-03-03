using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using System.Text;

namespace BitcoinLib.Tests
{
    [TestClass]
    public class PrivateKeyTest
    {
        // TODO - Consider making compressed and testnet parameters in the private key constructor (or determining from WIF string for Create)

        [TestMethod]
        public void GenerateWifStringTest()
        {
            // Compressed
            var prvKeyVal = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("1E99423A4ED27608A15A2616A2B0E9E52CED330AC530EDCC32C8FFC6A526AEDD");
            var expected = "KxFC1jmwwCoACiCAWZ3eXa96mBM6tb3TYzGmf6YwgdGWZgawvrtJ";
            var prvKey = new PrivateKey(prvKeyVal, false, true);
            var wifString = prvKey.GetWifString();
            Assert.AreEqual(expected, wifString);

            prvKeyVal = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("1E99423A4ED27608A15A2616A2B0E9E52CED330AC530EDCC32C8FFC6A526AEDD");
            expected = "L5oLkpV3aqBJ4BgssVAsax1iRa77G5CVYnv9adQ6Z87te7TyUdSC";
            prvKey = new PrivateKey(BigInteger.Pow(2, 256) - BigInteger.Pow(2, 199), false, true);
            wifString = prvKey.GetWifString();
            Assert.AreEqual(expected, wifString);

            prvKeyVal = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("1cca23de92fd1862fb5b76e5f4f50eb082165e5191e116c18ed1a6b24be6a53f");
            expected = "cNYfWuhDpbNM1JWc3c6JTrtrFVxU4AGhUKgw5f93NP2QaBqmxKkg";
            prvKey = new PrivateKey(prvKeyVal, true, true);
            wifString = prvKey.GetWifString();
            Assert.AreEqual(expected, wifString);

            // Uncompressed
            prvKeyVal = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("1E99423A4ED27608A15A2616A2B0E9E52CED330AC530EDCC32C8FFC6A526AEDD");
            expected = "5J3mBbAH58CpQ3Y5RNJpUKPE62SQ5tfcvU2JpbnkeyhfsYB1Jcn";
            prvKey = new PrivateKey(prvKeyVal, false, false);
            wifString = prvKey.GetWifString();
            Assert.AreEqual(expected, wifString);

            prvKeyVal = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("0C28FCA386C7A227600B2FE50B7CAE11EC86D3BF1FBE471BE89827E19D72AA1D");
            expected = "5HueCGU8rMjxEXxiPuD5BDku4MkFqeZyd4dZ1jvhTVqvbTLvyTJ";
            prvKey = new PrivateKey(prvKeyVal, false, false);
            wifString = prvKey.GetWifString();
            Assert.AreEqual(expected, wifString);

            prvKeyVal = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("0dba685b4511dbd3d368e5c4358a1277de9486447af7b3604a69b8d9d8b7889d");
            expected = "5HvLFPDVgFZRK9cd4C5jcWki5Skz6fmKqi1GQJf5ZoMofid2Dty";
            prvKey = new PrivateKey(prvKeyVal, false, false);
            wifString = prvKey.GetWifString();
            Assert.AreEqual(expected, wifString);

            expected = "93XfLeifX7Jx7n7ELGMAf1SUR6f9kgQs8Xke8WStMwUtrDucMzn";
            prvKey = new PrivateKey(BigInteger.Pow(2, 256) - BigInteger.Pow(2, 201), true, false);
            wifString = prvKey.GetWifString();
            Assert.AreEqual(expected, wifString);
        }

        [TestMethod]
        public void ParseWifStringTest()
        {
            // Compressed: "1E99423A4ED27608A15A2616A2B0E9E52CED330AC530EDCC32C8FFC6A526AEDD";
            var expectedBytes = HexUtilities.HexStringToBytes("801E99423A4ED27608A15A2616A2B0E9E52CED330AC530EDCC32C8FFC6A526AEDD01");
            var input = "KxFC1jmwwCoACiCAWZ3eXa96mBM6tb3TYzGmf6YwgdGWZgawvrtJ";
            var prvKey = PrivateKey.CreateFromWifString(input);
            var wifBytes = prvKey.GetWifBytes();
            Assert.IsTrue(ByteArrayUtilities.CompareByteArrays(expectedBytes, wifBytes));
            var wifString = prvKey.GetWifString();
            Assert.AreEqual(input, wifString);
            Assert.IsTrue(prvKey.WifCompressed);
            Assert.IsFalse(prvKey.TestNet);

            // Compressed (TestNet): "1cca23de92fd1862fb5b76e5f4f50eb082165e5191e116c18ed1a6b24be6a53f";
            expectedBytes = HexUtilities.HexStringToBytes("ef1cca23de92fd1862fb5b76e5f4f50eb082165e5191e116c18ed1a6b24be6a53f01");
            input = "cNYfWuhDpbNM1JWc3c6JTrtrFVxU4AGhUKgw5f93NP2QaBqmxKkg";
            prvKey = PrivateKey.CreateFromWifString(input);
            wifBytes = prvKey.GetWifBytes();
            Assert.IsTrue(ByteArrayUtilities.CompareByteArrays(expectedBytes, wifBytes));
            wifString = prvKey.GetWifString();
            Assert.AreEqual(input, wifString);
            Assert.IsTrue(prvKey.WifCompressed);
            Assert.IsTrue(prvKey.TestNet);

            // Uncompressed: "1E99423A4ED27608A15A2616A2B0E9E52CED330AC530EDCC32C8FFC6A526AEDD";
            expectedBytes = HexUtilities.HexStringToBytes("801E99423A4ED27608A15A2616A2B0E9E52CED330AC530EDCC32C8FFC6A526AEDD");
            input = "5J3mBbAH58CpQ3Y5RNJpUKPE62SQ5tfcvU2JpbnkeyhfsYB1Jcn";
            prvKey = PrivateKey.CreateFromWifString(input);
            wifBytes = prvKey.GetWifBytes();
            Assert.IsTrue(ByteArrayUtilities.CompareByteArrays(expectedBytes, wifBytes));
            wifString = prvKey.GetWifString();
            Assert.AreEqual(input, wifString);
            Assert.IsFalse(prvKey.WifCompressed);
            Assert.IsFalse(prvKey.TestNet);

            // Uncompressed: "0C28FCA386C7A227600B2FE50B7CAE11EC86D3BF1FBE471BE89827E19D72AA1D";
            input = "5HueCGU8rMjxEXxiPuD5BDku4MkFqeZyd4dZ1jvhTVqvbTLvyTJ";
            expectedBytes = HexUtilities.HexStringToBytes("800C28FCA386C7A227600B2FE50B7CAE11EC86D3BF1FBE471BE89827E19D72AA1D");
            prvKey = PrivateKey.CreateFromWifString(input);
            wifBytes = prvKey.GetWifBytes();
            Assert.IsTrue(ByteArrayUtilities.CompareByteArrays(expectedBytes, wifBytes));
            wifString = prvKey.GetWifString();
            Assert.AreEqual(input, wifString);
            Assert.IsFalse(prvKey.WifCompressed);
            Assert.IsFalse(prvKey.TestNet);

            // Uncompressed (TestNet): "93XfLeifX7Jx7n7ELGMAf1SUR6f9kgQs8Xke8WStMwUtrDucMzn";
            input = "93XfLeifX7Jx7n7ELGMAf1SUR6f9kgQs8Xke8WStMwUtrDucMzn";
            prvKey = PrivateKey.CreateFromWifString(input);
            Assert.AreEqual((BigInteger.Pow(2, 256) - BigInteger.Pow(2, 201)), prvKey.Value);
            wifString = prvKey.GetWifString();
            Assert.AreEqual(input, wifString);
            Assert.IsFalse(prvKey.WifCompressed);
            Assert.IsTrue(prvKey.TestNet);
        }

        [TestMethod]
        public void SignTest()
        {
            var prvKey = new PrivateKey(1234567890);
            var z = 42424242;
            var sig = prvKey.Sign(z);
            var pubKey = prvKey.GetPublicKey();
            bool valid = pubKey.Verify(z, sig);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void SignTestString()
        {
            var message = Encoding.ASCII.GetBytes("This is a test message");
            // Generate signature using private key
            var prvKey = new PrivateKey(1234567890); // WIF: KwDiBf89QgGbjEhKnhXJuH7LrciVrZi3qYjgd9M8naNQh65b43SA
            var wifString = prvKey.GetWifString();
            var sig = prvKey.Sign(message);
            var pubKeyBytes = prvKey.GetPublicKey().GetPublicKeyBytes();

            // Verify using only publicly shared information (public key, message and signature)
            var pubKey = PublicKey.Parse(pubKeyBytes);
            bool valid = pubKey.Verify(message, sig);
            Assert.IsTrue(valid);
        }
    }
}
