using System.Windows;

namespace FileManager.Services
{
    /// <summary>
    /// Handles all kind of MessageBoxes
    /// </summary>
    internal static class MessageBoxService
    {
        #region MessageBox OK

        /// <summary>
        /// MessageBox with only OK button
        /// </summary>
        /// <param name="height">Is optional. height = '0' sets it to default</param>
        /// <param name="width">Is optional. width = '0' sets it to default</param>
        public static void MessageBoxOK(string title, string text, int height = 0, int width = 0)
        {
            Wpf.Ui.Controls.MessageBox messageBox = new()
            {
                ButtonRightName = "OK",
                ButtonLeftName = "",
                ButtonLeftAppearance = Wpf.Ui.Common.ControlAppearance.Transparent,
            };

            if (height > 0)
            {
                messageBox.Height = height;
            }

            if (width > 0)
            {
                messageBox.Width = width;
            }

            messageBox.ButtonRightClick += MessageBoxOK_ButtonRightClickClose;
            messageBox.Show(title, text);
        }

        private static void MessageBoxOK_ButtonRightClickClose(object sender, RoutedEventArgs e)
        {
            (sender as Wpf.Ui.Controls.MessageBox)?.Close();
        }

        #endregion

        #region MessageBox Yes/No

        /// <summary>
        /// MessageBox with yes or no prompt. returns "bool?"
        /// </summary>
        /// <param name="height">Is optional. height = '0' sets it to default</param>
        /// <param name="width">Is optional. width = '0' sets it to default</param>
        public static bool? MessageBoxYesNo(string title, string text, int height = 0, int width = 0)
        {
            Wpf.Ui.Controls.MessageBox messageBox = new()
            {
                ButtonLeftName = "Yes",
                ButtonRightName = "No",
                ButtonRightAppearance = Wpf.Ui.Common.ControlAppearance.Primary,
                ButtonLeftAppearance = Wpf.Ui.Common.ControlAppearance.Secondary
            };
            messageBox.ButtonLeftClick += MessageBox_ButtonLeftClickYes;
            messageBox.ButtonRightClick += MessageBox_ButtonRightClickNo;

            if (height > 0)
            {
                messageBox.Height = height;
            }

            if (width > 0)
            {
                messageBox.Width = width;
            }

            messageBox.Title = title;
            messageBox.Content = text;

            return messageBox.ShowDialog();
        }

        private static void MessageBox_ButtonLeftClickYes(object sender, RoutedEventArgs e)
        {
            Wpf.Ui.Controls.MessageBox messageBox = sender as Wpf.Ui.Controls.MessageBox;
            messageBox.DialogResult = true;
            messageBox.Close();
        }
        private static void MessageBox_ButtonRightClickNo(object sender, RoutedEventArgs e)
        {
            Wpf.Ui.Controls.MessageBox messageBox = sender as Wpf.Ui.Controls.MessageBox;
            messageBox.DialogResult = false;
            messageBox.Close();
        }

        #endregion
    }
}
