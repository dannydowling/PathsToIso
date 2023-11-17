using DiscUtils.Iso9660;
using System.Text;
public class IsoCreator
{
    public void Main(string[] args)
    {
        string rootFolderPath = "";
        string outputFolderPath = "";

        if (args.Length! < 2)
        { Console.WriteLine("To use this, specify a source folder and a destination folder."); }
        else
        {
            args[0] = rootFolderPath;
            args[1] = outputFolderPath;
            CreateIso(rootFolderPath, outputFolderPath);
        }
    }
    public void CreateIso(string rootFolderPath, string outputFolderPath)
    {
        StringBuilder sb = new StringBuilder();

        string[] allFolders = Directory.GetDirectories(rootFolderPath);

        // here we're trying to split up all the subfolders into only the top level ones under 
        // where the utility is pointed.
        for (int i = 0; i < allFolders[i].Split(rootFolderPath).Count(); i++)
        {            
            if (!Directory.Exists(outputFolderPath))
            {    Directory.CreateDirectory(outputFolderPath);    }

            sb.Append(allFolders[i]);                   // c:\sources\things\whatsit...
            sb.Remove(0, rootFolderPath.Length);        // remove c:\sources\things\
         
            // create whatsit.iso, use recursion to the builder
            string isoFilePath = Path.Combine(outputFolderPath, $"{allFolders[i]}.iso");
            using (FileStream isoStream = new FileStream(isoFilePath, FileMode.Create))
            {
                CDBuilder builder = new CDBuilder();
                builder.UseJoliet = true;
                string volumeName = allFolders[i].ToUpper();
                if (volumeName.Length > 31)
                {   volumeName.Take(31);  }
                volumeName.Remove(' ');
                builder.VolumeIdentifier = volumeName;

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
