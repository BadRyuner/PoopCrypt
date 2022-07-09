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

        var data = new MyData() { SomeString = System.IO.File.ReadAllText("input.txt") };
        var encrypted = crypter.Encrypt(data);
        var decrypted = crypter.Decrypt<MyData>(encrypted);

        System.IO.File.WriteAllBytes($"output.txt", encrypted);

        WriteLine("-------Encrypted and Decrypted!-----------");
        //Write("- As Chars: ");
        //foreach (var b in encrypted)
        //    Write($"{(char)b} ");
        WriteLine($"Decrypted:\n{decrypted}");
        WriteLine("------------------------------------------");
        ReadLine();
    }

    public struct MyData
    {
        [EncryptMe] public string SomeString;
        public override string ToString() => $"SomeString: {SomeString}";
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