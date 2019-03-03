using BitcoinLib.Ecc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLib
{
    public class PublicKey
    {
        private static Secp256k1Point _K;

        public BigInteger X
        {
            get { return _K.X.Num; }
        }

        public BigInteger Y
        {
            get { return _K.Y.Num; }
        }

        public bool TestNet { get; private set; }
        public bool Compressed { get; private set; }

        public PublicKey(BigInteger x, BigInteger y, bool testNet = false, bool compressed = true)
        {
            _K = new Secp256k1Point(x, y);
            TestNet = testNet;
            Compressed = compressed;
        }

        internal static PublicKey Create(PrivateKey privateKey)
        {
            var point = BitcoinConstants.G.Multiply(privateKey.Value);
            return new PublicKey(point.X.Num, point.Y.Num, privateKey.TestNet, privateKey.WifCompressed);
        }

        public static PublicKey Parse(string hexString, bool testNet = false)
        {
            var bytes = HexUtilities.HexStringToBytes(hexString);
            return Parse(bytes, testNet);
        }

        public static PublicKey Parse(byte[] bytes, bool testNet = false)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            else if (bytes.Length == 33)
            {
                bool yIsEven;

                if (bytes[0] == 0x02)
                    yIsEven = true;
                else if (bytes[0] == 0x03)
                    yIsEven = false;
                else
                    throw new Exception(string.Format("Compressed public key found (33 bytes) with invalid prefix. Expected 0x02 or 0x03, found 0x{0:X2}", bytes[0]));

                var buffer = new byte[32];
                Buffer.BlockCopy(bytes, 1, buffer, 0, buffer.Length);
                var x = BigIntegerUtilities.CreateFromUnsignedBigEndianBytes(buffer);
                var y = Secp256k1Point.GetYCoordinate(new Secp256k1FieldElement(x), yIsEven);
                return new PublicKey(x, y.Num, testNet, true);
            }
            else if (bytes.Length == 65)
            {
                if (bytes[0] != 0x04)
                    throw new Exception(string.Format("Uncompressed public key found (65 bytes) with invalid prefix. Expected 0x04, found 0x{0:X2}", bytes[0]));
                var buffer = new byte[32];
                Buffer.BlockCopy(bytes, 1, buffer, 0, buffer.Length);
                var x = BigIntegerUtilities.CreateFromUnsignedBigEndianBytes(buffer);
                Buffer.BlockCopy(bytes, 33, buffer, 0, buffer.Length);
                var y = BigIntegerUtilities.CreateFromUnsignedBigEndianBytes(buffer);
                return new PublicKey(x, y, testNet, false);
            }
            else
                throw new Exception(nameof(bytes) + " length is invalid, expected 33 bytes (compressed pub key) or 65 bytes (uncompressed pub key)");
        }

        public string GetPublicKeyHexString(bool lowerCase = true)
        {
            return HexUtilities.BytesToHexString(GetPublicKeyBytes(), lowerCase);
        }

        // Is this the correct name for this?
        public byte[] GetPublicKeyBytes()
        {
            if (_K == Secp256k1Point.Infinity)
                throw new Exception("Cannot get public key for point at infinity");

            byte[] bytes;

            if (Compressed)
            {
                // Compressed
                bytes = new byte[33]; // type byte + 32 bytes
                if (this.Y.Mod(2) == 0)
                {
                    // Y is Even
                    bytes[0] = 0x02;
                    X.PositiveValToBigEndianBytes(bytes, 1, 32);
                }
                else
                {
                    // Y is Odd
                    bytes[0] = 0x03;
                    X.PositiveValToBigEndianBytes(bytes, 1, 32);
                }
            }
            else
            {
                // Uncompressed
                bytes = new byte[65]; // type byte + 32 bytes for X + 32 bytes for Y
                bytes[0] = 0x04;
                X.PositiveValToBigEndianBytes(bytes, 1, 32);
                Y.PositiveValToBigEndianBytes(bytes, 33, 32);
            }

            return bytes;
        }

        public string GetAddress()
        {
            var sec = GetPublicKeyBytes();
            var h160 = HashUtilities.Hash160(sec);

            byte[] rawBytes = new byte[21];
            if (TestNet)
                rawBytes[0] = 0x6F;
            else
                rawBytes[0] = 0x00;
            Buffer.BlockCopy(h160, 0, rawBytes, 1, h160.Length);

            return Base58Utilities.EncodeBase58Check(rawBytes);
        }

        public bool Verify(byte[] data, Signature s)
        {
            var hash = HashUtilities.Sha256(data);
            var z = BigIntegerUtilities.CreateFromUnsignedBigEndianBytes(hash).Mod(Secp256k1Constants.N);
            return Verify(z, s);
        }

        public bool Verify(BigInteger z, Signature s)
        {
            // 1/s = pow(s, N-2, N)
            var s_inv = BigInteger.ModPow(s.S, Secp256k1Constants.N - 2, Secp256k1Constants.N).Mod(Secp256k1Constants.N);
            // u = z / s
            var u = (z * s_inv).Mod(Secp256k1Constants.N);
            // v = r / s
            var v = (s.R * s_inv).Mod(Secp256k1Constants.N);
            // u*G + v*PubKey should have as the x coordinate, r
            var total = BitcoinConstants.G.Multiply(u).Add(_K.Multiply(v));
            return total.X.Num == s.R;
        }
    }
}
