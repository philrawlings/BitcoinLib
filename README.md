# BitcoinLib

Bitcoin Library for .NET, written in C#. This library is for educational purposes to explore the various features of Bitcoin.

*Warning: when creating private keys, always use input values with sufficiently high entropy*

## Create Public and Private Key Pairs

```C#
    var prvKey = new PrivateKey(BigInteger.Pow(888, 3));
    var pubKey = prvKey.GetPublicKey();
    var address = pubKey.GetAddress();
    Console.WriteLine(address); // 148dY81A9BmdpMhvYEVznrM45kWN32vSCN
```
## Sign and Verify Messages

```C#
    var message = Encoding.ASCII.GetBytes("This is a test message");
    // Generate signature using private key
    var prvKey = new PrivateKey(1234567890);
    var sig = prvKey.Sign(message);
    var pubKeyBytes = prvKey.GetPublicKey().GetPublicKeyBytes();
    
    // Verify using only publicly shared information (public key, message and signature)
    var pubKey = PublicKey.Parse(pubKeyBytes);
    bool valid = pubKey.Verify(message, sig);
    Console.WriteLine(valid); // true
```
