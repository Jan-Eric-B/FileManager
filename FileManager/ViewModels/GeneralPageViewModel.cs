using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;
using FileManager.Models.Data;
using FileManager.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using FileManager.Services;

namespace FileManager.ViewModels;

public class GeneralPageViewModel : ObservableObject
{
    public ContainerViewModel Container { get; set; }

    public GeneralPageViewModel(ContainerViewModel container)
    {
        Container = container;

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



    public void MoveToMainPath()
    {
        foreach (FileData file in Container.Files)
        {
            if (file.IsChecked && file.FileNameWithSubdirectory.Length != file.FilePath.Length)
            {
                File.Move(file.FilePath, CheckExistenceService.RenameIfExists(Container.MainPath + file.FileName));
            }
        }

    }

    #endregion

}