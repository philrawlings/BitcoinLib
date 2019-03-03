using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLib.Ecc
{
    public static class Secp256k1Constants
    {
        public static readonly BigInteger P = BigInteger.Pow(2, 256) - BigInteger.Pow(2, 32) - new BigInteger(977);
        public static readonly BigInteger N = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141");
        public static readonly FieldElement A = new FieldElement(0, P);
        public static readonly FieldElement B = new FieldElement(7, P);
    }
}
