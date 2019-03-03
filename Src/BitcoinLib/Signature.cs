using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace BitcoinLib
{
    public class Signature
    {
        public BigInteger R { get; private set; }
        public BigInteger S { get; private set; }
        public Signature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }

        public void Serialize(Stream stream)
        {
            var rBytes = R.PositiveValToBigEndianBytes();
            var sBytes = S.PositiveValToBigEndianBytes();

            // If the first byte has its highest bit set (>0x7F), 
            // BER (of which DER is a specialization of) says that it is to be interpreted as a negative number
            // Therefore we add an extra 00 to the beginning to ensure it is treated as a positive number
            // https://bitcoin.stackexchange.com/questions/12554/why-the-signature-is-always-65-13232-bytes-long

            bool rPrefix;
            int rLength;
            if (rBytes.Length > 0 && rBytes[0] > 0x7F)
            {
                rPrefix = true;
                rLength = rBytes.Length + 1;
            }
            else
            {
                rPrefix = false;
                rLength = rBytes.Length;
            }

            bool sPrefix;
            int sLength;
            if (sBytes.Length > 0 && sBytes[0] > 0x7F)
            {
                sPrefix = true;
                sLength = sBytes.Length + 1;
            }
            else
            {
                sPrefix = false;
                sLength = sBytes.Length;
            }

            var totalLength = Convert.ToByte(rLength + sLength);

            stream.WriteByte(0x30);
            stream.WriteByte(totalLength);
            stream.WriteByte(0x02);
            stream.WriteByte(Convert.ToByte(rLength));
            if (rPrefix)
                stream.WriteByte(0);
            stream.Write(rBytes, 0, rBytes.Length);
            stream.WriteByte(0x02);
            stream.WriteByte(Convert.ToByte(sLength));
            if (sPrefix)
                stream.WriteByte(0);
            stream.Write(sBytes, 0, sBytes.Length);

            // Note this doesnt include the sighash byte at the end - this is only required for transaction signatures.
        }

        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                Serialize(ms);
                return ms.ToArray();
            }
        }
    }
}
