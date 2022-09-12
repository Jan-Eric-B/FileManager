using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Threading;

namespace FileManager.Models
{
    /// <summary>
    /// Model for holding information of files
    /// </summary>
    public class FileModel : ObservableObject
    {
        /// <summary>
        /// Path of file: 'C:\Windows\System32\file.png'
        /// </summary>
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

        /// <summary>
        /// Name of file: 'file.png'
        /// </summary>
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

        /// <summary>
        /// Name of file without extension: 'file'
        /// </summary>
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

        /// <summary>
        /// Extension of filename: '.png'
        /// </summary>
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

        /// <summary>
        /// The name of the directory: 'C:\Windows\System32\'
        /// </summary>
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

        /// <summary>
        /// Difference from the MainPath and the file path (subdirectories and filename): 'System32\file.png'
        /// </summary>
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

        /// <summary>
        /// Difference from the MainPath and the file path (subdirectories ): 'System32\'
        /// </summary>
        private string subdirectoryPath;
        public string SubdirectoryPath
        {
            get => subdirectoryPath;
            set
            {
                subdirectoryPath = value;
                OnPropertyChanged(nameof(SubdirectoryPath));
            }
        }

        /// <summary>
        /// Size of the file: '3 Mb'
        /// </summary>
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

        /// <summary>
        /// Is checked in the file list
        /// </summary>
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

        private string highlightedText = string.Empty;
        public string HighlightedText
        {
            get => highlightedText;
            set
            {
                highlightedText = value;
                OnPropertyChanged(nameof(HighlightedText));
            }
        }

    }
}