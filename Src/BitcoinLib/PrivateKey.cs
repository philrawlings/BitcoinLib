using BitcoinLib.Ecc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLib
{
    public class PrivateKey
    {
        public BigInteger Value { get; private set; }
        public bool TestNet { get; private set; }
        public bool WifCompressed { get; private set; }

        public PrivateKey(BigInteger value, bool testNet = false, bool wifCompressed = true)
        {
            if (value < 1 || value >= Secp256k1Constants.N)
                throw new ArgumentOutOfRangeException(nameof(value));

            Value = value;
            TestNet = testNet;
            WifCompressed = wifCompressed;
        }

        public static PrivateKey CreateFromWifString(string wifString)
        {
            bool testNet;
            bool wifCompressed;
            byte[] rawBytes = Base58Utilities.DecodeBase58Check(wifString);
            byte[] privateKeyBytes = new byte[32];

            // Compressed is 34 bytes, uncompressed is 33 bytes
            if (rawBytes.Length == 34 || rawBytes.Length == 33)
            {
                Buffer.BlockCopy(rawBytes, 1, privateKeyBytes, 0, 32);
                if (rawBytes[0] == 0xEF)
                    testNet = true;
                else if (rawBytes[0] == 0x80)
                    testNet = false;
                else
                    throw new ArgumentException(nameof(wifString) + " prefix is invalid");

                wifCompressed = rawBytes.Length == 34;
            }
            else
                throw new ArgumentException(nameof(wifString) + " is invalid");

            var val = BigIntegerUtilities.CreateFromUnsignedBigEndianBytes(privateKeyBytes);
            return new PrivateKey(val, testNet, wifCompressed);
        }

        public string GetWifString()
        {
            var bytes = GetWifBytes();
            return Base58Utilities.EncodeBase58Check(bytes);
        }

        // Is this the correct name for this?
        public byte[] GetWifBytes()
        {
            byte prefix;
            if (TestNet)
                prefix = 0xEF;
            else
                prefix = 0x80;

            byte[] buffer;
            if (WifCompressed)
            {
                // Compressed WIF has additional 0x01 at the end - making the compressed form longer than the uncompressed one.
                // This is correct as it signals that compressed private keys should be generated.
                buffer = new byte[34];
                buffer[0] = prefix;
                buffer[33] = 0x01;
                Value.PositiveValToBigEndianBytes(buffer, 1, 32);
            }
            else
            {
                buffer = new byte[33];
                buffer[0] = prefix;
                Value.PositiveValToBigEndianBytes(buffer, 1, 32);
            }

            return buffer;
        }

        public Signature Sign(byte[] data, int offset, int count)
        {
            var hash = HashUtilities.Sha256(data, offset, count);
            var z = BigIntegerUtilities.CreateFromUnsignedBigEndianBytes(hash).Mod(Secp256k1Constants.N);
            return Sign(z);
        }

        public Signature Sign(byte[] data)
        {
            var hash = HashUtilities.Sha256(data);
            var z = BigIntegerUtilities.CreateFromUnsignedBigEndianBytes(hash).Mod(Secp256k1Constants.N);
            return Sign(z);
        }

        public Signature Sign(BigInteger z)
        {
            // Generate random number k (ephemeral private key)
            // TODO: this should be replaced with deterministic initialisation of k as shown here:
            // https://tools.ietf.org/html/rfc6979
            BigInteger k;
            do
            {
                var bytes = new byte[32];
                using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(bytes);
                }
                k = BigIntegerUtilities.CreateFromUnsignedBigEndianBytes(bytes);
            } while (k < 1 || k > Secp256k1Constants.N);

            // r is the x coordinate of the resulting point k*G (ephemeral public key)
            var r = BitcoinConstants.G.Multiply(k).X.Num;

            // 1/k = pow(k, N-2, N)
            var k_inv = BigInteger.ModPow(k, Secp256k1Constants.N - 2, Secp256k1Constants.N).Mod(Secp256k1Constants.N);

            // s = (z+r*prvKey) / k
            var s = ((z + (r * this.Value)) * k_inv).Mod(Secp256k1Constants.N);

            return new Signature(r, s);
        }

        public PublicKey GetPublicKey()
        {
            return PublicKey.Create(this);
        }
    }
}
