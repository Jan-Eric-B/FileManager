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
        _dialogControl.ButtonRightClick += DialogOnButtonClickRightSide;
    }
    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
    }

    // Open Dialog
    private async void OpenDialog(string title, string text)
    {
        await _dialogControl.ShowAndWaitAsync(title,text);
    }
    // Dialog Right Click
    private static void DialogOnButtonClickRightSide(object sender, RoutedEventArgs e)
    {
        var dialogControl = (IDialogControl)sender;
        dialogControl.Hide();
    }


    #region Move
    private void btnMoveToMainPath_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.MoveToMainPath();
    }

    //Info Button
    private void MoveCountUpExplanation_OnClick(object sender, RoutedEventArgs e)
    {
        OpenDialog("Count up", "If name of new subdirectories contains a '0', it counts up (1,2,3,4,..9).\r\nIf it contains '00', it counts up and depending on the amount of selected items, it will count up like this 01,02,03,04,... 001,002,003,004,...");
    }



    #endregion

    private void btnMoveToSamePath_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(ViewModel.MoveDirectoryName))
        {
            ViewModel.MoveToSamePath();
        }
        else
        {
            OpenDialog("Empty Textbox", "Please fill out the textbox");
        }
    }

    private void btnMoveToSinglePath_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.MoveDirectoryNameCountUp && string.IsNullOrWhiteSpace(ViewModel.MoveDirectoryName))
        {
            OpenDialog("Empty Textbox", "Please fill out the textbox");
        }
        else
        {
            ViewModel.MoveToSinglePath();
        }
    }
}