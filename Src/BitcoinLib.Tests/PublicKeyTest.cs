using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using System.Collections.Generic;

namespace BitcoinLib.Tests
{
    [TestClass]
    public class PublicKeyTest
    {
        [TestMethod]
        public void AddressTest()
        {
            // Uncompressed Public Key
            var ecdsaPrivateKey = "18E14A7B6A307F426A94F8114701E7C8E774E7F9A47E2C2035DB29A206321725";
            var expectedPubXVal = "50863AD64A87AE8A2FE83C1AF1A8403CB53F53E486D8511DAD8A04887E5B2352";
            var expectedPubYVal = "2CD470243453A299FA9E77237716103ABC11A1DF38855ED6F2EE187E9C582BA6";
            var expectedPubKey = "0450863AD64A87AE8A2FE83C1AF1A8403CB53F53E486D8511DAD8A04887E5B23522CD470243453A299FA9E77237716103ABC11A1DF38855ED6F2EE187E9C582BA6";
            var expectedAddress = "16UwLL9Risc3QfPqBUvKofHmBQ7wMtjvM";

            var prvKeyVal = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString(ecdsaPrivateKey);
            var prvKey = new PrivateKey(prvKeyVal, false, false);
            var pubKey = prvKey.GetPublicKey();
            Assert.AreEqual(expectedPubXVal, pubKey.X.PositiveValToHexString(false));
            Assert.AreEqual(expectedPubYVal, pubKey.Y.PositiveValToHexString(false));

            var pubKeyBytes = pubKey.GetPublicKeyBytes();
            var pubKeyString = HexUtilities.BytesToHexString(pubKeyBytes, false);
            Assert.AreEqual(expectedPubKey, pubKeyString);
            var address = pubKey.GetAddress();
            Assert.AreEqual(expectedAddress, address);
        }

        [TestMethod]
        public void TestPublicPoints()
        {
            var points = new List<PubPointTestData>();
            points.Add(new PubPointTestData(7, "5cbdf0646e5db4eaa398f365f2ea7a0e3d419b7e0330e39ce92bddedcac4f9bc", "6aebca40ba255960a3178d6d861a54dba813d0b813fde7b5a5082628087264da"));
            points.Add(new PubPointTestData(1485, "c982196a7466fbbbb0e27a940b6af926c1a74d5ad07128c82824a11b5398afda", "7a91f9eae64438afb9ce6448a1c133db2d8fb9254e4546b6f001637d50901f55"));
            points.Add(new PubPointTestData(BigInteger.Pow(2, 128), "8f68b9d2f63b5f339239c1ad981f162ee88c5678723ea3351b7b444c9ec4c0da", "662a9f2dba063986de1d90c2b6be215dbbea2cfe95510bfdf23cbf79501fff82"));
            points.Add(new PubPointTestData(BigInteger.Pow(2, 240) + BigInteger.Pow(2, 31), "9577ff57c8234558f293df502ca4f09cbc65a6572c842b39b366f21717945116", "10b49c67fa9365ad7b90dab070be339a1daf9052373ec30ffae4f72d5e66d053"));

            foreach (var point in points)
            {
                var expectedX = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString(point.X);
                var expectedY = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString(point.Y);

                // Using Private/Public Key classes
                var prvKey = new PrivateKey(point.Secret);
                var pubKey = prvKey.GetPublicKey();
                Assert.AreEqual(expectedX, pubKey.X);
                Assert.AreEqual(expectedY, pubKey.Y);

                // Manual check
                var pubPoint = BitcoinConstants.G.Multiply(point.Secret);
                Assert.AreEqual(expectedX, pubPoint.X.Num);
                Assert.AreEqual(expectedY, pubPoint.Y.Num);
            }
        }

        private class PubPointTestData
        {
            public BigInteger Secret { get; private set; }
            public string X { get; private set; }
            public string Y { get; private set; }

