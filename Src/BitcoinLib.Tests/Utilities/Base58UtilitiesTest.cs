using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BitcoinLib.Tests
{
    [TestClass]
    public class Base58UtilitiesTest
    {
        [TestMethod]
        public void EncodeBase58CheckVersion0Test()
        {
            // Version 0 (0x00) Prefixes (Bitcoin Addresses)

            var data = HexUtilities.HexStringToBytes("00010966776006953D5567439E5E39F86A0D273BEE");
            var expected = "16UwLL9Risc3QfPqBUvKofHmBQ7wMtjvM";
            var actual = Base58Utilities.EncodeBase58Check(data);
            Assert.AreEqual(expected, actual);

            data = HexUtilities.HexStringToBytes("00EF937DF494BC737F158D6593856DFEBD80988BED");
            expected = "1NqmBmniPt6viRizQjbjWzA6zc3M3Fvdqt";
            actual = Base58Utilities.EncodeBase58Check(data);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeBase58CheckVersion5Test()
        {
            // Version 5 (0x05) Prefixes (Pay-to-Script-Hash Addresses)

            var data = HexUtilities.HexStringToBytes("0574E9D4CDE54B8A6DECDD997541E44508FF8BA5E8");
            var expected = "3CMCRgEm8HVz3DrWaCCid3vAANE42jcEv9";
            var actual = Base58Utilities.EncodeBase58Check(data);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeBase58CheckNonStandardTest()
        {
            // Version 0 (0x00) Prefixes, non-standard lengths

            var data = HexUtilities.HexStringToBytes("00031bab84e687e36514eeaf5a017c30d32c1f59dd4ea6629da7970ca374513dd006");
            var expected = "173RKgkk7fMbYUYBGyyAHeZ6rwfKRMn17h7DtGsmpEdab8TV6UB";
            var actual = Base58Utilities.EncodeBase58Check(data);
            Assert.AreEqual(expected, actual);

            data = HexUtilities.HexStringToBytes("005361746f736869204e616b616d6f746f");
            expected = "12ANjYr7zPnxRdZfnmC2e6jjHDpBY";
            actual = Base58Utilities.EncodeBase58Check(data);
            Assert.AreEqual(expected, actual);

            // Version 42 (0x2A) Prefix, non standard data

            data = HexUtilities.HexStringToBytes("2a031bab84e687e36514eeaf5a017c30d32c1f59dd4ea6629da7970ca374513dd006");
            expected = "7DTXS6pY6a98XH2oQTZUbbd1Z7P4NzkJqfraixprPutXQVTkwBGw";
            actual = Base58Utilities.EncodeBase58Check(data);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DecodeBase58CheckVersion0Test()
        {
            // Version 0 (0x00) Prefixes (Bitcoin Addresses)

            var data = "16UwLL9Risc3QfPqBUvKofHmBQ7wMtjvM";
            var expected = HexUtilities.HexStringToBytes("00010966776006953D5567439E5E39F86A0D273BEE");
            var actual = Base58Utilities.DecodeBase58Check(data);
            Assert.IsTrue(ByteArrayUtilities.CompareByteArrays(expected, actual));

            data = "1NqmBmniPt6viRizQjbjWzA6zc3M3Fvdqt";
            expected = HexUtilities.HexStringToBytes("00EF937DF494BC737F158D6593856DFEBD80988BED");
            actual = Base58Utilities.DecodeBase58Check(data);
            Assert.IsTrue(ByteArrayUtilities.CompareByteArrays(expected, actual));
        }

        [TestMethod]
        public void DecodeBase58CheckVersion5Test()
        {
            // Version 5 (0x05) Prefixes (Pay-to-Script-Hash Addresses)

            var data = "3CMCRgEm8HVz3DrWaCCid3vAANE42jcEv9";
            var expected = HexUtilities.HexStringToBytes("0574E9D4CDE54B8A6DECDD997541E44508FF8BA5E8");
            var actual = Base58Utilities.DecodeBase58Check(data);
            Assert.IsTrue(ByteArrayUtilities.CompareByteArrays(expected, actual));
        }

        [TestMethod]
        public void DecodeBase58CheckNonStandardTest()
        {
            // Version 0 (0x00) Prefixes, non-standard lengths
            
            var data = "173RKgkk7fMbYUYBGyyAHeZ6rwfKRMn17h7DtGsmpEdab8TV6UB";
            var expected = HexUtilities.HexStringToBytes("00031bab84e687e36514eeaf5a017c30d32c1f59dd4ea6629da7970ca374513dd006");
            var actual = Base58Utilities.DecodeBase58Check(data);
            Assert.IsTrue(ByteArrayUtilities.CompareByteArrays(expected, actual));

            data = "12ANjYr7zPnxRdZfnmC2e6jjHDpBY";
            expected = HexUtilities.HexStringToBytes("005361746f736869204e616b616d6f746f");
            actual = Base58Utilities.DecodeBase58Check(data);
            Assert.IsTrue(ByteArrayUtilities.CompareByteArrays(expected, actual));

            // Version 42 (0x2A) Prefix, non standard data

            data = "7DTXS6pY6a98XH2oQTZUbbd1Z7P4NzkJqfraixprPutXQVTkwBGw";
            expected = HexUtilities.HexStringToBytes("2a031bab84e687e36514eeaf5a017c30d32c1f59dd4ea6629da7970ca374513dd006");
            actual = Base58Utilities.DecodeBase58Check(data);
            Assert.IsTrue(ByteArrayUtilities.CompareByteArrays(expected, actual));
        }
    }
}
