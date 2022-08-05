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
        //RootPanel.ScrollOwner = ScrollHost;
        _dialogControl.ButtonRightClick += DialogOnButtonClickRightSide;
    }
    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
    }



    private void MoveCountUpExplanation_OnClick(object sender, RoutedEventArgs e)
    {
        OpenDialog("The Title", "The Text");
    }

    private async void OpenDialog(string title, string text)
    {
        await _dialogControl.ShowAndWaitAsync(title,text);
    }
    private static void DialogOnButtonClickRightSide(object sender, RoutedEventArgs e)
    {
        var dialogControl = (IDialogControl)sender;
        dialogControl.Hide();
    }


}