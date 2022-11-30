using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common.Interfaces;

namespace FileManager.ViewModels
{
    public class VideoPageViewModel : ObservableObject, INavigationAware
    {
        #region Main

        private bool _dataInitialized;

        public ContainerViewModel Container { get; set; }

        #endregion

        //____________________________________________________________

        #region Main

        public VideoPageViewModel(ContainerViewModel container)
        {
            Container = container;
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

        #endregion

    }
}