            public PubPointTestData(BigInteger secret, string x, string y)
            {
                Secret = secret;
                X = x;
                Y = y;
            }
        }

        [TestMethod]
        public void PubKeyBytesTest()
        {
            var uncompressed = "049d5ca49670cbe4c3bfa84c96a8c87df086c6ea6a24ba6b809c9de234496808d56fa15cc7f3d38cda98dee2419f415b7513dde1301f8643cd9245aea7f3f911f9";
            var compressed = "039d5ca49670cbe4c3bfa84c96a8c87df086c6ea6a24ba6b809c9de234496808d5";
            var prvKey = new PrivateKey(BigInteger.Pow(999, 3), false, false);
            var pubKey = prvKey.GetPublicKey();
            var bytes = pubKey.GetPublicKeyBytes();
            Assert.AreEqual(uncompressed, HexUtilities.BytesToHexString(bytes));
            prvKey = new PrivateKey(BigInteger.Pow(999, 3), false, true);
            pubKey = prvKey.GetPublicKey();
            bytes = pubKey.GetPublicKeyBytes();
            Assert.AreEqual(compressed, HexUtilities.BytesToHexString(bytes));

            uncompressed = "04a598a8030da6d86c6bc7f2f5144ea549d28211ea58faa70ebf4c1e665c1fe9b5204b5d6f84822c307e4b4a7140737aec23fc63b65b35f86a10026dbd2d864e6b";
            compressed = "03a598a8030da6d86c6bc7f2f5144ea549d28211ea58faa70ebf4c1e665c1fe9b5";
            prvKey = new PrivateKey(123, false, false);
            pubKey = prvKey.GetPublicKey();
            bytes = pubKey.GetPublicKeyBytes();
            Assert.AreEqual(uncompressed, HexUtilities.BytesToHexString(bytes));
            prvKey = new PrivateKey(123, false, true);
            pubKey = prvKey.GetPublicKey();
            bytes = pubKey.GetPublicKeyBytes();
            Assert.AreEqual(compressed, HexUtilities.BytesToHexString(bytes));

            uncompressed = "04aee2e7d843f7430097859e2bc603abcc3274ff8169c1a469fee0f20614066f8e21ec53f40efac47ac1c5211b2123527e0e9b57ede790c4da1e72c91fb7da54a3";
            compressed = "03aee2e7d843f7430097859e2bc603abcc3274ff8169c1a469fee0f20614066f8e";
            prvKey = new PrivateKey(42424242, false, false);
            pubKey = prvKey.GetPublicKey();
            bytes = pubKey.GetPublicKeyBytes();
            Assert.AreEqual(uncompressed, HexUtilities.BytesToHexString(bytes));
            prvKey = new PrivateKey(42424242, false, true);
            pubKey = prvKey.GetPublicKey();
            bytes = pubKey.GetPublicKeyBytes();
            Assert.AreEqual(compressed, HexUtilities.BytesToHexString(bytes));
        }

