using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLib
{
    public static class ByteArrayUtilities
    {
        public static bool CompareByteArrays(byte[] arr1, byte[] arr2)
        {
            int length = arr1.Length;
            if (length != arr2.Length)
            {
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                if (arr1[i] != arr2[i])
                    return false;
            }
            return true;
        }

        public static byte[] ConcatenateByteArrays(byte[] arr1, byte[] arr2)
        {
            var dataToEncode = new byte[arr1.Length + arr2.Length];
            Buffer.BlockCopy(arr1, 0, dataToEncode, 0, arr1.Length);
            Buffer.BlockCopy(arr2, 0, dataToEncode, arr1.Length, arr2.Length);
            return dataToEncode;
        }
    }
}
