using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLib
{
    public static class HexUtilities
    {
        private static readonly uint[] _lookup32Lower = CreateLookup32(true);
        private static readonly uint[] _lookup32Upper = CreateLookup32(false);

        private static uint[] CreateLookup32(bool lowerCase)
        {
            var result = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                string s;
                if (lowerCase)
                    s = i.ToString("x2");
                else
                    s = i.ToString("X2");
                result[i] = ((uint)s[0]) + ((uint)s[1] << 16);
            }
            return result;
        }

        public static string BytesToHexString(byte[] bytes, bool lowerCase = true)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            var lookup32 = lowerCase ? _lookup32Lower : _lookup32Upper;
            var result = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                var val = lookup32[bytes[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }
            return new string(result);
        }

        public static byte[] HexStringToBytes(string hexString)
        {
            if (hexString == null)
                throw new ArgumentNullException(nameof(hexString));

            if ((hexString.Length & 1) != 0)
            {
                // Prefix with zero so there are an even number of characters, since we need two chars per byte
                hexString = "0" + hexString;
            }
            byte[] ret = new byte[hexString.Length / 2];
            for (int i = 0; i < ret.Length; i++)
            {
                int high = ParseNibble(hexString[i * 2]);
                int low = ParseNibble(hexString[i * 2 + 1]);
                ret[i] = (byte)((high << 4) | low);
            }

            return ret;
        }

        private static int ParseNibble(char c)
        {
            unchecked
            {
                uint i = (uint)(c - '0');
                if (i < 10)
                    return (int)i;
                i = ((uint)c & ~0x20u) - 'A';
                if (i < 6)
                    return (int)i + 10;
                throw new ArgumentException("Invalid nybble: " + c);
            }
        }
    }
}
