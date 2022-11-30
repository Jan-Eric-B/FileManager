using FileManager.Resources.Settings;
using FileManager.ViewModels;
using System.Windows;
using Wpf.Ui.Common.Interfaces;

namespace FileManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for VideoPage.xaml
    /// </summary>
    public partial class VideoPage : INavigableView<VideoPageViewModel>
    {
        //____________________________________________________________

        #region Main

        public VideoPage(VideoPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            Loaded += OnLoaded;

            InitializeComponent();
        }

        public VideoPageViewModel ViewModel
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