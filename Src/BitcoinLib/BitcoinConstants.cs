using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitcoinLib.Ecc;

namespace BitcoinLib
{
    public static class BitcoinConstants
    {
        private static Secp256k1Point _G = null;

        public static Secp256k1Point G
        {
            get
            {
                if (_G == null)
                {
                    var x = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("79be667ef9dcbbac55a06295ce870b07029bfcdb2dce28d959f2815b16f81798");
                    var y = BigIntegerUtilities.CreateFromUnsignedBigEndianHexString("483ada7726a3c4655da4fbfc0e1108a8fd17b448a68554199c47d08ffb10d4b8");
                    _G = new Secp256k1Point(x, y);
                }
                return _G;
            }
        }
    }
}
