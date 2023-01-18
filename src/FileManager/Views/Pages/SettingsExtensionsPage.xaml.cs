using FileManager.Services;
using FileManager.ViewModels;
using System.Diagnostics;
using System.Drawing;
using Wpf.Ui.Mvvm.Interfaces;
using System.Windows.Media;

namespace FileManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsExtensionsPage.xaml
    /// </summary>
    public partial class SettingsExtensionsPage
    {
        bool ExifToolInstalled = false;

        public SettingsExtensionsPage()
        {
            DataContext = ViewModel;

            InitializeComponent();


            if (ExtensionsService.CheckExistance("exiftool(-k).exe"))
            {
                btExifToolInstalled.Icon = Wpf.Ui.Common.SymbolRegular.CheckmarkCircle24;
                btExifToolInstalled.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                btExifToolInstalled.Icon = Wpf.Ui.Common.SymbolRegular.DismissCircle24;
                btExifToolInstalled.Foreground = System.Windows.Media.Brushes.Red;


            }

        }

        public SettingsExtensionsPageViewModel ViewModel
        {
            get;
        }

        private void btExifToolDownload_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            ExtensionsService.DownloadApplication("exiftool(-k).exe");

        }
        
        private void btFfmpegDownload_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            ExtensionsService.DownloadApplication("exiftool(-k).exe");

        }

        private void btOpenExtensionDirectory_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}