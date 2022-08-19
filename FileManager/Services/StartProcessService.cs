using System;
using System.Diagnostics;
using System.IO;

namespace FileManager.Services
{
    /// <summary>
    /// For starting processes and opening files
    /// </summary>
    internal static class StartProcessService
    {
        /// <summary>
        /// Starts process
        /// </summary>
        /// <param name="useShellExecute"> if true, then the Process class will use the ShellExecute function, otherwise it will use CreateProcess</param>
        public static bool Start(string path, bool useShellExecute)
        {
            try
            {
                Process process = new()
                {
                    StartInfo = new ProcessStartInfo(path) { UseShellExecute = useShellExecute },
                };
                process.Start();
                return true;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                ShowOpenWithDialog(path);
                return true;
            }
            catch (Exception ex)
            {
                MessageBoxService.MessageBoxOK("Error starting process", $"'{path}'" + Environment.NewLine + ex);
                return false;
            }
        }

        /// <summary>
        /// Open the "Open with" dialog if system doesn't know how to start the process
        /// </summary>
        public static void ShowOpenWithDialog(string path)
        {
            var args = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shell32.dll");
            args += ",OpenAs_RunDLL " + path;
            Process.Start("rundll32.exe", args);
        }
    }
}