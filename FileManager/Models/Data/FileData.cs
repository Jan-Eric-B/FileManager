using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace FileManager.Models.Data
{
    public class FileData : ObservableObject
    {

        //C:\Windows\System32\file.png
        private string filePath;
        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        //file.png
        private string fileName;
        public string FileName
        {
            get => fileName;
            set
            {
                fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }

        //file
        private string fileNameWithoutExtension;
        public string FileNameWithoutExtension
        {
            get => fileNameWithoutExtension;
            set
            {
                fileNameWithoutExtension = value;
                OnPropertyChanged(nameof(FileNameWithoutExtension));
            }
        }

        //.png
        private string extension;
        public string Extension
        {
            get => extension;
            set
            {
                extension = value;
                OnPropertyChanged(nameof(Extension));
            }
        }

        //C:\Windows\System32\
        private string directoryName;
        public string DirectoryName
        {
            get => directoryName;
            set
            {
                directoryName = value;
                OnPropertyChanged(nameof(DirectoryName));
            }
        }

        //System32\file.png
        private string fileNameWithSubdirectory;
        public string FileNameWithSubdirectory
        {
            get => fileNameWithSubdirectory;
            set
            {
                fileNameWithSubdirectory = value;
                OnPropertyChanged(nameof(FileNameWithSubdirectory));
            }
        }

        //3 Mb
        private string fileSize;
        public string FileSize
        {
            get => fileSize;
            set
            {
                fileSize = value;
                OnPropertyChanged(nameof(FileSize));
            }
        }

        //Is checked
        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }
    }
}
