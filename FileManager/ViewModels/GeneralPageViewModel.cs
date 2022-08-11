using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;
using FileManager.Models.Data;
using FileManager.Models;
using System.Collections.ObjectModel;

namespace FileManager.ViewModels;

public class GeneralPageViewModel : ObservableObject, INavigationAware
{

    ObservableCollection<FileData> Files { get; set; }

    public GeneralPageViewModel(FileModel fileModel)
    {
        Files = fileModel.Files;
    }


    private bool _dataInitialized = false;

    private bool cardExpanderMove = true;
    private bool cardExpanderRename = false;
    private bool cardExpanderDelete = false;
    public bool CardExpanderMove
    {
        get { return cardExpanderMove; }
        set
        {
            cardExpanderMove = value;
            OnPropertyChanged(nameof(CardExpanderMove));
        }
    }
    public bool CardExpanderRename
    {
        get { return cardExpanderRename; }
        set
        {
            cardExpanderRename = value;
            OnPropertyChanged(nameof(CardExpanderRename));
        }
    }
    public bool CardExpanderDelete
    {
        get { return cardExpanderDelete; }
        set
        {
            cardExpanderDelete = value;
            OnPropertyChanged(nameof(CardExpanderDelete));
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


    #region Move

    public bool MoveToMainPath()
    {
        foreach (FileData file in Files)
        {
            if (file.IsChecked)
            {

            }
        }
        return false;
    }

    #endregion

}