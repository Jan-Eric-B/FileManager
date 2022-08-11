using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using FileManager.Models.Data;

namespace FileManager.Models
{
    public class FileModel
    {
        public ObservableCollection<FileData> Files = new ObservableCollection<FileData>();
    }
}
