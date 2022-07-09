# PoopCrypt
Very bad custom encoder and decoder

# Features
 - Can use your byte encryption tricks
 - Turns classes and structures into a sequence of bytes and encrypts them
 - Decodes bytes into classes/structs very quickly
 - Slowly turns classes/structs into bytes, so you can insert your ad or save animation ^.^
 - Supports some built-in C# types
 - Has built-in stupid-coded byte encoders

# Usage example
```csharp
    var crypter = new PoorCrypt.Crypter(); // create a new Cryptor instance
    
    var me = new Person { Name = "Andrey", IQ = 0 }; // sample
    byte[] meAsBytes = crypter.Encode(me); // encrypt "me" into bytes
    Person meFromBytes = crypter.Decode<Person>(meAsBytes); // decrypt "me" from bytes

    public struct Person {
        [PoorCrypt.EncryptMe] public string Name;
        [PoorCrypt.EncryptMe] public int IQ;
    }
```

# Some problems and their solution
 - List, Hashset and similar types are not supported because I'm dumb to figure out how to combine them. However, you can add your own translator of classes / structures to bytes and vice versa.
```csharp
    class MaybeYourListCrypter : PoopCrypt.ITypeCrypter {
        public object Decrypt(byte[] data, ref int jump) {
            // your code
        }
        public byte[] Encrypt(object target) {
            // your code
        }
    }
```
 - For classes that have only primitive types inside, you can create an autoclass
```csharp
    var example = new Crypter();
    example.GenerateAutoTypeCrypterFor<Bar>(true);
    byte[] bytes = example.Encrypt(someFoo);

    public struct Foo {
        public Bar bar;
    }

    public struct Foo {
        public Bar bar;
    }
````
