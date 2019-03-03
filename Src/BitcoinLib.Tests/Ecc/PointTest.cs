using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using BitcoinLib.Ecc;

namespace BitcoinLib.Tests
{
    [TestClass]
    public class PointTest
    {
        private static FieldElement CreateElement(int val)
        {
            var prime = 104729;
            var value = new BigInteger(val).Mod(prime); // Ensures that value is always in Field {0..prime-1} as some of these tests use negative numbers
            return new FieldElement(value, prime);
        }

        private static readonly FieldElement A = CreateElement(5);
        private static readonly FieldElement B = CreateElement(7);

        [TestMethod]
        public void TestOnCurve()
        {
            FieldElement x, y;

            try
            {
                x = CreateElement(-2);
                y = CreateElement(4);
                var point = new Point(A, B, x, y);
                Assert.Fail("Exception not thrown");
            }
            catch
            {
            }

            x = CreateElement(3);
            y = CreateElement(-7);
            var pointOnCurve = new Point(A, B, x, y);
            x = CreateElement(18);
            y = CreateElement(77);
            pointOnCurve = new Point(A, B, x, y);
        }

        [TestMethod]
        public void TestAdd0()
        {
            var a = Point.CreatePointAtInfinity(A, B);
            var b = new Point(A, B, CreateElement(2), CreateElement(5));
            var c = new Point(A, B, CreateElement(2), CreateElement(-5));

            Assert.AreEqual(b, a.Add(b));
            Assert.AreEqual(b, b.Add(a));
            Assert.AreEqual(a, b.Add(c));
        }

        [TestMethod]
        public void TestAdd1()
        {
            var a = new Point(A, B, CreateElement(3), CreateElement(7));
            var b = new Point(A, B, CreateElement(-1), CreateElement(-1));

            Assert.AreEqual(new Point(A, B, CreateElement(2), CreateElement(-5)), a.Add(b));
        }

        [TestMethod]
        public void TestAdd2()
        {
            var a = new Point(A, B, CreateElement(-1), CreateElement(-1));
            Assert.AreEqual(new Point(A, B, CreateElement(18), CreateElement(77)), a.Add(a));
        }
    }
}
