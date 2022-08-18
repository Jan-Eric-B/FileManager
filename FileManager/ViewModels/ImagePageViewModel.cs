using Microsoft.Toolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common.Interfaces;

namespace FileManager.ViewModels
{
    public class ImagePageViewModel : ObservableObject, INavigationAware
    {
        private bool _dataInitialized;

        public ImagePageViewModel(ContainerViewModel container)
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
            {
                InitializeData();
            }
        }

        private void InitializeData()
        {
            _dataInitialized = true;
        }
    }
}