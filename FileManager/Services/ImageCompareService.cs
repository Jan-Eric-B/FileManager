using FileManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using XnaFan.ImageComparison;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace FileManager.Services
{
    public static class ImageCompareService
    {
        /// <summary>
        /// Compares Images and groups them together
        /// </summary>
        /// <param name="files">ObservableCollection of files that should be checked</param>
        /// <param name="percentage">The threshold/sensitivy</param>
        /// <param name="mainPath">Main path where the new folders are created</param>
        /// <param name="directoryName">name of the new folders</param>
        /// <returns></returns>
        public static async Task Compare(ObservableCollection<FileModel> files, double percentage, string mainPath, string directoryName)
        {
            // If Collection exists and is filled
            if (!files.Safe().Any())
            {
                // Check if "scene" folders already exist and count up
                int scene = 1;
                for (int i = 0; i < files.Count; i++)
                {
                    if (Directory.Exists(mainPath + directoryName + scene))
                    {
                        scene++;
                    }
                }

                for (int i = 0; i < files.Count; i++)
                {
                    string sceneDirectory = mainPath + directoryName + scene + System.IO.Path.DirectorySeparatorChar;

                    try
                    {
                        //Create new "scene" folder
                        if (!EditFileSevice.Check(sceneDirectory))
                        {
                            Directory.CreateDirectory(sceneDirectory);
                        }

                        // Calculate difference percentage
                        float percentageDifference = await CompareTwoImages(files[i].FilePath, files[i + 1].FilePath).ConfigureAwait(false);


                        // Move first file to the same "scene" folder
                        File.Move(files[i].FilePath, sceneDirectory + files[i].FileName);

                        // Are not the same or similar (Difference smaller than the percentage/tolerance(Threshold)
                        if (percentageDifference > percentage)
                        {                            
                            //jumps to the next 
                            scene++;
                        }
                    }
                    //catches because last File has nothing to compair
                    catch (ArgumentOutOfRangeException)
                    {
                        File.Move(files[i].FilePath, sceneDirectory + files[i].FileName);
                    }
                    catch
                    {

                    }
                }
            }
        }

        // Compares the two images
        public static async Task<float> CompareTwoImages(string img1, string img2)
        {
            float percentageDifference = ImageTool.GetPercentageDifference(img1, img2);
            return percentageDifference;
        }
    }
}
