// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

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

namespace FileManager.ViewModels;

public class ContainerViewModel : ObservableObject
{
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

    private ICommand _openFolderDialog;
    public ICommand OpenFolderDialog => _openFolderDialog ??= new RelayCommand<string>(FolderDialog);

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





    public ObservableCollection<FileModel> Files = new();

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

    private string searchInput = "";

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
                foreach (FileModel file in Files)
                {
                    file.IsChecked = true;
                }
            }
            else
            {
                foreach (FileModel file in Files)
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
                foreach (FileModel file in Files.Where((_, i) => i % 2 == 0))
                {
                    file.IsChecked = false;
                }
            }
            else
            {
                foreach (FileModel file in Files.Where((_, i) => i % 2 == 0))
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
                foreach (FileModel file in Files.Where((_, i) => i % 2 == 1))
                {
                    file.IsChecked = false;
                }
            }
            else
            {
                foreach (FileModel file in Files.Where((_, i) => i % 2 == 1))
                {
                    file.IsChecked = true;
                }
            }
            selectEverySecondLast = value;
            OnPropertyChanged(nameof(SelectEverySecondLast));
        }
    }



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
        //Clear previous search
        ClearSearch();

        await Task.Run(new Action(LoadFiles)).ConfigureAwait(false);
    }

    private void LoadFiles()
    {
        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(async () =>
            {
                SearchOption searchOption = SearchOptionsSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

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
            }));

    }


    public async Task AddFileAsync(string filePath)
    {
        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
        {
            FileModel file = new()
            {
                FileName = Path.GetFileName(filePath),
                FileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath),
                Extension = Path.GetExtension(filePath),
                DirectoryName = Path.GetDirectoryName(filePath),
                FilePath = filePath,
                FileNameWithSubdirectory = Path.GetFileName(filePath),
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
}