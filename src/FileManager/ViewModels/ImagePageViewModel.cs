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
    public class ImagePageViewModel : ObservableObject, INavigationAware
    {
        #region Move

        private string moveDirectoryName = "scene ";
        public string MoveDirectoryName
        {
            get => moveDirectoryName;
            set
            {
                moveDirectoryName = value;
                OnPropertyChanged(nameof(MoveDirectoryName));
            }
        }

        private double moveToSimilarDifferenceTolerance = 0.95;
        public double MoveToSimilarDifferenceTolerance
        {
            get { return moveToSimilarDifferenceTolerance; }
            set
            {
                moveToSimilarDifferenceTolerance = value;
                OnPropertyChanged(nameof(MoveToSimilarDifferenceTolerance));
            }
        }


        #endregion

        private readonly ISnackbarService _snackbarService;

        #region Main

        private bool _dataInitialized;

        public ContainerViewModel Container { get; set; }

        #endregion

        //____________________________________________________________

        #region Main

        public ImagePageViewModel(ContainerViewModel container, ISnackbarService snackbarService)
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

        #region Move

        public async void MoveToSimilar()
        {
            List<string> images = new List<string> { ".png", ".jpg", ".jpeg", ".jpe", ".bmp", ".dip", ".gif" };


            ObservableCollection<FileModel> files = new ObservableCollection<FileModel>();

            foreach (FileModel file in Container.Files)
            {
                if(file.IsChecked && EditFileSevice.Check(file.FilePath) && images.Contains(file.Extension.ToLower()))
                {
                    files.Add(file);
                }
            }

            ImageCompareService.Compare(files, MoveToSimilarDifferenceTolerance, Container.MainPath, MoveDirectoryName);
        }

        #endregion


    }
}