using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

namespace FileManager.Models
{
    internal class FileModel : INotifyPropertyChanged
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
        public string FileName()
        {
            return Path.GetFileName(FilePath);
        }

        //file
        public string FileNameWithoutExtension()
        {
            return Path.GetFileNameWithoutExtension(FilePath);
        }

        //.png
        public string Extension()
        {
            return Path.GetExtension(FilePath);
        }

        //C:\Windows\System32\
        public string DirectoryName()
        {
            return Path.GetDirectoryName(FilePath);
        }


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

        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion OnPropertyChanged
    }
}
