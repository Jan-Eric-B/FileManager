using System.IO;

namespace FileManager.Services
{
    internal static class CheckExistenceService
    {
        public static bool Check(string path)
        {
            return Directory.Exists(path) || File.Exists(path);
        }

        public static bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        public static string RenameIfExists(string file)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
            string extension = Path.GetExtension(file);
            string directoryName = Path.GetDirectoryName(file);

            string newFullPath = file;

            int count = 1;
            while (Check(newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileNameWithoutExtension, count++);
                newFullPath = Path.Combine(directoryName, tempFileName + extension);
            }
            return newFullPath;
        }
    }
}
