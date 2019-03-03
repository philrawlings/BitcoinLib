using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BitcoinLib
{
    public static class HashUtilities
    {
        public static byte[] Sha256(byte[] data, int offset, int count)
        {
            var hSha256 = new SHA256CryptoServiceProvider();
            return hSha256.ComputeHash(data, offset, count);
        }

        public static byte[] Sha256(byte[] data)
        {
            var hSha256 = new SHA256CryptoServiceProvider();
            return hSha256.ComputeHash(data);
        }

        public static byte[] DoubleSha256(byte[] data, int offset, int count)
        {
            return Sha256(Sha256(data, offset, count));
        }

        public static byte[] DoubleSha256(byte[] data)
        {
            return Sha256(Sha256(data));
        }

        public static byte[] RipeMd160(byte[] data, int offset, int count)
        {
            var h160 = RIPEMD160Managed.Create();
            return h160.ComputeHash(data, offset, count);
        }

        public static byte[] RipeMd160(byte[] data)
        {
            var h160 = RIPEMD160Managed.Create();
            return h160.ComputeHash(data);
        }

        public static byte[] Hash160(byte[] data, int offset, int count)
        {
            var sha256Hash = Sha256(data, offset, count);
            return RipeMd160(sha256Hash);
        }

        public static byte[] Hash160(byte[] data)
        {
            var sha256Hash = Sha256(data);
            return RipeMd160(sha256Hash);
        }
    }
}
