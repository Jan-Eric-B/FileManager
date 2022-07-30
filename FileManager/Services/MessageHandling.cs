using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;

using Wpf.Ui;

namespace FileManager.Services
{
    internal static class MessageHandling
    {
        public static bool IsOpen { get; set; }

        public static void PrintException(string message, Exception ex)
        {
            if (ex != null)
            {
                if (!string.IsNullOrEmpty(message)) message += "\n";

                if (!IsOpen)
                {
                    Show(message + ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                PrintException("", ex.InnerException);
            }
        }

        public static void Show(string text, string caption, MessageBoxButton buttonName, MessageBoxImage image)
        {
            IsOpen = true;

            Wpf.Ui.Controls.MessageBox messageBox = new();


            messageBox.Content = text;
            messageBox.Title = caption;

            messageBox.ButtonRightName = buttonName.ToString();
            messageBox.ButtonRightClick += CloseMessageBox;

            messageBox.ButtonLeftAppearance = Wpf.Ui.Common.ControlAppearance.Transparent;
            messageBox.ButtonLeftName = "";

            messageBox.SizeToContent = SizeToContent.WidthAndHeight;


            messageBox.Show();

            //MessageBox.Show(text, caption, button, image);
            IsOpen = false;
        }

        private static void CloseMessageBox(object sender, RoutedEventArgs e)
        {
            Wpf.Ui.Controls.MessageBox messageBox = (Wpf.Ui.Controls.MessageBox)sender;
            messageBox.Close();
        }
    }
}