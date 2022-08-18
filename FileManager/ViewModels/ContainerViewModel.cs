using FileManager.Models.Data;
using FileManager.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

        private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

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

        public ObservableCollection<FileData> Files { get; set; }

        #region Selection

        private bool selectAll = false;
        public bool SelectAll
        {
            get { return selectAll; }
            set
            {
                selectAll = value;

                if (selectAll)
                {
                    foreach (FileData file in Files)
                    {
                        file.IsChecked = true;
                    }
                }
                else
                {
                    foreach (FileData file in Files)
                    {
                        file.IsChecked = false;
                    }
                }

                OnPropertyChanged(nameof(SelectAll));
            }
        }

        private bool selectEverySecondFirst = false;
        public bool SelectEverySecondFirst
        {
            get { return selectEverySecondFirst; }
            set
            {
                if (selectEverySecondFirst)
                {
                    foreach (FileData file in Files.Where((_, i) => i % 2 == 0))
                    {
                        file.IsChecked = false;
                    }
                }
                else
                {
                    foreach (FileData file in Files.Where((_, i) => i % 2 == 0))
                    {
                        file.IsChecked = true;
                    }
                }

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
                if (selectEverySecondLast)
                {
                    foreach (FileData file in Files.Where((_, i) => i % 2 == 1))
                    {
                        file.IsChecked = false;
                    }
                }
                else
                {
                    foreach (FileData file in Files.Where((_, i) => i % 2 == 1))
                    {
                        file.IsChecked = true;
                    }
                }
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
            Files = new ObservableCollection<FileData>();
        }

        private async void FolderDialog(string parameter)
        {
            VistaFolderBrowserDialog dialog = new();

            if (CheckExistenceService.Check(MainPath))
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
                await SearchingAsync();
            }
        }

        private async void ScopeOut(string parameter)
        {
            DirectoryInfo directoryInfoMainPath = System.IO.Directory.GetParent(MainPath);

            if (directoryInfoMainPath != null)
            {
                DirectoryInfo directoryInfoMainPathParent = directoryInfoMainPath.Parent;

                if (CheckExistenceService.Check(directoryInfoMainPathParent.FullName))
                {
                    MainPath = directoryInfoMainPathParent.FullName + Path.DirectorySeparatorChar;
                    await SearchingAsync();
                }
            }
        }

        #endregion Main

        #region Search

        private void ClearSearch()
        {
            Files.Clear();
            FileCount = 0;
            FileCountSelected = 0;

            SelectAll = false;
            SelectEverySecondFirst = false;
            SelectEverySecondLast = false;
        }

        public async Task SearchingAsync()
        {
            ClearSearch();
            await Task.Run(new Action(LoadFiles)).ConfigureAwait(false);
        }

        private async void LoadFiles()
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(async () =>
            {
                SearchOption searchOption = SearchOptionsSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                if (CheckExistenceService.Check(MainPath))
                {
                    foreach (string filePath in Directory.EnumerateFiles(MainPath, "*.*", searchOption))
                    {
                        if (string.IsNullOrWhiteSpace(SearchInput))
                        {
                            await AddFileAsync(filePath);
                            FileCount++;
                        }
                        else
                        {
                            //Get Filename
                            string fileName = System.IO.Path.GetFileName(filePath);

                            //If Case Sensitive
                            if (!SearchOptionsCaseSensitive)
                            {
                                searchInput = searchInput.ToUpper();
                                fileName = fileName.ToUpper();
                            }

                            //Search in Name
                            if (SearchOptionsFileName && fileName.Contains(searchInput))
                            {
                                await AddFileAsync(filePath);
                                FileCount++;
                            }

                            //Search in File
                            if (SearchOptionsFileContent)
                            {
                                string content = File.ReadAllText(filePath);
                                //If Case Sensitive
                                if (!SearchOptionsCaseSensitive) content = content.ToUpper();

                                if (content.Contains(searchInput))
                                {
                                    await AddFileAsync(filePath);
                                    FileCount++;
                                }
                            }
                        }
                    }
                }
            }));
        }

        public async Task AddFileAsync(string filePath)
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                FileData file = new()
                {
                    FileName = Path.GetFileName(filePath),
                    FileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath),
                    Extension = Path.GetExtension(filePath),
                    DirectoryName = Path.GetDirectoryName(filePath),
                    FilePath = filePath,
                    FileNameWithSubdirectory = Path.GetFileName(filePath),
                    FileSize = SizeSuffix(new FileInfo(filePath).Length, 1),
                    IsChecked = false
                };
                //if file is in subdirectory, save subdirectories to filename
                if (filePath.Length > MainPath.Length + file.FileName.Length)
                {
                    file.FileNameWithSubdirectory = filePath[MainPath.Length..];
                }

                Files.Add(file);
            }));
        }

        private static string SizeSuffix(long value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException(nameof(decimalPlaces)); }
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag)
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag++;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        #endregion Search
    }
}