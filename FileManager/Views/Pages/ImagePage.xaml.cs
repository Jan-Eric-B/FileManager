using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using FileManager.ViewModels;

namespace FileManager.Views.Pages;

/// <summary>
/// Interaction logic for ImagePage.xaml
/// </summary>
public partial class ImagePage : INavigableView<ImagePageViewModel>
{
    public ImagePage(ImagePageViewModel viewModel)
    {
        ViewModel = viewModel;
        Loaded += OnLoaded;

        InitializeComponent();
    }

    public ImagePageViewModel ViewModel
    {
        get;
    }
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        RootPanel.ScrollOwner = ScrollHost;
    }
}