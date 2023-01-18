using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileManager.Models;
using Windows.Media.Protection.PlayReady;
using System.Net;

namespace FileManager.Services
{
    public static class ExtensionsService
    {
        public static bool CheckExistance(string file)
        {
            string applicationPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);


            return EditFileSevice.Check(applicationPath + "\\" + file);
        }

        public static string GetPath(string file)
        {
            string applicationPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            return applicationPath + "\\Extensions\\" + file;
        }


        public static void DownloadApplication(string file)
        {
            if (file == "exiftool(-k).exe")
            {
                StartProcessService.Start("https://exiftool.org/", false, false);
            }
            else if (file == "ffmpeg.exe")
            {
                StartProcessService.Start("https://www.gyan.dev/ffmpeg/builds/", false, false);
            }
        }



}
}
