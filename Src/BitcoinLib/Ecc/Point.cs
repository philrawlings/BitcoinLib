using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLib.Ecc
{
    public class Point : IEquatable<Point>
    {
        public FieldElement A { get; private set; }
        public FieldElement B { get; private set; }
        public FieldElement X { get; private set; }
        public FieldElement Y { get; private set; }

        public Point(FieldElement a, FieldElement b, FieldElement x, FieldElement y)
        {
            A = a;
            B = b;
            X = x;
            Y = y;

            // X and Y being null represents the point at infinity
            if (x == null && y == null)
            {
                return;
            }

            // make sure that the elliptic curve equation is satisfied
            // y^2 == x^3 + a*x + b
            if (y.Pow(2) != x.Pow(3) + a * x + b)
            {
                throw new Exception(string.Format("{0}, {1} is not on the curve", x, y));
            }
        }

        public static Point CreatePointAtInfinity(FieldElement a, FieldElement b)
        {
            return new Point(a, b, null, null);
        }

        public override string ToString()
        {
            if (X == null)
                return "Point(infinity)";
            else
                return string.Format("Point({0},{1})", X, Y);
        }

        public static bool operator ==(Point element1, Point element2)
        {
            if (ReferenceEquals(element1, element2))
                return true;
            else if ((object)element1 == null || (object)element2 == null)
                return false;
            else
                return element1.Equals(element2);
        }

        public static bool operator !=(Point element1, Point element2)
        {
            return !(element1 == element2);
        }

        public bool Equals(Point other)
        {
            if (other == null)
                return false;

            return (this.X == other.X && this.Y == other.Y && this.A == other.A && this.B == other.B);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
                return (this.Equals((Point)obj));
            else
                return false;
        }

        public override int GetHashCode()
        {
            int hashCode = HashCode.Start
                .Hash(A)
                .Hash(B)
                .Hash(X)
                .Hash(Y);
            return hashCode;
        }

        public Point Add(Point other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (A != other.A || B != other.B)
                throw new Exception(string.Format("Points {0}, {1} are not on the same curve", this, other));

            if (X == null)
            {
                // return other point
                return new Point(other.A, other.B, other.X, other.Y);
            }
            else if (other.X == null)
            {
                // return this point
                return new Point(A, B, X, Y);
            }
            else if (X == other.X && Y != other.Y)
            {
                // return point at infinity
                return Point.CreatePointAtInfinity(A, B);
            }
            else if (X != other.X)
            {
                // Formula (x3,y3) = (x1,y1)+(x2,y2)
                // s = (y2-y1)/(x2-x1)
                var x1 = X;
                var y1 = Y;
                var x2 = other.X;
                var y2 = other.Y;

                var s = (y2 - y1) / (x2 - x1);
                // x3 = s^2-x1-x2
                var x3 = s.Pow(2) - x1 - x2;
                // y3 = s*(x1-x3)-y1
                var y3 = s * (x1 - x3) - y1;

                return new Point(A, B, x3, y3);
            }
            else if (X == other.X && Y == other.Y)
            {
                // Formula (x3,y3) = (x1,y1)+(x1,y1)
                // s = (3*x1^2+a)/(2*y1)
                var x1 = X;
                var y1 = Y;

                var s = (x1.Pow(2).Multiply(3) + A) / (y1.Multiply(2));

                // x3 = s^2-2*x1
                var x3 = s.Pow(2) - x1.Multiply(2);
                // y3 = s*(x1-x3)-y1
                var y3 = s * (x1 - x3) - y1;

                return new Point(A, B, x3, y3);
            }
            else
            {
                // Shouldnt ever occur - just here for readability
                throw new Exception("Invalid combination of X and Y values");
            }
        }

        public Point Multiply(BigInteger coefficient)
        {
            // Start at Infinity
            var product = Point.CreatePointAtInfinity(A, B);

            for (BigInteger i = 0; i < coefficient; i++)
                product = product.Add(this);

            return product;
        }
    }
}
