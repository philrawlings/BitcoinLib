using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLib.Ecc
{
    public class FieldElement : IEquatable<FieldElement>
    {
        // Note that C# % operator returns a value with the same sign as the Dividend
        // Extension method .Mod() created to return a value with the same sign as the divisor (same as Python % operator).

        public BigInteger Num { get; private set; }
        public BigInteger Prime { get; private set; }

        public FieldElement(BigInteger num, BigInteger prime)
        {
            if (prime <= 0)
                throw new Exception(nameof(prime) + " cannot be less than or equal to 0");
            if (num >= prime || num < 0)
                throw new Exception(nameof(num) + " is not in range 0 to prime)");
            Num = num;
            Prime = prime;
        }

        public static bool operator ==(FieldElement element1, FieldElement element2)
        {
            if (ReferenceEquals(element1, element2))
                return true;
            else if ((object)element1 == null || (object)element2 == null)
                return false;
            else
                return element1.Equals(element2);
        }

        public static bool operator !=(FieldElement element1, FieldElement element2)
        {
            return !(element1 == element2);
        }

        public bool Equals(FieldElement other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return (this.Num == other.Num && this.Prime == other.Prime);
        }

        public override bool Equals(object obj)
        {
            if (obj is FieldElement)
                return (this.Equals((FieldElement)obj));
            else
                return false;
        }

        public override int GetHashCode()
        {
            int hashCode = HashCode.Start
                .Hash(Num)
                .Hash(Prime);
            return hashCode;
        }

        public FieldElement Add(FieldElement other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (Prime != other.Prime)
                throw new Exception("Primes must be the same");

            var num = (Num + other.Num).Mod(Prime);
            return new FieldElement(num, Prime);
        }

        public static FieldElement operator +(FieldElement element1, FieldElement element2)
        {
            if (element1 == null || element2 == null)
                return null;
            else
                return element1.Add(element2);
        }

        public FieldElement Subtract(FieldElement other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (Prime != other.Prime)
                throw new Exception("Primes must be the same");

            var num = (Num - other.Num).Mod(Prime);
            return new FieldElement(num, Prime);

        }

        public static FieldElement operator -(FieldElement element1, FieldElement element2)
        {
            if (element1 == null || element2 == null)
                return null;
            else
                return element1.Subtract(element2);
        }

        public FieldElement Multiply(FieldElement other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (Prime != other.Prime)
                throw new Exception("Primes must be the same");

            var num = (Num * other.Num).Mod(Prime);
            return new FieldElement(num, Prime);
        }

        public static FieldElement operator *(FieldElement element1, FieldElement element2)
        {
            if (element1 == null || element2 == null)
                return null;
            else
                return element1.Multiply(element2);
        }

        public FieldElement Multiply(BigInteger coefficient)
        {
            var num = (Num * coefficient).Mod(Prime);
            return new FieldElement(num, Prime);
        }

        public static FieldElement operator *(FieldElement element1, BigInteger element2)
        {
            if (element1 == null || element2 == null)
                return null;
            else
                return element1.Multiply(element2);
        }

        public static FieldElement operator *(BigInteger element1, FieldElement element2)
        {
            if (element1 == null || element2 == null)
                return null;
            else
                return element2.Multiply(element1);
        }

        public FieldElement Pow(BigInteger n)
        {
            var num = BigInteger.ModPow(Num, n, Prime).Mod(Prime);
            return new FieldElement(num, Prime);
        }

        public FieldElement TrueDiv(FieldElement other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (Prime != other.Prime)
                throw new Exception("Primes must be the same");

            var num = (Num * BigInteger.ModPow(other.Num, Prime - 2, Prime)).Mod(Prime);
            return new FieldElement(num, Prime);
        }

        public static FieldElement operator /(FieldElement element1, FieldElement element2)
        {
            if (element1 == null || element2 == null)
                return null;
            else
                return element1.TrueDiv(element2);
        }

        public override string ToString()
        {
            return string.Format("FieldElement({0},{1})", Num, Prime);
        }
    }
}
