// C# program to find dublicate files in folders
//created on 12 November 2026 by Daniils Luka Mishkins
using System.Security.Cryptography;
using System;
using System.IO;

class DupeFile
{

    static void Main(string[] args)
    {

        using var writer = new StreamWriter("all_files.csv"); //csv file with file names
        writer.WriteLine("Name,Hash");


        DirectoryInfo place = new DirectoryInfo(@"C:\Users\lukam\OneDrive\Desktop\test c#");
        FileInfo[] Files = place.GetFiles("*",SearchOption.AllDirectories);
        Console.WriteLine("Files are:");
        Console.WriteLine();

        foreach (FileInfo file in Files) // hashing files for easy and fast comparison
        {
            using var stream = File.OpenRead(file.FullName);
            string hash = Convert.ToHexString(SHA256.HashData(stream));
            Console.WriteLine($"{file.Name}: {hash}");
            writer.WriteLine($"{file.Name}: {hash}");

        }
    }
}