using FileManager.Models;
using FileManager.Resources.Settings;
using FileManager.Services;
using FileManager.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;

namespace FileManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for GeneralPage.xaml
    /// </summary>
    public partial class GeneralPage : INavigableView<GeneralPageViewModel>
    {
        //____________________________________________________________

        #region Main

        public GeneralPage(GeneralPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            Loaded += OnLoaded;

            InitializeComponent();
        }

        public GeneralPageViewModel ViewModel
        {
            get;
        }

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
            ViewModel.CardExpanderMove = settings.CardExpanderMove;
            ViewModel.CardExpanderDelete = settings.CardExpanderDelete;
            ViewModel.CardExpanderRename = settings.CardExpanderRename;
        }

        private void SettingsSave()
        {
            GeneralPageSettings settings = new()
            {
                CardExpanderMove = ViewModel.CardExpanderMove,
                CardExpanderDelete = ViewModel.CardExpanderDelete,
                CardExpanderRename = ViewModel.CardExpanderRename,
            };
            settings.Save();
        }

        #endregion

        #endregion Main

        #region Move

        //Highlight TextBox when using Buttons who depend on it
        private void BtnHighlightRequiredTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnMoveToSinglePath" && ViewModel.MoveDirectoryNameCountUp)
            {
                txSubdirectoryName.Focus();
            }
            if (btn.Name == "btnMoveToSamePath")
            {
                txSubdirectoryName.Focus();
            }
        }

        private void BtnHighlightRequiredTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.Focus();
        }

        //MainDirectory
        private void BtnMoveToMainPath_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.MoveToMainPath();
        }

        //Same Subdirectory
        private void BtnMoveToSamePath_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ViewModel.MoveDirectoryName))
            {
                ViewModel.MoveToSamePath();
            }
            else
            {
                MessageBoxService.MessageBoxOK("Empty Textbox", "Please fill out the textbox");
            }
        }

        //Single Subdirectory
        private void BtnMoveToSinglePath_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.MoveDirectoryNameCountUp && string.IsNullOrWhiteSpace(ViewModel.MoveDirectoryName))
            {
                MessageBoxService.MessageBoxOK("Empty Textbox", "Please fill out the textbox");
            }
            else
            {
                ViewModel.MoveToSinglePath();
            }
        }

        //Info Button
        private void BtnMoveCountUpExplanation_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxService.MessageBoxOK("Counting-Up description", "If containing a '0' it will be replaced with the counted up number\r\n(1,2,3,4,...17,18,...)\r\nIf containing a '00' it will be replaced with leading zeros depending on count of selected items\r\n(01,02,03,04,.../001,002,003,004,...)", 0,600);
        }

        #endregion Move

        #region Delete

        private void BtnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxService.MessageBoxYesNo("Deletion", "Do you really want to delete these files?", 150, 0) == true)
            {
                ViewModel.DeleteItem();
            }
        }

        private void BtnDeleteItemPermanently_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxService.MessageBoxYesNo("Deletion", "Do you really want to delete these files permanently?", 150, 0) == true)
            {
                ViewModel.DeleteItemPermanently();
            }
        }

        #endregion Delete

        private void BtnRenameReplace_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RenameReplace();
        }

        private void BtnRenameReplaceByIndexExplanation_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void TxtRenameReplaceInputString_KeyDown(object sender, KeyEventArgs e)
        {
            foreach(FileModel file in ViewModel.Container.Files)
            {
                file.HighlightedText = txtRenameReplaceInputString.Text;
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
        }

        private void txtRenameReplaceInputString_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (FileModel file in ViewModel.Container.Files)
            {
                file.HighlightedText = txtRenameReplaceInputString.Text;
            }
        }

        private void txtRenameReplaceInputString_LostFocus(object sender, RoutedEventArgs e)
        {
            foreach (FileModel file in ViewModel.Container.Files)
            {
                file.HighlightedText = "";
            }
        }

        private void txtRenameReplaceInputString_GotFocus(object sender, RoutedEventArgs e)
        {
            foreach (FileModel file in ViewModel.Container.Files)
            {
                file.HighlightedText = txtRenameReplaceInputString.Text;
            }
        }

        private void TextBlock_RemoveHighlight(object sender, RoutedEventArgs e)
        {
            foreach (FileModel file in ViewModel.Container.Files)
            {
                file.HighlightedText = "";
            }
        }


        // Highlight by Index
        private void NumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (FileModel file in ViewModel.Container.Files)
            {
                file.HighlightedText = file.FileName.Substring(int.Parse(nbRenameReplaceInputStartInt.Text) - 1, int.Parse(nbRenameReplaceInputLengthInt.Text));
            }
        }

        private void CardExpander_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ceMove_GotFocus(object sender, RoutedEventArgs e)
        {
            foreach (FileModel file in ViewModel.Container.Files)
            {
                file.HighlightedText = "";
            }
        }

        private void ceDelete_GotFocus(object sender, RoutedEventArgs e)
        {
            foreach (FileModel file in ViewModel.Container.Files)
            {
                file.HighlightedText = "";
            }
        }

        private void ceRename_GotFocus(object sender, RoutedEventArgs e)
        {
            foreach (FileModel file in ViewModel.Container.Files)
            {
                file.HighlightedText = "";
            }
        }
    }
}