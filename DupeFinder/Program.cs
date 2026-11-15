// C# program to find duplicate files in downloads folder ( can be changed)
// works as a single console project in a solution
//created on 12 November 2026 by Daniils Luka Mishkins
//finalyzed on 15 November 2026
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
        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); // finding downloads folder
        string pathfiles = Path.Combine(userProfile, "Downloads");

        DirectoryInfo place = new DirectoryInfo(pathfiles);
        FileInfo[] Files = place.GetFiles("*", SearchOption.AllDirectories);
        var pathfolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "all_files.csv");//creating table on desktop


        Console.WriteLine("Files are:");
        Console.WriteLine();

        using var writer = new StreamWriter(pathfolder); //csv file with file names
        writer.WriteLine($"Files are taken from {pathfiles}");
        writer.WriteLine($"and written into {pathfolder}");
        int length = Files.Length;
        string[] hashes = new string[length];

        int i = 0;
        foreach (FileInfo file in Files) // hashing files for easy and fast comparison
        {
            using var stream = File.OpenRead(file.FullName);
            string hash = Convert.ToHexString(SHA256.HashData(stream));
            hashes[i] = hash;
            Console.WriteLine($"{file.FullName}: {hash}");
            //writer.WriteLine($"{file.FullName}, {hash}");
            i++;

        }
        Dictionary<string, int> groups = new Dictionary<string, int>();
        

        foreach(string hash in hashes) //making hash groups
        {
            if (groups.ContainsKey(hash))
            {
                groups[hash]++;
            }
            else
            {
                groups[hash] = 1;
            }


        }
        
        writer.WriteLine($"Grouped by identical hash");
        List<string> writtenhashes = new List<string>();
        int groupnumber = 1;
        if (groups.Count == 0) //checking if there are any files
        {
            writer.WriteLine("No files were in the folders");
   
        }
        else
        {
            writer.WriteLine("================");
            writer.WriteLine("These files might be deleted (THIS IS A SUGGESTION AND NOT A RULE): ");
            writer.WriteLine("");

        }
        for (int l = 0;  l<length; l++) //grouping by hash and writing into CSV file
        {
            if (groups.Count == 0)
            {
                break;
            }


            if (groups.ContainsKey(hashes[l]) && groups[hashes[l]] < 2)
            {
               
                continue;
            }

            if (!writtenhashes.Contains(hashes[l]))
            {
                writer.WriteLine($"Group {groupnumber}:");
            }
            if (!writtenhashes.Contains(hashes[l]))
            {
                for (int k = 0; k < length; k++)
                {


                    if (hashes[k] == hashes[l])
                    {
                        writer.WriteLine($"{Files[k].FullName}");
                        if (!writtenhashes.Contains(hashes[k]))
                        {
                            writtenhashes.Add(hashes[k]);
                        }
                    }
                }
            }
            else continue;

            groupnumber++;

        }

        Console.WriteLine(Path.GetFullPath(pathfolder));
    }
}