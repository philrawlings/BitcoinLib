using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BitcoinLib
{
    public static class Base58Utilities
    {
        private const string Base58Values = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

        private static string EncodeBase58(byte[] data)
        {
            StringBuilder sb = new StringBuilder(33);
            BigInteger dividend = 0;
            for (int i = 0; i < data.Length; ++i)
            {
                dividend = dividend * 256 + data[i];
            }
            while (dividend > 0)
            {
                BigInteger rem;
                dividend = BigInteger.DivRem(dividend, 58, out rem);
                sb.Insert(0, Base58Values[(int)rem]);
            }

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 0)
                    sb.Insert(0, Base58Values[0]);
                else
                    break;
            }

            return sb.ToString();
        }

        private static byte[] DecodeBase58Alternative(string data)
        {
            // Decode Base58 string to BigInteger 
            BigInteger intData = 0;
            for (var i = 0; i < data.Length; i++)
            {
                var digit = Base58Values.IndexOf(data[i]); //Slow

                if (digit < 0)
                {
                    throw new FormatException(string.Format("Invalid Base58 character `{0}` at position {1}", data[i], i));
                }

                intData = intData * 58 + digit;
            }

            // Encode BigInteger to byte[]
            // Leading zero bytes get encoded as leading `1` characters
            var leadingZeroCount = data.TakeWhile(c => c == '1').Count();
            var leadingZeros = Enumerable.Repeat((byte)0, leadingZeroCount);
            var bytesWithoutLeadingZeros =
              intData.ToByteArray()
              .Reverse()// to big endian
              .SkipWhile(b => b == 0);//strip sign byte
            var result = leadingZeros.Concat(bytesWithoutLeadingZeros).ToArray();

            return result;
        }


        public static byte[] DecodeBase58(string base58String)
        {
            var num = new BigInteger(0);
            var leadingZeroCount = 0;
            bool leadingZeroCheck = true;

            foreach (var c in base58String)
            {
                num *= 58;
                num += Base58Values.IndexOf(c);
                if (c == '1' && leadingZeroCheck)
                {
                    leadingZeroCount++;
                }
                else
                {
                    leadingZeroCheck = false;
                }
            }

            byte[] rawBytes = num.PositiveValToBigEndianBytes();

            if (leadingZeroCount == 0)
                return rawBytes;
            else
            {
                var bytes = new byte[leadingZeroCount + rawBytes.Length];
                Buffer.BlockCopy(rawBytes, 0, bytes, leadingZeroCount, rawBytes.Length);
                return bytes;
            }
        }

        public static string EncodeBase58Check(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            // Calculate checksum (First 4 bytes of Double SHA256 of data)
            byte[] checksum = CalculateChecksum(data);

            // Concatenate data and checksum
            byte[] dataToEncode = ByteArrayUtilities.ConcatenateByteArrays(data, checksum);

            // Encode Base58 and return
            return EncodeBase58(dataToEncode);
        }

        public static byte[] DecodeBase58Check(string base58String)
        {
            byte[] bytes = DecodeBase58(base58String);

            byte[] checksum = new byte[4];
            Buffer.BlockCopy(bytes, bytes.Length - checksum.Length, checksum, 0, checksum.Length);

            byte[] data = new byte[bytes.Length - checksum.Length];
            Buffer.BlockCopy(bytes, 0, data, 0, data.Length);

            byte[] calculatedChecksum = CalculateChecksum(data);

            if (!ByteArrayUtilities.CompareByteArrays(checksum, calculatedChecksum))
                throw new Exception("Checksum failure");

            return data;
        }

        private static byte[] CalculateChecksum(byte[] data)
        {
            var checksumData = HashUtilities.DoubleSha256(data);
            var checksum = new byte[4];
            Buffer.BlockCopy(checksumData, 0, checksum, 0, 4);
            return checksum;
        }
    }
}