        [TestMethod]
        public void MainAndTestnetAddressTest()
        {
            // Compressed
            var prvKey = new PrivateKey(BigInteger.Pow(888, 3), false, true);
            var pubKey = prvKey.GetPublicKey();
            var mainnetAddress = pubKey.GetAddress();
            Assert.AreEqual("148dY81A9BmdpMhvYEVznrM45kWN32vSCN", mainnetAddress);
            prvKey = new PrivateKey(BigInteger.Pow(888, 3), true, true);
            pubKey = prvKey.GetPublicKey();
            var testnetAddress = pubKey.GetAddress();
            Assert.AreEqual("mieaqB68xDCtbUBYFoUNcmZNwk74xcBfTP", testnetAddress);

            // Compressed
            var wifString = "KwDiBf89QgGbjEhKnhXJuH7LrciVrZi3qYjgd9M8naNQh65b43SA";
            prvKey = PrivateKey.CreateFromWifString(wifString);
            Assert.AreEqual(1234567890, prvKey.Value);
            mainnetAddress = prvKey.GetPublicKey().GetAddress();
            Assert.AreEqual("17V2YVsReZMkvkQDP8Gs1yyGXi6y7jpnV7", mainnetAddress);

            // Compressed
            wifString = "Kyd8ohVKC84wDwfmBEVkDnopsYRF9GmjRWx6VeUQqF4BpbWVUukv";
            prvKey = PrivateKey.CreateFromWifString(wifString);
            mainnetAddress = prvKey.GetPublicKey().GetAddress();
            Assert.AreEqual("12Kk2yQEYamxySZk2wGzc8ueNuepMkbEQJ", mainnetAddress);

            // Uncompressed
            prvKey = new PrivateKey(321, false, false);
            pubKey = prvKey.GetPublicKey();
            mainnetAddress = pubKey.GetAddress();
            Assert.AreEqual("1S6g2xBJSED7Qr9CYZib5f4PYVhHZiVfj", mainnetAddress);
            prvKey = new PrivateKey(321, true, false);
            pubKey = prvKey.GetPublicKey();
            testnetAddress = pubKey.GetAddress();
            Assert.AreEqual("mfx3y63A7TfTtXKkv7Y6QzsPFY6QCBCXiP", testnetAddress);

            // Uncompressed
            prvKey = new PrivateKey(4242424242, false, false);
            pubKey = prvKey.GetPublicKey();
            mainnetAddress = pubKey.GetAddress();
            Assert.AreEqual("1226JSptcStqn4Yq9aAmNXdwdc2ixuH9nb", mainnetAddress);
            prvKey = new PrivateKey(4242424242, true, false);
            pubKey = prvKey.GetPublicKey();
            testnetAddress = pubKey.GetAddress();
            Assert.AreEqual("mgY3bVusRUL6ZB2Ss999CSrGVbdRwVpM8s", testnetAddress);

            // Uncompressed
            wifString = "5Kb8kLf9zgWQnogidDA76MzPL6TsZZY36hWXMssSzNydYXYB9KF";
            prvKey = PrivateKey.CreateFromWifString(wifString);
            mainnetAddress = prvKey.GetPublicKey().GetAddress();
            Assert.AreEqual("1CC3X2gu58d6wXUWMffpuzN9JAfTUWu4Kj", mainnetAddress);
        }

        [TestMethod]
        public void TestVerify()
        {
            var x = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("887387e452b8eacc4acfde10d9aaf7f6d9a0f975aabb10d006e4da568744d06c");
            var y = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("61de6d95231cd89026e286df3b6ae4a894a3378e393e93a0f45b666329a0ae34");
            var p = new PublicKey(x, y);
            var z = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("ec208baa0fc1c19f708a9ca96fdeff3ac3f230bb4a7ba4aede4942ad003c0f60");
            var r = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("ac8d1c87e51d0d441be8b3dd5b05c8795b48875dffe00b7ffcfac23010d3a395");
            var s = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("68342ceff8935ededd102dd876ffd6ba72d6a427a3edb13d26eb0781cb423c4");
            var signature = new Signature(r, s);
            var valid = p.Verify(z, signature);
            Assert.IsTrue(valid);

            z = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("7c076ff316692a3d7eb3c3bb0f8b1488cf72e1afcd929e29307032997a838a3d");
            r = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("eff69ef2b1bd93a66ed5219add4fb51e11a840f404876325a1e8ffe0529a2c");
            s = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("c7207fee197d27c618aea621406f6bf5ef6fca38681d82b2f06fddbdce6feab6");
            signature = new Signature(r, s);
            valid = p.Verify(z, signature);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void TestParse()
        {
            var expected = "0349fc4e631e3624a545de3f89f5d8684c7b8138bd94bdd531d2e213bf016b278a";
            var pubKey = PublicKey.Parse(expected);
            var actual = pubKey.GetPublicKeyHexString();
            Assert.AreEqual(expected, actual);
        }
    }
}
