using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;
using FileManager.Models.Data;

namespace FileManager.ViewModels;

public class GeneralPageViewModel : ObservableObject, INavigationAware
{

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





    private bool _dataInitialized = false;

    private IEnumerable<string> _listBoxItemCollection = Array.Empty<string>();

    public IEnumerable<string> ListBoxItemCollection
    {
        get => _listBoxItemCollection;
        set => SetProperty(ref _listBoxItemCollection, value);
    }

    private IEnumerable<Item> _dataGridItemCollection = Array.Empty<Item>();

    public IEnumerable<Item> DataGridItemCollection
    {
        get => _dataGridItemCollection;
        set => SetProperty(ref _dataGridItemCollection, value);
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
        ListBoxItemCollection = new List<string>()
        {
            "Item 1",
            "Item 2",
            "Item 3",
            "Item 4",
            "Item 5",
            "Item 6",
            "Item 7"
        };

        DataGridItemCollection = new List<Item>()
        {
            new()
            {
                Name = "John",
                Path = "Doe",
            },
            new()
            {
                Name = "Chloe",
                Path = "Clarkson",
            },
            new()
            {
                Name = "Eric",
                Path = "Brown",
            },
            new()
            {
                Name = "John",
                Path = "Doe",
            },
            new()
            {
                Name = "Chloe",
                Path = "Clarkson",
            },
            new()
            {
                Name = "Eric",
                Path = "Brown",
            }
        };

        _dataInitialized = true;
    }
}