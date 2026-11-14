// C# program to find dublicate files in folders
//created on 12 November 2026 by Daniils Luka Mishkins
using System.Security.Cryptography;
using System;
using System.IO;

class DupeFile
{

    static void Main(string[] args)
    {
        
        try
        {
            new DupeFile().Run();
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.Error.WriteLine($"Not enough authority to access the file or folder: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.Error.WriteLine($"Cannot open CSV file or files folder: {ex.Message}");
        }
                      
    }



    public void Run()
    {
        var pathfiles = @"C:\Users\lukam\OneDrive\Desktop\test c#";
        var pathfolder = @"C:\Users\lukam\source\repos\DupeFinderConsole\DupeFinder\all_files.csv";

        DirectoryInfo place = new DirectoryInfo(pathfiles);
        FileInfo[] Files = place.GetFiles("*", SearchOption.AllDirectories);
        Console.WriteLine("Files are:");
        Console.WriteLine();

        using var writer = new StreamWriter(pathfolder); //csv file with file names
        writer.WriteLine("Name,Hash");
        int length = Files.Length;
        string[] hashes = new string[length];

        int i = 0;
        foreach (FileInfo file in Files) // hashing files for easy and fast comparison
        {
            using var stream = File.OpenRead(file.FullName);
            string hash = Convert.ToHexString(SHA256.HashData(stream));
            hashes[i] = hash;
            Console.WriteLine($"{file.Name}: {hash}");
            writer.WriteLine($"{file.Name}, {hash}");
            i++;

        }
        string[,] group = new string[length, 1];
        //int number = 0;
        int j = 0;
        foreach(string hash in hashes) //making hash groups
        {
            bool same = false;

            for (int l=0; l<j; l++)
            {
                if (group[l, 0] == hash)
                {
                    same = true;
                    break;
                }

            }

            if (same == false)
            {
                group[j, 0] = hash;
                j++;
            }


        }
        
        writer.WriteLine($"Grouped by identical hash");

        for (int l = 0;  l<length; l++) //grouping by hash and writing into CSV file
        {
            if (group[l, 0] == null) break;
            writer.WriteLine($"Group {l+1}");
            
                for (int k = 0; k<length; k++)
                {
                    if (group[l, 0] == hashes[k])
                    {
                    writer.WriteLine(Files[k].Name);
                    }
                }

            

        }

        Console.WriteLine(Path.GetFullPath("all_files.csv"));
    }
}