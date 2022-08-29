using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models
{
    public class CapitalizationChangesList
    {
        public ObservableCollection<FileModel> Old;

        public ObservableCollection<FileModel> New;

    }
}
