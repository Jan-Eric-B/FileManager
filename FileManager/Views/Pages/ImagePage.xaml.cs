using FileManager.Resources.Settings;
using FileManager.ViewModels;
using System.Windows;
using Wpf.Ui.Common.Interfaces;

namespace FileManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for ImagePage.xaml
    /// </summary>
    public partial class ImagePage : INavigableView<ImagePageViewModel>
    {
        //____________________________________________________________

        #region Main

        public ImagePage(ImagePageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            Loaded += OnLoaded;

            InitializeComponent();
        }

        public ImagePageViewModel ViewModel
        {
            get;
        }

        //Maybe
        private void CardExpander_SaveSettings(object sender, RoutedEventArgs e)
        {
            SettingsSave();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SettingsLoad();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            SettingsSave();
        }

        #region Settings

        private void SettingsLoad()
        {
            GeneralPageSettings settings = new();
            //ViewModel.CardExpanderMove = settings.CardExpanderMove;
            //ViewModel.CardExpanderDelete = settings.CardExpanderDelete;
            //ViewModel.CardExpanderRename = settings.CardExpanderRename;
        }

        private void SettingsSave()
        {
            GeneralPageSettings settings = new()
            {
                //CardExpanderMove = ViewModel.CardExpanderMove,
                //CardExpanderDelete = ViewModel.CardExpanderDelete,
                //CardExpanderRename = ViewModel.CardExpanderRename,
            };
            settings.Save();
        }

        #endregion

        #endregion

    }
}