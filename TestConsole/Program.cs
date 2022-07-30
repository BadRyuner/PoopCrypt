//#define AlsoDecrypt

using System.Linq;
using static System.Console;
using PoopCrypt;

internal class Program
{
    static void Main(string[] args) // just example
    {
        var crypter = new Crypter();

        if (args.Length > 0)
        {
            WriteLine("Decrypted:");
            WriteLine("-----------------------------------------------");
            WriteLine($"{crypter.Decrypt<MyData>(System.IO.File.ReadAllBytes(args[0]))}");
            WriteLine("-----------------------------------------------");
            ReadLine();
            return;
        }

        var data = new MyData() { Date = System.DateTime.Now, SomeString = System.IO.File.ReadAllText("input.txt") };
        var encrypted = crypter.Encrypt(data);

#if AlsoDecrypt
        var decrypted = crypter.Decrypt<MyData>(encrypted.ToArray());

        WriteLine("-------Encrypted and Decrypted!-----------");
        //Write("- As Chars: ");
        //foreach (var b in encrypted)
        //    Write($"{(char)b} ");
        WriteLine($"Decrypted:\n{decrypted}");
        WriteLine("------------------------------------------");
#else
        System.IO.File.WriteAllBytes($"output.txt", encrypted.ToArray()); // 1:44
        WriteLine("Done!");
#endif
        ReadLine();
    }

    public struct MyData
    {
        [EncryptMe] public System.DateTime Date;
        [EncryptMe] public string SomeString;
        public override string ToString() => $"Date: {Date}\nSomeString: {SomeString}";
        //public override string ToString() => SomeString;
    }

    /* public struct MegaSecureMyData
    {
        [EncryptMe] public MyData data;
    }

    public struct OverMegaSecureMyData
    {
        [EncryptMe] public MegaSecureMyData data;
    } */
}