using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using System.Collections.Generic;
using BitcoinLib.Ecc;

namespace BitcoinLib.Tests
{
    [TestClass]
    public class EccTest
    {
        [TestMethod]
        public void TestOnCurve()
        {
            // tests the following points whether they are on the curve or not
            // on curve y^2=x^3-7 over F_223:
            // (192,105) (17,56) (200,119) (1,193) (42,99)
            // the ones that aren't should throw an exception

            var prime = 223;
            var a = new FieldElement(0, prime);
            var b = new FieldElement(7, prime);

            var validPoints = new List<Tuple<BigInteger, BigInteger>>();
            validPoints.Add(new Tuple<BigInteger, BigInteger>(192, 105));
            validPoints.Add(new Tuple<BigInteger, BigInteger>(17, 56));
            validPoints.Add(new Tuple<BigInteger, BigInteger>(1, 193));

            foreach (var rawPoint in validPoints)
            {
                var x = new FieldElement(rawPoint.Item1, prime);
                var y = new FieldElement(rawPoint.Item2, prime);

                var point = new Point(a, b, x, y);
            }

            var invalidPoints = new List<Tuple<BigInteger, BigInteger>>();
            invalidPoints.Add(new Tuple<BigInteger, BigInteger>(200, 119));
            invalidPoints.Add(new Tuple<BigInteger, BigInteger>(42, 99));

            foreach (var rawPoint in invalidPoints)
            {
                var x = new FieldElement(rawPoint.Item1, prime);
                var y = new FieldElement(rawPoint.Item2, prime);

                try
                {
                    var point = new Point(a, b, x, y);
                    Assert.Fail("Exception not thrown");
                }
                catch
                {
                }
            }
        }

        [TestMethod]
        public void TestAdd()
        {
            var prime = 223;
            var a = new FieldElement(0, prime);
            var b = new FieldElement(7, prime);

            // AddTestData = { x1, y1, x2, y2, x3, y3 }
            var coordinates = new List<AddTestData>();
            coordinates.Add(new AddTestData(192, 105, 17, 56, 170, 142));
            coordinates.Add(new AddTestData(47, 71, 117, 141, 60, 139));
            coordinates.Add(new AddTestData(143, 98, 76, 66, 47, 71));

            foreach (var coordinate in coordinates)
            {
                var x1 = new FieldElement(coordinate.x1Raw, prime);
                var y1 = new FieldElement(coordinate.y1Raw, prime);
                var p1 = new Point(a, b, x1, y1);
                var x2 = new FieldElement(coordinate.x2Raw, prime);
                var y2 = new FieldElement(coordinate.y2Raw, prime);
                var p2 = new Point(a, b, x2, y2);
                var x3 = new FieldElement(coordinate.x3Raw, prime);
                var y3 = new FieldElement(coordinate.y3Raw, prime);
                var p3 = new Point(a, b, x3, y3);
                Assert.AreEqual(p3, p1.Add(p2));
            }
        }

        [TestMethod]
        public void TestScalarMultiply()
        {
            var prime = 223;
            var a = new FieldElement(0, prime);
            var b = new FieldElement(7, prime);

            // MulTestData = { x1, y1, x2, y2, coefficient }
            var coordinates = new List<MulTestData>();
            coordinates.Add(new MulTestData(192, 105, 49, 71, 2));
            coordinates.Add(new MulTestData(143, 98, 64, 168, 2));
            coordinates.Add(new MulTestData(47, 71, 36, 111, 2));
            coordinates.Add(new MulTestData(47, 71, 194, 51, 4));
            coordinates.Add(new MulTestData(47, 71, 116, 55, 8));

            foreach (var coordinate in coordinates)
            {
                var x1 = new FieldElement(coordinate.x1Raw, prime);
                var y1 = new FieldElement(coordinate.y1Raw, prime);
                var p1 = new Point(a, b, x1, y1);
                var x2 = new FieldElement(coordinate.x2Raw, prime);
                var y2 = new FieldElement(coordinate.y2Raw, prime);
                var p2 = new Point(a, b, x2, y2);
                Assert.AreEqual(p2, p1.Multiply(coordinate.coefficient));
            }

            {
                // Infinity test
                var x1 = new FieldElement(47, prime);
                var y1 = new FieldElement(71, prime);
                var p1 = new Point(a, b, x1, y1);
                var p2 = Point.CreatePointAtInfinity(a, b);
                Assert.AreEqual(p2, p1.Multiply(21));
            }
        }

        private class AddTestData
        {
            public BigInteger x1Raw { get; private set; }
            public BigInteger y1Raw { get; private set; }
            public BigInteger x2Raw { get; private set; }
            public BigInteger y2Raw { get; private set; }
            public BigInteger x3Raw { get; private set; }
            public BigInteger y3Raw { get; private set; }

            public AddTestData(BigInteger x1Raw, BigInteger y1Raw, BigInteger x2Raw, BigInteger y2Raw, BigInteger x3Raw, BigInteger y3Raw)
            {
                this.x1Raw = x1Raw;
                this.y1Raw = y1Raw;
                this.x2Raw = x2Raw;
                this.y2Raw = y2Raw;
                this.x3Raw = x3Raw;
                this.y3Raw = y3Raw;
            }
        }

        private class MulTestData
        {
            public BigInteger x1Raw { get; private set; }
            public BigInteger y1Raw { get; private set; }
            public BigInteger x2Raw { get; private set; }
            public BigInteger y2Raw { get; private set; }
            public BigInteger coefficient { get; private set; }

            public MulTestData(BigInteger x1Raw, BigInteger y1Raw, BigInteger x2Raw, BigInteger y2Raw, BigInteger coefficient)
            {
                this.x1Raw = x1Raw;
                this.y1Raw = y1Raw;
                this.x2Raw = x2Raw;
                this.y2Raw = y2Raw;
                this.coefficient = coefficient;
            }
        }

    }
}
