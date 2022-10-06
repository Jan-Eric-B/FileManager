using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Windows.Graphics.Printing.PrintSupport;

namespace FileManager.Services
{
    public static class ReadSettings
    {
        public static List<string> ReadFormats(string format)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;

            if (format != null)
            {
                if(format == "image")
                {
                    if(!EditFileSevice.Check(path + "ImageFormats.ini"))
                    {
                        try
                        {
                            File.Create((path + "ImageFormats.ini"));
                        }
                        catch
                        {
                            MessageBoxService.MessageBoxOK("ERROR", "Can't find and create 'Resources\\ImageFormats.ini'");
                            return null;
                        }

                    }
                    else
                    {
                        return File.ReadLines(path + "ImageFormats.ini").ToList();

                    }
                }
                if (format == "video")
                {
                    if (!EditFileSevice.Check(path + "VideoFormats.ini"))
                    {
                        try
                        {
                            File.Create((path + "VideoFormats.ini"));
                        }
                        catch
                        {
                            MessageBoxService.MessageBoxOK("ERROR", "Can't find and create 'Resources\\VideoFormats.ini'");
                            return null;
                        }

                    }
                    else
                    {
                        return File.ReadLines(path + "ImageFormats.ini").ToList();

                    }
                }
                return null;
            }
            return null;
        }

        public static List<string> ReadFormat(string format)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + format;

            if (!EditFileSevice.Check(path))
            {
                try
                {
                    File.Create(path);

                    if (path.EndsWith("ImageFormats.ini"))
                    {
                        File.WriteAllText(path, ".png", ".jpg", ".gif", Encoder.ReferenceEquals);
                    }
                    if (path.EndsWith("VideoFormats.ini"))
                    {

                    }
                }
                catch
                {

                }
            }
        }


    }
}
