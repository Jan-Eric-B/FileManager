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

        //Info Button Count up
        private void CountUpExplanation_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxService.MessageBoxOK("Counting-Up description", "If containing a '0' it will be replaced with the counted up number:\r\n(1,2,3,4,...17,18,...)\r\nIf containing a '00' it will be replaced with leading zeros depending on count of selected items:\r\n(01,02,03,04,.../001,002,003,004,...)", 0, 600);
        }

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

        #region Rename

        // Remove Highlight
        private void TextBlock_RemoveHighlight(object sender, RoutedEventArgs e)
        {
            foreach (FileModel file in ViewModel.Container.Files)
            {
                file.HighlightedText = "";
            }
        }

        #region Replace

        private void BtnRenameReplace_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RenameReplace();
        }

        private void BtnRenameReplaceByIndexExplanation_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxService.MessageBoxOK("Replace by Index", "First input-field is the start position and the second one is the length.\r\nVisualization can be buggy. If multiple are visible, only the first one will be replaced.", 160, 520);
        }

        #region Highlight

        // Add Highlight (String)
        private void TextBlockRenameReplaceInputString_AddHighlight(object sender, RoutedEventArgs e)
        {
            foreach (FileModel file in ViewModel.Container.Files)
            {
                if (file.IsChecked)
                {
                    file.HighlightedText = txtRenameReplaceInputString.Text;
                }
            }
        }

        // Add Highlight (Index)
        private void NumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (FileModel file in ViewModel.Container.Files)
            {
                if (file.IsChecked)
                {
                    int fileNameLenght = file.FileNameWithoutExtension.Length;

                    int start = int.Parse(nbRenameReplaceInputStartInt.Text) - 1;
                    int lenght = int.Parse(nbRenameReplaceInputLengthInt.Text);

                    if (start + lenght > fileNameLenght)
                    {
                        if (start > fileNameLenght)
                        {
                            file.HighlightedText = "";
                        }
                        else
                        {
                            file.HighlightedText = file.FileName[start..fileNameLenght];
                        }
                    }
                    else
                    {
                        file.HighlightedText = file.FileName.Substring(start, lenght);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Swap

        private void BtnRenameSwap_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RenameSwap();
        }

        #region Highlight

        private void TextBlockRenameSwapInputString_AddHighlight(object sender, RoutedEventArgs e)
        {
            foreach (FileModel file in ViewModel.Container.Files)
            {
                if (file.IsChecked)
                {
                    file.HighlightedText = txtRenameSwapInputString.Text;
                }
            }
        }

        #endregion

        #endregion

        #region Insert

        private void BtnRenameInsertFront_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RenameInsertFront();
        }

        private void BtnRenameInsertBack_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RenameInsertBack();
        }

        #endregion

        #region Capitalzation

        private async void BtnRenameCapitalize_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.ToCapitalization();
        }

        private async void BtnRenameLowerCase_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.ToLowerCase();
        }

        private async void BtnRenameUpperCase_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.ToUpperCase();
        }

        private async void BtnRenameCapitalizationUndo(object sender, RoutedEventArgs e)
        {
            await ViewModel.CapitalizationUndo();
        }

        #endregion

        #endregion


    }
}