using DiscUtils.Iso9660;
using System.Text;

class IsoCreator
{
    public void Main(string[] args)
    {
        string rootFolderPath = "";
        string outputFolderPath = "";

      if (args.Length < 1)
{
    if (args.Length < 2)
    {
        { Console.WriteLine("To use this, specify a source folder and a destination folder."); }
    }
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
        List<string> folderNames = new List<string>();

        foreach (var folderPath in Directory.GetDirectories(rootFolderPath))
        {
            folderNames.Add(folderPath); // add the full path of all the folders to the list
            sb.Clear();
        }

        for (int i = 0; i < folderNames.Count; i++)
        {
            //split the part of the remaining path up by folders
            string[] subStrings = folderNames[i].Split(rootFolderPath + '/');

            foreach (var item in subStrings)
            {
                // Ensure the output directory exists
                if (!Directory.Exists(outputFolderPath))
                {
                    Directory.CreateDirectory(outputFolderPath);
                }

                // Loop through each folder in the specified path
                foreach (var folderPath in Directory.GetDirectories(rootFolderPath))
                {
                    // Create a new ISO file for each folder
                    string folderName = Path.GetFileName(folderPath);
                    string isoFilePath = Path.Combine(outputFolderPath, $"{folderName}.iso");

                    using (FileStream isoStream = new FileStream(isoFilePath, FileMode.Create))
                    {

                        CDBuilder builder = new CDBuilder();
                        builder.UseJoliet = true;
                        builder.VolumeIdentifier = folderName;

                        // Add all files and subdirectories to the ISO
                        AddDirectoryContents(builder, folderPath, outputFolderPath);

                        builder.Build(isoStream);
                    }

                    Console.WriteLine($"ISO file created: {isoFilePath}");
                }
            }
        }
    }

    private static void AddDirectoryContents(CDBuilder builder, string sourcePath, string targetPath)
    {
        foreach (var file in Directory.GetFiles(sourcePath))
        {
            // Add files to the ISO
            builder.AddFile(targetPath + Path.GetFileName(file), file);
        }

        foreach (var subdirectory in Directory.GetDirectories(sourcePath))
        {
            // Recursively add subdirectories
            string subdirectoryName = Path.GetFileName(subdirectory);
            AddDirectoryContents(builder, subdirectory, $"{targetPath}{subdirectoryName}/");
        }
    }

}


