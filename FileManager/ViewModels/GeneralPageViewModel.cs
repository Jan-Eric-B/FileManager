using FileManager.Models;
using FileManager.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using Wpf.Ui.Common.Interfaces;

namespace FileManager.ViewModels
{
    public class GeneralPageViewModel : ObservableObject
    {
        #region Main

        private bool _dataInitialized;

        public ContainerViewModel Container { get; set; }

        #region Cards Expander

        private bool cardExpanderMove;
        public bool CardExpanderMove
        {
            get { return cardExpanderMove; }
            set
            {
                cardExpanderMove = value;
                OnPropertyChanged(nameof(CardExpanderMove));
            }
        }

        private bool cardExpanderRename;
        public bool CardExpanderRename
        {
            get { return cardExpanderRename; }
            set
            {
                cardExpanderRename = value;
                OnPropertyChanged(nameof(CardExpanderRename));
            }
        }

        private bool cardExpanderDelete;
        public bool CardExpanderDelete
        {
            get { return cardExpanderDelete; }
            set
            {
                cardExpanderDelete = value;
                OnPropertyChanged(nameof(CardExpanderDelete));
            }
        }

        #endregion Cards Expander

        #endregion Main

        #region Move

        private bool moveDirectoryNameCountUp = false;
        public bool MoveDirectoryNameCountUp
        {
            get { return moveDirectoryNameCountUp; }
            set
            {
                moveDirectoryNameCountUp = value;
                OnPropertyChanged(nameof(MoveDirectoryNameCountUp));
            }
        }

        private bool moveDirectoryUseFileName = true;
        public bool MoveDirectoryUseFileName
        {
            get { return moveDirectoryUseFileName; }
            set
            {
                moveDirectoryUseFileName = value;
                OnPropertyChanged(nameof(MoveDirectoryUseFileName));
            }
        }

        private string moveDirectoryName;
        public string MoveDirectoryName
        {
            get => moveDirectoryName;
            set
            {
                moveDirectoryName = value;
                OnPropertyChanged(nameof(MoveDirectoryName));
            }
        }

        #endregion Move

        #region Rename

        #region Replace

        private bool renameReplaceByString = true;
        public bool RenameReplaceByString
        {
            get { return renameReplaceByString; }
            set
            {
                renameReplaceByString = value;
                OnPropertyChanged(nameof(RenameReplaceByString));
            }
        }

        private bool renameReplaceByIndex = false;
        public bool RenameReplaceByIndex
        {
            get { return renameReplaceByIndex; }
            set
            {
                renameReplaceByIndex = value;
                OnPropertyChanged(nameof(RenameReplaceByIndex));
            }
        }

        // Input (String)
        private string renameReplaceInputString;
        public string RenameReplaceInputString
        {
            get => renameReplaceInputString;
            set
            {
                renameReplaceInputString = value;
                OnPropertyChanged(nameof(RenameReplaceInputString));
            }
        }

        // Start (Index)
        private int renameReplaceInputStartInt;
        public int RenameReplaceInputStartInt
        {
            get => renameReplaceInputStartInt;
            set
            {
                renameReplaceInputStartInt = value;
                OnPropertyChanged(nameof(RenameReplaceInputStartInt));
            }
        }
        // Lenght (Index)
        private int renameReplaceInputLengthInt;
        public int RenameReplaceInputLengthInt
        {
            get => renameReplaceInputLengthInt;
            set
            {
                renameReplaceInputLengthInt = value;
                OnPropertyChanged(nameof(RenameReplaceInputLengthInt));
            }
        }


        // Output
        private string renameReplaceOutputString;
        public string RenameReplaceOutputString
        {
            get => renameReplaceOutputString;
            set
            {
                renameReplaceOutputString = value;
                OnPropertyChanged(nameof(RenameReplaceOutputString));
            }
        }



        #endregion




        #endregion


        //____________________________________________________________

        #region Main

        public GeneralPageViewModel(ContainerViewModel container)
        {
            Container = container;
        }

        public static void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
            if (!_dataInitialized)
                InitializeData();
        }

        private void InitializeData()
        {
            _dataInitialized = true;
        }

        #endregion Main

        #region Move

        public void MoveToMainPath()
        {
            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked && file.FileNameWithSubdirectory.Length != file.FilePath.Length)
                {
                    File.Move(file.FilePath, CheckExistenceService.RenameIfExists(Container.MainPath + file.FileName));
                }
            }
            Container.SearchAsync();
        }

        public void MoveToSamePath()
        {
            string newdir = Container.MainPath + MoveDirectoryName + Path.DirectorySeparatorChar;

            if (!Directory.Exists(newdir))
            {
                Directory.CreateDirectory(newdir);
            }

            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked)
                {
                    File.Move(file.FilePath, CheckExistenceService.RenameIfExists(newdir + file.FileName));
                }
            }

            Container.SearchAsync();
        }

        public void MoveToSinglePath()
        {
            if (MoveDirectoryUseFileName)
            {
                foreach (FileModel file in Container.Files)
                {
                    if (file.IsChecked)
                    {
                        string newdir = Container.MainPath + file.FileNameWithoutExtension + Path.DirectorySeparatorChar;

                        if (!Directory.Exists(newdir))
                        {
                            Directory.CreateDirectory(newdir);
                        }
                        //else
                        //{
                        //    newdir = CheckExistenceService.RenameIfExists(newdir);
                        //}

                        File.Move(file.FilePath, CheckExistenceService.RenameIfExists(newdir + file.FileName));
                    }
                }
            }
            else
            {
                int count = 0;

                foreach (FileModel file in Container.Files)
                {
                    if (file.IsChecked)
                    {
                        count++;

                        string newdir = Container.MainPath + NameCount(count) + Path.DirectorySeparatorChar;

                        if (!Directory.Exists(newdir))
                        {
                            Directory.CreateDirectory(newdir);
                        }

                        File.Move(file.FilePath, CheckExistenceService.RenameIfExists(newdir + file.FileName));
                    }
                }
            }
            Container.SearchAsync();
        }

        private string NameCount(int count)
        {
            if (MoveDirectoryName.Contains('0'))
            {
                string countString = string.Empty;

                if (MoveDirectoryName.Contains("00"))
                {
                    switch (Container.FileCountSelected)
                    {
                        case <= 99:
                            countString = count.ToString("D" + 2);
                            break;

                        case >= 100 and <= 999:
                            countString = count.ToString("D" + 3);
                            break;

                        case >= 1000 and <= 9999:
                            countString = count.ToString("D" + 4);
                            break;

                        case >= 10000 and <= 99999:
                            countString = count.ToString("D" + 5);
                            break;
                    }
                    return MoveDirectoryName.Replace("00", countString);
                }
                return MoveDirectoryName.Replace("0", count.ToString());
            }
            return MoveDirectoryName;
        }

        #endregion Move

        #region Delete

        public void DeleteItem()
        {
            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked && CheckExistenceService.Check(file.FilePath))
                {
                    FileSystem.DeleteFile(file.FilePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
            }

            Container.SearchAsync();
        }

        public void DeleteItemPermanently()
        {
            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked && CheckExistenceService.Check(file.FilePath))
                {
                    FileSystem.DeleteFile(file.FilePath, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
                }
            }

            Container.SearchAsync();
        }

        #endregion Delete
    }
}