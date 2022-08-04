using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
