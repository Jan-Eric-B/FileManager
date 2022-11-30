using Brain2CPU.ExifTool;
using CommunityToolkit.Mvvm.ComponentModel;
using FileManager.Models;
using FileManager.Resources;
using FileManager.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.Design;
using System.Windows.Threading;
using Windows.Media.Playlists;
using Windows.Storage;
using Wpf.Ui.Common;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;
using static System.Net.WebRequestMethods;
using Dispatcher = Microsoft.AspNetCore.Components.Dispatcher;
using File = System.IO.File;

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
        private string renameReplaceInputString = string.Empty;
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
        private int renameReplaceInputStartInt = 1;
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
        private int renameReplaceInputLengthInt = 0;
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
        private string renameReplaceOutputString = string.Empty;
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

        #region Swap

        // Input
        private string renameSwapInputString = " - ";
        public string RenameSwapInputString
        {
            get => renameSwapInputString;
            set
            {
                renameSwapInputString = value;
                OnPropertyChanged(nameof(RenameSwapInputString));
            }
        }

        // Part One
        private int renameSwapPartOneInt = 1;
        public int RenameSwapPartOneInt
        {
            get => renameSwapPartOneInt;
            set
            {
                renameSwapPartOneInt = value;
                OnPropertyChanged(nameof(RenameSwapPartOneInt));
            }
        }

        // Part Two
        private int renameSwapPartTwoInt = 2;
        public int RenameSwapPartTwoInt
        {
            get => renameSwapPartTwoInt;
            set
            {
                renameSwapPartTwoInt = value;
                OnPropertyChanged(nameof(RenameSwapPartTwoInt));
            }
        }

        #endregion

        #region Insert

        // Input (String)
        private string renameInsertInputString = string.Empty;
        public string RenameInsertInputString
        {
            get => renameInsertInputString;
            set
            {
                renameInsertInputString = value;
                OnPropertyChanged(nameof(RenameInsertInputString));
            }
        }

        private bool renameInsertCountUp = false;
        public bool RenameInsertCountUp
        {
            get { return renameInsertCountUp; }
            set
            {
                renameInsertCountUp = value;
                OnPropertyChanged(nameof(RenameInsertCountUp));
            }
        }

        #endregion


        #region Capitalization

        private bool renameCapitalizationUndo = true;
        public bool RenameCapitalizationUndo
        {
            get { return renameCapitalizationUndo; }
            set
            {
                renameCapitalizationUndo = value;
                OnPropertyChanged(nameof(RenameCapitalizationUndo));

            }
        }



        #endregion

        #endregion
        private readonly ISnackbarService _snackbarService;
        private ExifToolWrapper _exif;

        //____________________________________________________________

        #region Main

        public GeneralPageViewModel(ContainerViewModel container, ISnackbarService snackbarService)
        {
            Container = container;
            _snackbarService = snackbarService;
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

        private string NameCount(string input, int count)
        {
            if (input.Contains('0'))
            {
                string countString = string.Empty;

                if (input.Contains("00"))
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
                    return input.Replace("00", countString);
                }
                return input.Replace("0", count.ToString());
            }
            return input;
        }

        #endregion Main

        #region Move

        public async void MoveToMainPath()
        {
            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked && file.FileNameWithSubdirectory.Length != file.FilePath.Length)
                {
                    (bool, string) result = EditFileSevice.FileMove(file.FilePath, Container.MainPath + file.FileName);

                    if (result.Item1)
                    {
                        await UpdateFileAsync(file, Path.GetFileNameWithoutExtension(result.Item2));
                    }
                }
            }
            await Container.SearchAsync();
        }

        public async void MoveToSamePath()
        {
            string newdir = Container.MainPath + MoveDirectoryName + Path.DirectorySeparatorChar;

            if (!EditFileSevice.IsDirectory(newdir))
            {
                Directory.CreateDirectory(newdir);
            }

            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked)
                {
                    (bool, string) result = EditFileSevice.FileMove(file.FilePath, newdir + file.FileName);

                    if (result.Item1)
                    {
                        await UpdateFileAsync(file, Path.GetFileNameWithoutExtension(result.Item2));
                    }
                }
            }

            await Container.SearchAsync();
        }

        public async void MoveToSinglePath()
        {
            if (MoveDirectoryUseFileName)
            {
                foreach (FileModel file in Container.Files)
                {
                    if (file.IsChecked)
                    {
                        string newdir = Container.MainPath + file.FileNameWithoutExtension + Path.DirectorySeparatorChar;

                        if (!EditFileSevice.IsDirectory(newdir))
                        {
                            Directory.CreateDirectory(newdir);
                        }
                        //else
                        //{
                        //    newdir = CheckExistenceService.RenameIfExists(newdir);
                        //}

                        (bool, string) result = EditFileSevice.FileMove(file.FilePath, newdir + file.FileName);

                        if (result.Item1)
                        {
                            await UpdateFileAsync(file, Path.GetFileNameWithoutExtension(result.Item2));
                        }
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

                        string newdir = Container.MainPath + NameCount(MoveDirectoryName, count) + Path.DirectorySeparatorChar;

                        if (!EditFileSevice.IsDirectory(newdir))
                        {
                            Directory.CreateDirectory(newdir);
                        }

                        EditFileSevice.FileMove(file.FilePath, newdir + file.FileName);
                    }
                }
            }
            await Container.SearchAsync();
        }

        #endregion Move

        #region Delete

        public async void DeleteItem()
        {
            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked)
                {
                    EditFileSevice.FileDelete(file.FilePath);
                }
            }

            await Container.SearchAsync();
        }

        public async void DeleteItemPermanently()
        {
            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked)
                {
                    EditFileSevice.FileDeletePermanent(file.FilePath);
                }
            }

            await Container.SearchAsync();
        }

        public async Task DeleteItemMetaData()
        {
            await _snackbarService.ShowAsync("Removing MetaData", "Please wait", SymbolRegular.Clock24);


            _exif = new ExifToolWrapper();
            _exif.Start();
           
            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked && EditFileSevice.Check(file.FilePath) && ExifToolSupportedFiles.Writeable.Keys.ToList().Contains(file.Extension.Replace(".", "").ToUpper()))
                {
                    ExifToolResponse r = _exif.ClearExif(file.FilePath);
                }
            }
            _exif.Dispose();
            //await Container.SearchAsync();
            await _snackbarService.HideAsync();
            MessageBoxService.MessageBoxOK("Removing MetaData", "Done", 150, 0);
        }

        public async void DeleteEmptyFolders(string startLocation)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                DeleteEmptyFolders(directory);
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }

        #endregion Delete

        #region Rename

        private async Task UpdateFileAsync(FileModel file, string fileNameWithoutExtensionNew)
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                file.FileNameWithoutExtension = fileNameWithoutExtensionNew;
                file.FileName = fileNameWithoutExtensionNew + file.Extension;
                file.FilePath = file.DirectoryName + fileNameWithoutExtensionNew + file.Extension;
                file.FileNameWithSubdirectory = file.FilePath[Container.MainPath.Length..];
                file.SubdirectoryPath = file.DirectoryName[Container.MainPath.Length..];
            }));
        }

        #region Replace

        public async void RenameReplace()
        {
            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked && EditFileSevice.Check(file.FilePath))
                {
                    string fileNameWithoutExtensionNew = string.Empty;

                    //String
                    if (RenameReplaceByString && file.FileNameWithoutExtension.Contains(RenameReplaceInputString))
                    {
                        fileNameWithoutExtensionNew = file.FileNameWithoutExtension.Replace(RenameReplaceInputString, RenameReplaceOutputString);
                    }
                    //Position
                    else if (!RenameReplaceByString)
                    {
                        try
                        {
                            int start = RenameReplaceInputStartInt - 1;
                            int end = RenameReplaceInputLengthInt;
                            fileNameWithoutExtensionNew = file.FileNameWithoutExtension.Remove(start, end).Insert(start, RenameReplaceOutputString);
                        }
                        catch
                        {
                            MessageBoxService.MessageBoxOK("Length was out of range", "Error", 150, 0);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(fileNameWithoutExtensionNew))
                    {
                        (bool, string) result = EditFileSevice.FileMove(file.FilePath, file.DirectoryName + fileNameWithoutExtensionNew + file.Extension);

                        if (result.Item1)
                        {
                            await UpdateFileAsync(file, Path.GetFileNameWithoutExtension(result.Item2));
                        }
                    }
                }
            }
        }

        #endregion

        #region Swap

        public async void RenameSwap()
        {
            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked && file.FileNameWithoutExtension.Contains(RenameSwapInputString))
                {
                    //Splits into parts
                    string[] parts = file.FileNameWithoutExtension.Split(new string[] { RenameSwapInputString }, StringSplitOptions.None);

                    // If NumberBox input to high. Not enough parts
                    if (RenameSwapPartOneInt <= parts.Length && RenameSwapPartTwoInt <= parts.Length && RenameSwapPartOneInt != RenameSwapPartTwoInt)
                    {
                        string fileNameWithoutExtensionNew = string.Empty;

                        for (int i = 0; i < parts.Length; i++)
                        {
                            if (i == RenameSwapPartOneInt - 1)
                            {
                                fileNameWithoutExtensionNew += parts[RenameSwapPartTwoInt - 1] + RenameSwapInputString;
                            }
                            else if (i == RenameSwapPartTwoInt - 1)
                            {
                                fileNameWithoutExtensionNew += parts[RenameSwapPartOneInt - 1] + RenameSwapInputString;
                            }
                            else
                            {
                                fileNameWithoutExtensionNew += parts[i] + RenameSwapInputString;
                            }
                        }
                        //Removes last RenameSwapInputString
                        fileNameWithoutExtensionNew = fileNameWithoutExtensionNew.Remove(fileNameWithoutExtensionNew.Length - RenameSwapInputString.Length);

                        (bool, string) result = EditFileSevice.FileMove(file.FilePath, file.DirectoryName + fileNameWithoutExtensionNew + file.Extension);

                        if (result.Item1)
                        {
                            await UpdateFileAsync(file, Path.GetFileNameWithoutExtension(result.Item2));
                        }
                    }
                }
            }
        }

        #endregion

        #region Insert

        public async void RenameInsertFront()
        {
            int count = 0;

            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked)
                {
                    string RenameInsertInputNew = RenameInsertInputString;

                    if (RenameInsertCountUp)
                    {
                        count++;
                        RenameInsertInputNew = NameCount(RenameInsertInputString, count);
                    }

                    string fileNameWithoutExtensionNew = RenameInsertInputNew + file.FileNameWithoutExtension;

                    (bool, string) result = EditFileSevice.FileMove(file.FilePath, file.DirectoryName + fileNameWithoutExtensionNew + file.Extension);

                    if (result.Item1)
                    {
                        await UpdateFileAsync(file, Path.GetFileNameWithoutExtension(result.Item2));
                    }
                }
            }
        }

        public async void RenameInsertBack()
        {
            int count = 0;

            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked)
                {
                    string RenameInsertInputNew = RenameInsertInputString;

                    if (RenameInsertCountUp)
                    {
                        count++;
                        RenameInsertInputNew = NameCount(RenameInsertInputString, count);
                    }

                    string fileNameWithoutExtensionNew = file.FileNameWithoutExtension + RenameInsertInputNew;

                    (bool, string) result = EditFileSevice.FileMove(file.FilePath, file.DirectoryName + fileNameWithoutExtensionNew + file.Extension);

                    if (result.Item1)
                    {
                        await UpdateFileAsync(file, Path.GetFileNameWithoutExtension(result.Item2));
                    }
                }
            }
        }

        #endregion

        #region Capitalization

        //List<CapitalizationChangesList> CapitalizationChangesLists = new List<CapitalizationChangesList>();

        public async Task ToCapitalization()
        {
           // List<FileModel> backUpValues = Container.Files.ToList();

            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked)
                {
                    string fileNameWithoutExtensionNew = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(file.FileNameWithoutExtension.ToLower());

                    (bool, string) result = EditFileSevice.FileMove(file.FilePath, file.DirectoryName + fileNameWithoutExtensionNew + file.Extension);

                    if (result.Item1)
                    {
                        await UpdateFileAsync(file, Path.GetFileNameWithoutExtension(result.Item2));
                    }
                }
            }
            //CapitalizationChangesLists.Add(new CapitalizationChangesList { Old = backUpValues, New = Container.Files.ToList()});
            //RenameCapitalizationUndo = true;
        }

        public async Task ToUpperCase()
        {
            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked)
                {
                    string fileNameWithoutExtensionNew = file.FileNameWithoutExtension.ToUpper();

                    (bool, string) result = EditFileSevice.FileMove(file.FilePath, file.DirectoryName + fileNameWithoutExtensionNew + file.Extension);

                    if (result.Item1)
                    {
                        await UpdateFileAsync(file, Path.GetFileNameWithoutExtension(result.Item2));
                    }
                }
            }
        }

        public async Task ToLowerCase()
        {
            //List<FileModel> backUpValues = new List<FileModel>();

            foreach (FileModel file in Container.Files)
            {
               // backUpValues.Add(file);


                if (file.IsChecked)
                {
                    string fileNameWithoutExtensionNew = file.FileNameWithoutExtension.ToLower();

                    (bool, string) result = EditFileSevice.FileMove(file.FilePath, file.DirectoryName + fileNameWithoutExtensionNew + file.Extension);

                    if (result.Item1)
                    {
                        await UpdateFileAsync(file, Path.GetFileNameWithoutExtension(result.Item2));
                    }
                }
            }
            //CapitalizationChangesList test = new CapitalizationChangesList { Old = backUpValues, New = Container.Files.ToList() };

            //CapitalizationChangesLists.Add(test);
            //RenameCapitalizationUndo = true;
        }

        //public async Task ToUpperCase()
        //{

        //    List<FileModel> backUpValues = new List<FileModel>(Container.Files);

        //    ToUpperTest();
        //    CapitalizationChangesList test = new CapitalizationChangesList { Old = backUpValues, New = Container.Files.ToList() };

        //    CapitalizationChangesLists.Add(test);
        //    RenameCapitalizationUndo = true;
        //}

        public async Task CapitalizationUndo()
        {
            //CapitalizationChangesList lastChange = CapitalizationChangesLists.Last();

            //foreach (FileModel value in lastChange.New)
            //{
            //    int index = lastChange.New.IndexOf(value);

            //    File.Move(value.FilePath, lastChange.Old[index].FilePath);
            //    await UpdateFileAsync(Container.Files[index], lastChange.Old[index].FileNameWithoutExtension);
            //}



        }

        #endregion


        #endregion


        public async Task FixExtensions()
        {
            foreach (FileModel file in Container.Files)
            {
                if (file.IsChecked)
                {
                    string newExtension = file.Extension.ToLower();

                    if (newExtension.Equals(".jpeg") || newExtension.Equals(".jpe"))
                    {
                        newExtension = ".jpg";
                    }

                    if (newExtension.Equals(".dip"))
                    {
                        newExtension = ".bmp";
                    }

                    (bool, string) result = EditFileSevice.FileMove(file.FilePath, file.DirectoryName + file.FileNameWithoutExtension + newExtension);

                    if (result.Item1)
                    {
                        await UpdateFileAsync(file, Path.GetFileNameWithoutExtension(result.Item2));
                    }
                }
            }
        }

    }
}