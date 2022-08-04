// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using Wpf.Ui.Common.Interfaces;
using FileManager.ViewModels;

namespace FileManager.Views.Pages;

/// <summary>
/// Interaction logic for VideoPage.xaml
/// </summary>
public partial class VideoPage : INavigableView<VideoPageViewModel>
{
    public VideoPage(VideoPageViewModel viewModel)
    {
        ViewModel = viewModel;
        Loaded += OnLoaded;

        InitializeComponent();
    }

    public VideoPageViewModel ViewModel
    {
        get;
    }
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        RootPanel.ScrollOwner = ScrollHost;
    }
}