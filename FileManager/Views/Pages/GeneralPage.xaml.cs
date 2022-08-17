// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using FileManager.ViewModels;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Mvvm.Services;
using FileManager.Resources;
using FileManager.Services;
using System;
using Wpf.Ui.Appearance;
using Microsoft.VisualBasic.FileIO;

namespace FileManager.Views.Pages;

/// <summary>
/// Interaction logic for GeneralPage.xaml
/// </summary>
public partial class GeneralPage : INavigableView<GeneralPageViewModel>
{
    private readonly IDialogControl _dialogControl;

    public GeneralPage(GeneralPageViewModel viewModel, IDialogService dialogService)
    {
        ViewModel = viewModel;
        this.DataContext = ViewModel;

        Loaded += OnLoaded;

        InitializeComponent();

        _dialogControl = dialogService.GetDialogControl();
    }
    public GeneralPageViewModel ViewModel
    {
        get;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        SettingsLoad();
    }
    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        SettingsSave();
    }

    // Open Dialog
    private async void OpenDialog(string title, string text)
    {
        await _dialogControl.ShowAndWaitAsync(title,text);
    }
    // Dialog Right Click Close
    private static void DialogOnButtonClickRightSideClose(object sender, RoutedEventArgs e)
    {
        var dialogControl = (IDialogControl)sender;
        dialogControl.Hide();

    }
    // Dialog Right Click No
    private static void DialogOnButtonClickRightSideNo(object sender, RoutedEventArgs e)
    {
        var dialogControl = (IDialogControl)sender;
        dialogControl.Hide();
    }
    // Dialog Left Click Yes
    private static void DialogOnButtonClickYesSideYes(object sender, RoutedEventArgs e)
    {
        var dialogControl = (IDialogControl)sender;
        dialogControl.Hide();
    }

    #region Move

    //MainDirectory
    private void btnMoveToMainPath_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.MoveToMainPath();
    }

    //Same Subdirectory
    private void btnMoveToSamePath_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(ViewModel.MoveDirectoryName))
        {
            ViewModel.MoveToSamePath();
        }
        else
        {
            _dialogControl.ButtonRightClick += DialogOnButtonClickRightSideClose;
            OpenDialog("Empty Textbox", "Please fill out the textbox");
        }
    }

    //Single Subdirectory
    private void btnMoveToSinglePath_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.MoveDirectoryNameCountUp && string.IsNullOrWhiteSpace(ViewModel.MoveDirectoryName))
        {
            _dialogControl.ButtonRightClick += DialogOnButtonClickRightSideClose;
            OpenDialog("Empty Textbox", "Please fill out the textbox");
        }
        else
        {
            ViewModel.MoveToSinglePath();
        }
    }
    //Info Button
    private void MoveCountUpExplanation_OnClick(object sender, RoutedEventArgs e)
    {
        _dialogControl.ButtonRightClick += DialogOnButtonClickRightSideClose;
        OpenDialog("Count up", "If name of new subdirectories contains a '0', it counts up (1,2,3,4,..9).\r\nIf it contains '00', it counts up and depending on the amount of selected items, it will count up like this 01,02,03,04,... 001,002,003,004,...");
    }

    //Highlight TextBox when using Buttons who depend on it
    private void btnHighlightRequiredTextBox_MouseEnter(object sender, MouseEventArgs e)
    {
        Button btn = sender as Button;
        if (btn.Name == "btnMoveToSinglePath" && ViewModel.MoveDirectoryNameCountUp)
        {
            txSubdirectoryName.Focus();
        }
        if (btn.Name == "btnMoveToSamePath")
        {
            txSubdirectoryName.Focus();
        }
    }
    private void btnHighlightRequiredTextBox_MouseLeave(object sender, MouseEventArgs e)
    {
        Button btn = sender as Button;
        btn.Focus();
    }


    #endregion

    private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBoxService.MessageBoxYesNo("Deletion", "Do you really want to delete these files?") == true)
        {
            ViewModel.DeleteItem();
        }
    }

    private void btnDeleteItemPermanently_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBoxService.MessageBoxYesNo("Deletion", "Do you really want to delete these files permanently?") == true)
        {
            ViewModel.DeleteItemPermanently();
        }
    }




    //Save
    private void SettingsSave()
    {
        GeneralPageSettings settings = new()
        {
            CardExpanderMove = ViewModel.CardExpanderMove,
            CardExpanderDelete = ViewModel.CardExpanderDelete,
            CardExpanderRename = ViewModel.CardExpanderRename,
        };
        settings.Save();
    }

    //Load
    private void SettingsLoad()
    {
        GeneralPageSettings settings = new();
        ViewModel.CardExpanderMove = settings.CardExpanderMove;
        ViewModel.CardExpanderDelete = settings.CardExpanderDelete;
        ViewModel.CardExpanderRename = settings.CardExpanderRename;
    }

    private void CardExpander_SaveSettings(object sender, RoutedEventArgs e)
    {
        SettingsSave();
    }
}