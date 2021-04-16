using System.IO;

namespace SummaryCreator.IO
{
    public class FileSystemService
    {
        public string[] FindAllFilesInDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return new string[0];
            }

            return Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        }
    }
}
