using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinLib.Ecc;

namespace BitcoinLib.Tests
{
    [TestClass]
    public class Secp256k1PointTest
    {
        [TestMethod]
        public void CoordinateTest()
        {
            // Uncompressed Public Key
            var xValStr = "50863AD64A87AE8A2FE83C1AF1A8403CB53F53E486D8511DAD8A04887E5B2352";
            var yValStr = "2CD470243453A299FA9E77237716103ABC11A1DF38855ED6F2EE187E9C582BA6";

            var xVal = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString(xValStr);
            var yVal = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString(yValStr);

            var point = new Secp256k1Point(xVal, yVal);
            var xValStr2 = point.X.Num.PositiveValToHexString(false);
            var yValStr2 = point.Y.Num.PositiveValToHexString(false);

            Assert.AreEqual(xValStr, xValStr2);
            Assert.AreEqual(yValStr, yValStr2);
        }

        [TestMethod]
        public void OrderTest()
        {
            var N = Secp256k1Constants.N;
            var inf = BitcoinConstants.G.Multiply(N);
            Assert.AreEqual(Secp256k1Point.Infinity, inf);
        }
    }
}
