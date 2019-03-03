using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLib
{
    public static class BigIntegerExtensionMethods
    {
        /// <summary>
        /// <para>Modulo Operation: Ensuring that the result is positive - to emulate the Python % operator</para>
        /// <para>https://en.wikipedia.org/wiki/Modulo_operation</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static BigInteger Mod(this BigInteger x, BigInteger m)
        {
            return (x % m + m) % m;
        }

        public static byte[] PositiveValToBigEndianBytes(this BigInteger x)
        {
            if (x < 0)
                throw new Exception("Value is less than 0");

            var bytes = x.ToByteArray();
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);


            // Count leading zeros
            int leadingZeros = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0)
                    leadingZeros++;
                else
                    break;
            }

            if (leadingZeros != 0)
            {
                // Discard leading zeros
                var truncatedBytes = new byte[bytes.Length - leadingZeros];
                Buffer.BlockCopy(bytes, leadingZeros, truncatedBytes, 0, truncatedBytes.Length);
                return truncatedBytes;
            }
            else
                return bytes;
        }

        public static void PositiveValToBigEndianBytes(this BigInteger x, byte[] buffer, int bufferOffset, int numBytes)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            if (x < 0)
                throw new Exception("Value is less than 0");

            var bytes = x.ToByteArray();
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);


            // Count leading zeros
            int leadingZeros = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0)
                    leadingZeros++;
                else
                    break;
            }

            int dataLength = bytes.Length - leadingZeros;

            if (dataLength > numBytes)
                throw new Exception("Data too large for buffer");

            int startPos = bufferOffset + (numBytes - dataLength);

            // Clear buffer section
            Array.Clear(buffer, bufferOffset, numBytes);

            // Copy to buffer, right aligned
            Buffer.BlockCopy(bytes, leadingZeros, buffer, startPos, dataLength);
        }

        public static string PositiveValToHexString(this BigInteger x, bool lowerCase = true)
        {
            var bytes = x.PositiveValToBigEndianBytes();
            string hexString = HexUtilities.BytesToHexString(bytes, lowerCase);
            if (hexString.StartsWith("0"))
                return hexString.Substring(1);
            else
                return hexString;
        }

        public static string PositiveValToHexString(this BigInteger x, int minWidth, bool lowerCase = true)
        {
            var minBytes = minWidth / 2 + minWidth % 2;
            var bytes = x.PositiveValToBigEndianBytes();
            if (bytes.Length >= minBytes)
                return HexUtilities.BytesToHexString(bytes, lowerCase);
            else
            {
                byte[] expanded = new byte[minBytes];
                Buffer.BlockCopy(bytes, 0, expanded, expanded.Length - bytes.Length, bytes.Length);
                string hexString = HexUtilities.BytesToHexString(expanded, lowerCase);
                if (hexString.Length > minWidth)
                    return hexString.Substring(1);
                else
                    return hexString;
            }

        }
    }
}
