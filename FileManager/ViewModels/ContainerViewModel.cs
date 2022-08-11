using FileManager.Models;
using FileManager.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;
using System.IO;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Threading;
using FileManager.Models.Data;
using System.Collections.Specialized;
using FileManager.Views.Pages;

namespace FileManager.ViewModels;

public class ContainerViewModel : ObservableObject
{
    private ICommand _openFolderDialog;
    private ICommand _directoryScopeOut;

    public ICommand OpenFolderDialog => _openFolderDialog ??= new RelayCommand<string>(FolderDialog);
    public ICommand DirectoryScopeOut => _directoryScopeOut ??= new RelayCommand<string>(ScopeOut);

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

    #region Search Options
    private bool searchOptionsCaseSensitive = false;
    private bool searchOptionsFileContent = false;
    private bool searchOptionsFileName = false;
    private bool searchOptionsSubdirectories = false;
    public bool SearchOptionsCaseSensitive
    {
        get { return searchOptionsCaseSensitive; }
        set
        {
            searchOptionsCaseSensitive = value;
            OnPropertyChanged(nameof(SearchOptionsCaseSensitive));
        }
    }
    public bool SearchOptionsFileContent
    {
        get { return searchOptionsFileContent; }
        set
        {
            searchOptionsFileContent = value;
            OnPropertyChanged(nameof(SearchOptionsFileContent));
        }
    }
    public bool SearchOptionsFileName
    {
        get { return searchOptionsFileName; }
        set
        {
            searchOptionsFileName = value;
            OnPropertyChanged(nameof(SearchOptionsFileName));
        }
    }
    public bool SearchOptionsSubdirectories
    {
        get { return searchOptionsSubdirectories; }
        set
        {
            searchOptionsSubdirectories = value;
            OnPropertyChanged(nameof(SearchOptionsSubdirectories));
        }
    }
    #endregion Search Options

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

    private int fileCount;
    private int fileCountSelected;
    public int FileCount
    {
        get { return fileCount; }
        set
        {
            fileCount = value;
            OnPropertyChanged(nameof(FileCount));
        }
    }
    public int FileCountSelected
    {
        get { return fileCountSelected; }
        set
        {
            fileCountSelected = value;
            OnPropertyChanged(nameof(FileCountSelected));
        }
    }

    private bool selectAll = false;
    private bool selectEverySecondFirst = false;
    private bool selectEverySecondLast = false;
    public bool SelectAll
    {
        get { return selectAll; }
        set
        {
            selectAll = value;

            if (selectAll)
            {
                foreach (FileData file in FileModel.Files)
                {
                    file.IsChecked = true;
                }
            }
            else
            {
                foreach (FileData file in FileModel.Files)
                {
                    file.IsChecked = false;
                }
            }

            OnPropertyChanged(nameof(SelectAll));
        }
    }
    public bool SelectEverySecondFirst
    {
        get { return selectEverySecondFirst; }
        set
        {
            if (selectEverySecondFirst)
            {
                foreach (FileData file in FileModel.Files.Where((_, i) => i % 2 == 0))
                {
                    file.IsChecked = false;
                }
            }
            else
            {
                foreach (FileData file in FileModel.Files.Where((_, i) => i % 2 == 0))
                {
                    file.IsChecked = true;
                }
            }

            selectEverySecondFirst = value;
            OnPropertyChanged(nameof(SelectEverySecondFirst));
        }
    }
    public bool SelectEverySecondLast
    {
        get { return selectEverySecondLast; }
        set
        {
            if (selectEverySecondLast)
            {
                foreach (FileData file in FileModel.Files.Where((_, i) => i % 2 == 1))
                {
                    file.IsChecked = false;
                }
            }
            else
            {
                foreach (FileData file in FileModel.Files.Where((_, i) => i % 2 == 1))
                {
                    file.IsChecked = true;
                }
            }
            selectEverySecondLast = value;
            OnPropertyChanged(nameof(SelectEverySecondLast));
        }
    }

    //public ObservableCollection<FileModel> Files = new();

    public FileModel FileModel { get; set; }
    GeneralPageViewModel GeneralPageViewModel { get; set; }
    ImagePageViewModel ImagePageViewModel { get; set; }
    VideoPageViewModel VideoPageViewModel { get; set; }

    public ContainerViewModel()
    {
        FileModel = new FileModel();
        FileModel.Files.CollectionChanged += this.OnCollectionChanged;

        GeneralPageViewModel = new GeneralPageViewModel(FileModel);
        ImagePageViewModel = new ImagePageViewModel(FileModel);
        VideoPageViewModel = new VideoPageViewModel(FileModel);
    }

    void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        //Get the sender observable collection
        ObservableCollection<string> obsSender = sender as ObservableCollection<string>;


        //Get the action which raised the collection changed event
        NotifyCollectionChangedAction action = e.Action;
    }

    private void ClearSearch()
    {
        FileModel.Files.Clear();
        FileCount = 0;
        FileCountSelected = 0;

        SelectAll = false;
        SelectEverySecondFirst = false;
        SelectEverySecondLast = false;
    }

    public async Task SearchingAsync()
    {
        //Clear previous search
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

            FileModel.Files.Add(file);
        }));
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

    static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
    static string SizeSuffix(long value, int decimalPlaces = 1)
    {
        if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
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
}