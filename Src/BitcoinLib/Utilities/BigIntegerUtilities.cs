using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLib
{
    public static class BigIntegerUtilities
    {
        public static BigInteger CreateFromUnsignedBigEndianHexString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value) + " is null or whitespace");

            byte[] bytes = HexUtilities.HexStringToBytes(value);
            return CreateFromUnsignedBigEndianBytes(bytes);
        }

        public static BigInteger CreateFromUnsignedBigEndianBytes(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            if (bytes.Length > 0)
            {
                // Check that last byte (most significant) has the MSB set to zero
                if ((bytes[bytes.Length - 1] & 0x80) != 0)
                {
                    var expandedBytes = new byte[bytes.Length + 1];
                    Buffer.BlockCopy(bytes, 0, expandedBytes, 0, bytes.Length);
                    return new BigInteger(expandedBytes);
                }
                else
                    return new BigInteger(bytes);
            }
            else
                return new BigInteger(0);
        }
    }
}
