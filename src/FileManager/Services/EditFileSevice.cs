using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;


namespace FileManager.Services
{
    public static class EditFileSevice
    {
        /// <summary>
        /// Checks if string is a path to a file or directory and if it's also existing
        /// </summary>
        public static bool Check(string path)
        {
            return Directory.Exists(path) || File.Exists(path);
        }

        /// <summary>
        /// Checks if string is a path to a directory and if it's also existing
        /// </summary>
        public static bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Checks if string is a path to a file and if it's also existing
        /// </summary>
        public static bool IsFile(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// TEMP
        /// Checks if file exists (Case Sensitive)
        /// </summary>
        public static bool FileExistsCaseSensitive(string filename)
        {
            string name = Path.GetDirectoryName(filename);

            if (name != null && Array.Exists(Directory.GetFiles(name), s => s == Path.GetFullPath(filename)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks like Windows if file exists and renames it if necessary.
        /// </summary>
        public static string CheckForExistingFile(string oldFile, string newFile)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(newFile);
            string extension = Path.GetExtension(newFile);
            string directoryName = Path.GetDirectoryName(newFile);

            string newFullPath = newFile;

            if (!string.Equals(oldFile, newFile, StringComparison.OrdinalIgnoreCase))
            {
                int count = 1;
                while (Check(newFullPath))
                {
                    string tempFileName = string.Format("{0}({1})", fileNameWithoutExtension, count++);
                    newFullPath = Path.Combine(directoryName, tempFileName + extension);
                }
            }
            return newFullPath;
        }

        /// <summary>
        /// Moves/renames file
        /// </summary>
        public static (bool, string) FileMove(string input, string output)
        {
            try
            {
                string newOutput = CheckForExistingFile(input, output);
                File.Move(input, newOutput);
                return (true, newOutput);
            }
            catch (UnauthorizedAccessException)
            {
                //StartApplicationAsAdministrator.StartAsAdmin();
            }
            catch (Exception ex)
            {
                MessageBoxService.MessageBoxOK("Error", ex.Message);
            }
            return (false, "");
        }

        /// <summary>
        /// Deletes specific file
        /// </summary>
        public static bool FileDelete(string input)
        {
            try
            {
                if (Check(input))
                {
                    FileSystem.DeleteFile(input, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (UnauthorizedAccessException)
            {
                //StartApplicationAsAdministrator.StartAsAdmin();
            }
            catch (Exception ex)
            {
                MessageBoxService.MessageBoxOK("Error", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Deletes specific file permanently
        /// </summary>
        public static bool FileDeletePermanent(string input)
        {
            try
            {
                if (Check(input))
                {
                    FileSystem.DeleteFile(input, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (UnauthorizedAccessException)
            {
                //StartApplicationAsAdministrator.StartAsAdmin();
            }
            catch(Exception ex)
            {
                MessageBoxService.MessageBoxOK("Error", ex.Message);
            }
            return false;
        }








    }
}
