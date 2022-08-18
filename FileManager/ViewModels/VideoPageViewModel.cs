using Microsoft.Toolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common.Interfaces;

namespace FileManager.ViewModels
{
    public class VideoPageViewModel : ObservableObject, INavigationAware
    {
        private bool _dataInitialized;

        public VideoPageViewModel(ContainerViewModel container)
        {
            Container = container;
        }

        public ContainerViewModel Container { get; set; }

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
    }
}