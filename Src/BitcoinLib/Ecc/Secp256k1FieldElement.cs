using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLib.Ecc
{
    public class Secp256k1FieldElement : FieldElement
    {
        public Secp256k1FieldElement(BigInteger num) : base(num, Secp256k1Constants.P)
        {
        }

        public Secp256k1FieldElement Sqrt()
        {
            var val = BigInteger.ModPow(this.Num, (Secp256k1Constants.P + 1) / 4, Secp256k1Constants.P);
            var sqrt = val.Mod(Secp256k1Constants.P);
            return new Secp256k1FieldElement(sqrt);
        }

        public string ToHex(bool lowerCase = true)
        {
            return Num.PositiveValToHexString(64, lowerCase);
        }

        public override string ToString()
        {
            return ToHex();
        }
    }
}
