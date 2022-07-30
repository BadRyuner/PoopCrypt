//#define AlsoDecrypt

using System.Linq;
using static System.Console;
using PoopCrypt;

internal class Program
{
    static void Main(string[] args) // just example
    {
        var crypter = new Crypter();
        var timer = new System.Diagnostics.Stopwatch();

        if (args.Length > 0)
        {   
            timer.Start();
            var result = crypter.Decrypt<MyData>(System.IO.File.ReadAllBytes(args[0])); 
            timer.Stop();
            WriteLine("Decrypted:");
            WriteLine("-----------------------------------------------");
            WriteLine(result);
            WriteLine($"-----------------------------------------------\nDone in {timer.ElapsedMilliseconds}ms!");
            ReadLine();
            return;
        }

        var data = new MyData() { Date = System.DateTime.Now, SomeString = System.IO.File.ReadAllText("input.txt") };
        timer.Start();
        var encrypted = crypter.Encrypt(data); 
        timer.Stop();

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
        WriteLine($"Done in {timer.ElapsedMilliseconds}ms!");
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