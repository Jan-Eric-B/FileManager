using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace FileManager.Services
{
    internal static class MessageBoxService
    {
        public static void MessageBoxOK(string title, string text)
        {
            Wpf.Ui.Controls.MessageBox messageBox = new()
            {
                ButtonRightName = "OK"
            };
            messageBox.ButtonRightClick += MessageBox_RightButtonClickClose;
            messageBox.Show(title, text);
        }

        private static void MessageBox_RightButtonClickClose(object sender, System.Windows.RoutedEventArgs e)
        {
            (sender as Wpf.Ui.Controls.MessageBox)?.Close();
        }

        public static bool? MessageBoxYesNo(string title, string text)
        {
            Wpf.Ui.Controls.MessageBox messageBox = new()
            {
                ButtonLeftName = "Yes",
                ButtonRightName = "No",
            };
            messageBox.ButtonLeftClick += MessageBox_LeftButtonClick;
            messageBox.ButtonRightClick += MessageBox_RightButtonClick;

            messageBox.Title = title;
            messageBox.Content = text;

            return messageBox.ShowDialog();
        }

        private static void MessageBox_LeftButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Wpf.Ui.Controls.MessageBox mb = (sender as Wpf.Ui.Controls.MessageBox);
            mb.DialogResult = true;
            mb.Close();
        }

        private static void MessageBox_RightButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Wpf.Ui.Controls.MessageBox mb = (sender as Wpf.Ui.Controls.MessageBox);
            mb.DialogResult = false;
            mb.Close();
        }





    }
}
