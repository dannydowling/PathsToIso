﻿using DiscUtils.Iso9660;
using System.Text;

class IsoCreator
{
    public void Main(string[] args)
    {
        string rootFolderPath = "";
        string outputFolderPath = "";

        if (args.Length! < 2)
        {
            { Console.WriteLine("To use this, specify a source folder and a destination folder."); }
        }
        else
        {
            args[0] = rootFolderPath;
            args[1] = outputFolderPath;
            CreateIso(rootFolderPath, outputFolderPath);
        }
    }

    public void CreateIso(string rootFolderPath, string outputFolderPath)
    {
        //path logic 
        StringBuilder sb = new StringBuilder();

        string[] allFolders = Directory.GetDirectories(rootFolderPath);

        // here we're trying to split up all the subfolders into only the top level ones under 
        // where the utility is pointed.

        for (int i = 0; i < allFolders[i].Split(rootFolderPath).Count(); i++)
        {
            // Ensure the output directory exists
            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }
            
            //remove the substring locating where on the source we are.
            // the folders before the top level that we care about
            sb.Remove(0, rootFolderPath.Length); 

            // Create a new ISO file for each folder in the source. Use the name in the string builder
           
            string isoFilePath = Path.Combine(outputFolderPath, $"{allFolders[i]}.iso");

            using (FileStream isoStream = new FileStream(isoFilePath, FileMode.Create))
            {

                CDBuilder builder = new CDBuilder();
                builder.UseJoliet = true;
                builder.VolumeIdentifier = allFolders[i];

                // Add all files and subdirectories to the ISO
                AddDirectoryContents(builder, sb.ToString(), outputFolderPath);

                builder.Build(isoStream);
            }

            sb.Clear();

            Console.WriteLine($"ISO file created: {isoFilePath}");
        }
    }
    private static void AddDirectoryContents(CDBuilder builder, string sourcePath, string targetPath)
    {
        foreach (var subdirectory in Directory.GetDirectories(sourcePath))
        {
            // Recursively add subdirectories
            string subdirectoryName = Path.GetFileName(subdirectory);
            AddDirectoryContents(builder, subdirectory, $"{targetPath}{subdirectoryName}/");
        }

        foreach (var file in Directory.GetFiles(sourcePath))
        {
            // Add files to the ISO
            builder.AddFile(targetPath + Path.GetFileName(file), file);
        }      
    }
}
