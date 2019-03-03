using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLib.Ecc
{
    public class Secp256k1Point : Point
    {
        public Secp256k1Point(Secp256k1FieldElement x, Secp256k1FieldElement y) : base(Secp256k1Constants.A, Secp256k1Constants.B, x, y)
        {
        }

        public Secp256k1Point(BigInteger x, BigInteger y) : this(new Secp256k1FieldElement(x), new Secp256k1FieldElement(y))
        {
        }

        private static readonly Secp256k1Point _infinity = new Secp256k1Point(null, null);
        public static Secp256k1Point Infinity
        {
            get
            {
                return _infinity;
            }
        }

        public new Secp256k1Point Multiply(BigInteger coefficient)
        {
            var current = (Point)this;
            var result = (Point)Infinity;

            for (var i = 0; i < 256; i++)
            {
                if ((coefficient & 1) > 0)
                    result = result.Add(current);
                current = current.Add(current);
                coefficient = coefficient >> 1;
            }

            if (result.X == null)
                return Infinity;
            else
                return new Secp256k1Point(result.X.Num, result.Y.Num);
        }

        public static Secp256k1FieldElement GetYCoordinate(Secp256k1FieldElement x, bool yIsEven)
        {
            var a = Secp256k1Constants.A;
            var b = Secp256k1Constants.B;
            
            // y^2 == x^3 + a*x + b
            var left = x.Pow(3) + a * x + b;

            var leftField = new Secp256k1FieldElement(left.Num);
            var candidateY = leftField.Sqrt();

            Secp256k1FieldElement even;
            Secp256k1FieldElement odd;
            if (candidateY.Num % 2 == 0)
            {
                even = candidateY;
                odd = new Secp256k1FieldElement(Secp256k1Constants.P - candidateY.Num);
            }
            else
            {
                even = new Secp256k1FieldElement(Secp256k1Constants.P - candidateY.Num);
                odd = candidateY;
            }

            if (yIsEven)
                return even;
            else
                return odd;
        }
    }
}
