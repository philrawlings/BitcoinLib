using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using System.Globalization;

namespace BitcoinLib.Tests
{
    [TestClass]
    public class HexUtilitiesTest
    {
        [TestMethod]
        public void BytesToHexStringTest()
        {
            var bytes = new byte[] { 0x01, 0x80, 0xFF };
            var hexLower = HexUtilities.BytesToHexString(bytes, true);
            Assert.AreEqual("0180ff", hexLower);
            var hexUpper = HexUtilities.BytesToHexString(bytes, false);
            Assert.AreEqual("0180FF", hexUpper);

            bytes = new byte[0];
            var hexEmpty = HexUtilities.BytesToHexString(bytes);
            Assert.AreEqual(string.Empty, hexEmpty);

            bytes = new byte[] { 0x00, 0x00 };
            var hexZero = HexUtilities.BytesToHexString(bytes);
            Assert.AreEqual("0000", hexZero);
        }

        [TestMethod]
        public void HexStringToBytesTest()
        {
            string hexLower = "0180ff";
            var expected = new byte[] { 0x01, 0x80, 0xFF };
            var bytes = HexUtilities.HexStringToBytes(hexLower);
            Assert.IsTrue(ByteArrayUtilities.CompareByteArrays(expected, bytes));

            string hexUpper = "0180FF";
            bytes = HexUtilities.HexStringToBytes(hexUpper);
            Assert.IsTrue(ByteArrayUtilities.CompareByteArrays(expected, bytes));

            string hexEmpty = string.Empty;
            bytes = HexUtilities.HexStringToBytes(hexEmpty);
            Assert.IsTrue(bytes.Length == 0);

            string hexZero = "0000";
            expected = new byte[] { 0x00, 0x00 };
            bytes = HexUtilities.HexStringToBytes(hexZero);
            Assert.IsTrue(ByteArrayUtilities.CompareByteArrays(expected, bytes));
        }

        [TestMethod]
        public void BigIntegerToHexTest()
        {
            var val = new BigInteger(0x1234567890ABCDEF);
            var expected = "1234567890abcdef";
            var hexLower = val.PositiveValToHexString();
            Assert.AreEqual(expected, hexLower);
            hexLower = val.PositiveValToHexString(true);
            Assert.AreEqual(expected, hexLower);
            expected = "1234567890ABCDEF";
            var hexUpper = val.PositiveValToHexString(false);
            Assert.AreEqual(expected, hexUpper);

            val = new BigInteger(0x00005678FF);
            expected = "5678ff";
            var hexStr = val.PositiveValToHexString();
            Assert.AreEqual(expected, hexStr);

            val = new BigInteger(0x00005678FF);
            expected = "00005678ff";
            hexStr = val.PositiveValToHexString(10);
            Assert.AreEqual(expected, hexStr);

            val = new BigInteger(0x5678FF);
            expected = "0005678ff";
            hexStr = val.PositiveValToHexString(9);
            Assert.AreEqual(expected, hexStr);

            expected = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141";
            val = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString(expected);
            hexStr = val.PositiveValToHexString(64, false);
            Assert.AreEqual(expected, hexStr);

            var input = "0000000000000000000000000000000EBAAEDCE6AF48A03BBFD25E8CD0364141";
            expected = "EBAAEDCE6AF48A03BBFD25E8CD0364141";
            val = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString(input);
            hexStr = val.PositiveValToHexString(false);
            Assert.AreEqual(expected, hexStr);

            input = "EBAAEDCE6AF48A03BBFD25E8CD0364141";
            expected = "0000000000000000000000000000000EBAAEDCE6AF48A03BBFD25E8CD0364141";
            val = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString(input);
            hexStr = val.PositiveValToHexString(64, false);
            Assert.AreEqual(expected, hexStr);
        }

        [TestMethod]
        public void BigIntegerToBytesTest()
        {
            // Uncompressed Public Key
            var xValStr = "50863AD64A87AE8A2FE83C1AF1A8403CB53F53E486D8511DAD8A04887E5B2352";
            var yValStr = "2CD470243453A299FA9E77237716103ABC11A1DF38855ED6F2EE187E9C582BA6";
            var expected = "0450863AD64A87AE8A2FE83C1AF1A8403CB53F53E486D8511DAD8A04887E5B23522CD470243453A299FA9E77237716103ABC11A1DF38855ED6F2EE187E9C582BA6";

            var xVal = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString(xValStr);
            var yVal = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString(yValStr);
            var bytes = new byte[65]; // type byte + 32 bytes for X + 32 bytes for Y
            bytes[0] = 0x04;
            xVal.PositiveValToBigEndianBytes(bytes, 1, 32);
            yVal.PositiveValToBigEndianBytes(bytes, 33, 32);
            var hexStr = HexUtilities.BytesToHexString(bytes, false);
            Assert.AreEqual(expected, hexStr);

            // Arbitrary data to arbitrary position in buffer
            var str1 = "010203040506";
            var str2 = "ABCDEF";
            expected = "010203040506000000ABCDEF";

            var val1 = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString(str1);
            var val2 = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString(str2);
            bytes = new byte[12];
            val1.PositiveValToBigEndianBytes(bytes, 0, 6);
            val2.PositiveValToBigEndianBytes(bytes, 9, 3);
            hexStr = HexUtilities.BytesToHexString(bytes, false);
            Assert.AreEqual(expected, hexStr);

            // Arbitrary data to arbitrary position in buffer, right aligned
            str1 = "010203040506";
            str2 = "ABCDEF";
            expected = "010203040506000000ABCDEF";

            val1 = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString(str1);
            val2 = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString(str2);
            bytes = new byte[12];
            val1.PositiveValToBigEndianBytes(bytes, 0, 6);
            val2.PositiveValToBigEndianBytes(bytes, 6, 6);
            hexStr = HexUtilities.BytesToHexString(bytes, false);
            Assert.AreEqual(expected, hexStr);
        }
    }
}
