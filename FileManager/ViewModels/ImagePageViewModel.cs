using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;
using FileManager.Models.Data;
using System.Collections.ObjectModel;
using FileManager.Models;

namespace FileManager.ViewModels;

public class ImagePageViewModel : ObservableObject, INavigationAware
{
    ObservableCollection<FileData> Files { get; set; }

    public ImagePageViewModel(FileModel fileModel)
    {
        Files = new ObservableCollection<FileData>();
    }

    private bool _dataInitialized = false;

    public void OnNavigatedFrom()
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
}