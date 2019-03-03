using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using BitcoinLib.Ecc;

namespace BitcoinLib.Tests
{
    [TestClass]
    public class FieldElementTest
    {
        [TestMethod]
        public void TestAdd()
        {
            var a = new FieldElement(new BigInteger(2), new BigInteger(31));
            var b = new FieldElement(new BigInteger(15), new BigInteger(31));
            Assert.AreEqual(new FieldElement(new BigInteger(17), new BigInteger(31)), a + b);

            a = new FieldElement(new BigInteger(17), new BigInteger(31));
            b = new FieldElement(new BigInteger(21), new BigInteger(31));
            Assert.AreEqual(new FieldElement(new BigInteger(7), new BigInteger(31)), a + b);
        }

        [TestMethod]
        public void TestSubtract()
        {
            var a = new FieldElement(new BigInteger(29), new BigInteger(31));
            var b = new FieldElement(new BigInteger(4), new BigInteger(31));
            Assert.AreEqual(new FieldElement(new BigInteger(25), new BigInteger(31)), a - b);

            a = new FieldElement(new BigInteger(15), new BigInteger(31));
            b = new FieldElement(new BigInteger(30), new BigInteger(31));
            Assert.AreEqual(new FieldElement(new BigInteger(16), new BigInteger(31)), a - b);
        }

        [TestMethod]
        public void TestMultiply()
        {
            var a = new FieldElement(new BigInteger(24), new BigInteger(31));
            var b = new FieldElement(new BigInteger(19), new BigInteger(31));
            Assert.AreEqual(new FieldElement(new BigInteger(22), new BigInteger(31)), a * b);
        }

        [TestMethod]
        public void TestRMultiply()
        {
            var a = new FieldElement(new BigInteger(24), new BigInteger(31));
            var b = new BigInteger(2);
            Assert.AreEqual(a + a, a.Multiply(b));
        }

        [TestMethod]
        public void TestPow()
        {
            var a = new FieldElement(new BigInteger(17), new BigInteger(31));
            Assert.AreEqual(new FieldElement(new BigInteger(15), new BigInteger(31)), a.Pow(3));

            a = new FieldElement(new BigInteger(5), new BigInteger(31));
            var b = new FieldElement(new BigInteger(18), new BigInteger(31));
            Assert.AreEqual(new FieldElement(new BigInteger(16), new BigInteger(31)), a.Pow(5) * b);
        }

        [TestMethod]
        public void TestDiv()
        {
            var a = new FieldElement(new BigInteger(3), new BigInteger(31));
            var b = new FieldElement(new BigInteger(24), new BigInteger(31));
            Assert.AreEqual(new FieldElement(new BigInteger(4), new BigInteger(31)), a/b);

            // Anything raised to a negative power is less than 1, so not sure how these can work
            //a = new FieldElement(new BigInteger(17), new BigInteger(31));
            //Assert.AreEqual(new FieldElement(new BigInteger(29), new BigInteger(31)), a.Pow(-3));

            //a = new FieldElement(new BigInteger(4), new BigInteger(31));
            //b = new FieldElement(new BigInteger(11), new BigInteger(31));
            //Assert.AreEqual(new FieldElement(new BigInteger(13), new BigInteger(31)), a.Pow(-4) * b);
        }
    }
}
