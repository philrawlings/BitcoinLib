using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLib
{
    public struct HashCode
    {
        private readonly int hashCode;

        public HashCode(int hashCode)
        {
            this.hashCode = hashCode;
        }

        public static HashCode Start
        {
            get { return new HashCode(17); }
        }

        public static implicit operator int(HashCode hashCode)
        {
            return hashCode.GetHashCode();
        }

        public HashCode Hash<T>(T obj)
        {
            var c = EqualityComparer<T>.Default;
            var h = c.Equals(obj, default(T)) ? 0 : obj.GetHashCode();
            unchecked { h += this.hashCode * 31; }
            return new HashCode(h);
        }

        public override int GetHashCode()
        {
            return this.hashCode;
        }
    }
}
