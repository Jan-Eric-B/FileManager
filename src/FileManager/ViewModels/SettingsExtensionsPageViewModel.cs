using CommunityToolkit.Mvvm.ComponentModel;
using FileManager.Models;
using FileManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Composition.Scenes;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace FileManager.ViewModels
{
    public class SettingsExtensionsPageViewModel : ObservableObject, INavigationAware
    {
      

        private readonly ISnackbarService _snackbarService;

        #region Main

        private bool _dataInitialized;

        public ContainerViewModel Container { get; set; }

        #endregion

        //____________________________________________________________

        #region Main

        public SettingsExtensionsPageViewModel(ContainerViewModel container, ISnackbarService snackbarService)
        {
            Container = container;
            _snackbarService = snackbarService;
        }

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

        #endregion




    }
}