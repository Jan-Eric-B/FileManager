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
using Windows.Storage;
using Microsoft.VisualBasic.FileIO;

namespace FileManager.ViewModels;

public class GeneralPageViewModel : ObservableObject
{
    public ContainerViewModel Container { get; set; }

    public GeneralPageViewModel(ContainerViewModel container)
    {
        Container = container;
    }


    private bool _dataInitialized = false;

    private bool cardExpanderMove;
    private bool cardExpanderRename;
    private bool cardExpanderDelete;
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
        Container.SearchingAsync().Wait();
    }

    public void MoveToSamePath()
    {
        string newdir = Container.MainPath + MoveDirectoryName + Path.DirectorySeparatorChar;

        if (!Directory.Exists(newdir))
        {
            Directory.CreateDirectory(newdir);
        }

        foreach (FileData file in Container.Files) if (file.IsChecked)
        {
            File.Move(file.FilePath, CheckExistenceService.RenameIfExists(newdir + file.FileName));
        }
        Container.SearchingAsync().Wait();
    }

    public void MoveToSinglePath()
    {
        if (MoveDirectoryUseFileName)
        {
            foreach(FileData file in Container.Files) if (file.IsChecked)
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
        else
        {
            int count = 0;

            foreach (FileData file in Container.Files) if (file.IsChecked)
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
        Container.SearchingAsync().Wait();
    }

    private string NameCount(int count)
    {
        if (MoveDirectoryName.Contains("0"))
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

    #endregion

    #region Delete

    public void DeleteItem()
    {
        foreach (FileData file in Container.Files) if (file.IsChecked)
        {
            FileSystem.DeleteFile(file.FilePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
        }
        Container.SearchingAsync().Wait();
    }

    public void DeleteItemPermanently()
    {
        foreach (FileData file in Container.Files) if (file.IsChecked)
        {
            FileSystem.DeleteFile(file.FilePath, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently); 
        }
        Container.SearchingAsync().Wait();
    }

    #endregion

}