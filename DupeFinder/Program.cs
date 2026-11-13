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
        string[,] group = new string[length, length+1];
        int number = 0;
        int j = 0;
        

        foreach (string hash in hashes) // finding dublicates and grouping
        {
            if (group[number,0]==hash)
            {
                group[number, j+1] = Files[j].Name;
                
            }
            else if (group[number, 0]!=hash && number!=0)
            {
                number++;
                group[number, j+1] = Files[j].Name;

            }
            else if (group[number, 0] != hash)
            {
                group[number, j+1] = Files[j].Name;

            }
            j++;

        }

        //int column = 0;
        for (int k = 0; k<length; k++) //writing result
        {
            
                writer.WriteLine($"Group {k}");
                int filesize=0;
                for (int column=0; column<length; column++)
                {
                    long fullgroupsize = 0;
                    if (group[k, column] != null)
                    {

                    //long sizeInBytes = new FileInfo(@$"{pathfiles}\{group[k,column]}").Length;
                    //writer.WriteLine($"{group[k,column]}, {sizeInBytes} Bytes");
                    //fullgroupsize += sizeInBytes;
                    writer.WriteLine($"{group[k,column]}");
                }
                //writer.WriteLine($"Group {k} size{fullgroupsize} Bytes");

            }

        }

        Console.WriteLine(Path.GetFullPath("all_files.csv"));
    }
}