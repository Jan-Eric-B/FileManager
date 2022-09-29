using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileManager.Models;
using FileManager.Services;
using FileManager.Views;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace FileManager.ViewModels
{
    public class ContainerViewModel : ObservableObject
    {
        #region ICommands

        private ICommand _directoryScopeOut;
        private ICommand _openFolderDialog;
        public ICommand DirectoryScopeOut => _directoryScopeOut ??= new RelayCommand<string>(ScopeOut);
        public ICommand OpenFolderDialog => _openFolderDialog ??= new RelayCommand<string>(FolderDialog);

        #endregion ICommands

        #region Search

        private CancellationTokenSource _tokenSource = new();

        private string mainPath;
        public string MainPath
        {
            get => mainPath;
            set
            {
                mainPath = value;
                OnPropertyChanged(nameof(MainPath));
            }
        }

        private string searchInput = string.Empty;
        public string SearchInput
        {
            get { return searchInput; }
            set
            {
                searchInput = value;
                OnPropertyChanged(nameof(SearchInput));
            }
        }

        #region Options

        private bool searchOptionsCaseSensitive = false;
        public bool SearchOptionsCaseSensitive
        {
            get { return searchOptionsCaseSensitive; }
            set
            {
                searchOptionsCaseSensitive = value;
                OnPropertyChanged(nameof(SearchOptionsCaseSensitive));
            }
        }

        private bool searchOptionsFileContent = false;
        public bool SearchOptionsFileContent
        {
            get { return searchOptionsFileContent; }
            set
            {
                searchOptionsFileContent = value;
                OnPropertyChanged(nameof(SearchOptionsFileContent));
            }
        }

        private bool searchOptionsFileName = false;
        public bool SearchOptionsFileName
        {
            get { return searchOptionsFileName; }
            set
            {
                searchOptionsFileName = value;
                OnPropertyChanged(nameof(SearchOptionsFileName));
            }
        }

        private bool searchOptionsSubdirectories = false;
        public bool SearchOptionsSubdirectories
        {
            get { return searchOptionsSubdirectories; }
            set
            {
                searchOptionsSubdirectories = value;
                OnPropertyChanged(nameof(SearchOptionsSubdirectories));
            }
        }

        #endregion Options

        #endregion Search

        #region ItemList

        public ObservableCollection<FileModel> Files { get; set; }

        public ObservableCollection<string> FileExtensions { get; set; }


        private int selectedFileExtension = 0;
        public int SelectedFileExtension
        {
            get => selectedFileExtension;
            set
            {
                selectedFileExtension = value;
                OnPropertyChanged(nameof(SelectedFileExtension));
            }
        }

        private string highlightedText = "Apple";
        public string HighlightedText
        {
            get => highlightedText;
            set
            {
                highlightedText = value;
                OnPropertyChanged(nameof(HighlightedText));
            }
        }

        #region Selection

        private bool selectAll = false;
        public bool SelectAll
        {
            get { return selectAll; }
            set
            {
                //if (selectAll)
                //{
                //    foreach (FileModel file in Files)
                //    {
                //        file.IsChecked = true;
                //    }
                //}
                //else
                //{
                //    foreach (FileModel file in Files)
                //    {
                //        file.IsChecked = false;
                //    }
                //}

                selectAll = value;
                OnPropertyChanged(nameof(SelectAll));
            }
        }

        private bool selectEverySecondFirst = false;
        public bool SelectEverySecondFirst
        {
            get { return selectEverySecondFirst; }
            set
            {
                selectEverySecondFirst = value;
                OnPropertyChanged(nameof(SelectEverySecondFirst));
            }
        }

        private bool selectEverySecondLast = false;
        public bool SelectEverySecondLast
        {
            get { return selectEverySecondLast; }
            set
            {
                selectEverySecondLast = value;
                OnPropertyChanged(nameof(SelectEverySecondLast));
            }
        }

        #endregion Selection

        #region ItemCount

        private int fileCount;
        public int FileCount
        {
            get { return fileCount; }
            set
            {
                fileCount = value;
                OnPropertyChanged(nameof(FileCount));
            }
        }

        private int fileCountSelected;
        public int FileCountSelected
        {
            get { return fileCountSelected; }
            set
            {
                fileCountSelected = value;
                OnPropertyChanged(nameof(FileCountSelected));
            }
        }

        #endregion ItemCount

        #endregion ItemList

        //____________________________________________________________

        #region Main

        public ContainerViewModel()
        {
            Files = new ObservableCollection<FileModel>();
            FileExtensions = new ObservableCollection<string>();
        }

        private async void FolderDialog(string parameter)
        {
            VistaFolderBrowserDialog dialog = new();

            if (EditFileSevice.Check(MainPath))
            {
                dialog.SelectedPath = MainPath;
            }
            else
            {
                dialog.SelectedPath = "c:\\";
            }

            dialog.Description = "Choose the main directory";
            dialog.UseDescriptionForTitle = true;
            dialog.ShowNewFolderButton = true;

            if (dialog.ShowDialog() == true)
            {
                MainPath = dialog.SelectedPath + Path.DirectorySeparatorChar;
                await SearchAsync();
            }
        }

        private async void ScopeOut(string parameter)
        {
            DirectoryInfo directoryInfoMainPath = System.IO.Directory.GetParent(MainPath);

            if (directoryInfoMainPath != null)
            {
                DirectoryInfo directoryInfoMainPathParent = directoryInfoMainPath.Parent;

                if (EditFileSevice.Check(directoryInfoMainPathParent.FullName))
                {
                    MainPath = directoryInfoMainPathParent.FullName + Path.DirectorySeparatorChar;
                    await SearchAsync();
                }
            }
        }

        #endregion Main

        #region Search

        // Start of Searching
        public async Task SearchAsync()
        {
            Files.Clear();
            FileExtensions.Clear();
            FileExtensions.Add(".*");
            SelectedFileExtension = 0;
            FileCount = 0;
            FileCountSelected = 0;

            SelectAll = false;
            SelectEverySecondFirst = false;
            SelectEverySecondLast = false;

            CancellationTokenSource newTokenSource = new();

            CancellationTokenSource oldTokenSource = Interlocked.Exchange(ref _tokenSource, newTokenSource);

            if (!oldTokenSource.IsCancellationRequested)
            {
                oldTokenSource.Cancel();
            }

            try
            {
                //Starts Task
                await Task.Run(() => FindFiles(newTokenSource.Token));
            }
            catch (OperationCanceledException)
            {
                //When Canceled
            }
        }

        // Find file
        private async Task FindFiles(CancellationToken token)
        {
            SearchOption searchOption = SearchOptionsSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            if (EditFileSevice.Check(MainPath))
            {
                foreach (string filePath in Directory.EnumerateFiles(MainPath, "*.*", searchOption))
                {
                    await AddFileAsync(filePath);
                    await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => FileCount++));

                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                }
            }
        }

        // Add file to ObservableCollection
        private async Task AddFileAsync(string filePath)
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                FileModel file = new()
                {
                    FileName = Path.GetFileName(filePath),
                    FileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath),
                    Extension = Path.GetExtension(filePath),
                    DirectoryName = Path.GetDirectoryName(filePath) + Path.DirectorySeparatorChar,
                    FilePath = filePath,
                    FileNameWithSubdirectory = Path.GetFileName(filePath),
                    SubdirectoryPath = "",
                    FileSize = FileSizeConverterService.SizeSuffix(new FileInfo(filePath).Length, 1),
                    IsChecked = false
                };

                //if file is in subdirectory, save subdirectories to filename
                if (filePath.Length > MainPath.Length + file.FileName.Length)
                {
                    file.FileNameWithSubdirectory = filePath[MainPath.Length..];
                    file.SubdirectoryPath = file.DirectoryName[MainPath.Length..];
                }

                Files.Add(file);

                if (!FileExtensions.Contains(file.Extension.ToLower()))
                {
                    FileExtensions.Add(file.Extension.ToLower());
                }

            }));
        }

        #endregion Search

    }
}