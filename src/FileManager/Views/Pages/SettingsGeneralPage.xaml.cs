using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FileManager.Models;
using FileManager.Resources.Settings;

namespace FileManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsGeneralPage.xaml
    /// </summary>
    public partial class SettingsGeneralPage
    {
        public SettingsGeneralPage()
        {
            InitializeComponent();

            CustomTreeViewItem root = this.tree.Items[0] as CustomTreeViewItem;

            base.CommandBindings.Add(
                new CommandBinding(
                    ApplicationCommands.Undo,
                    (sender, e) => // Execute
                    {
                        e.Handled = true;
                        root.IsChecked = false;
                        this.tree.Focus();
                    },
                    (sender, e) => // CanExecute
                    {
                        e.Handled = true;
                        e.CanExecute = (root.IsChecked != false);
                    }));

            this.tree.Focus();
        }

        private void HandlePreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(
                    e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        private void CheckBox_Checked_SavePresets(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)e.OriginalSource;
            string checkBoxName = TypeDescriptor.GetProperties(cb.DataContext)["SettingsKey"].GetValue(cb.DataContext).ToString();

            Presets presets = new Presets();

            presets[checkBoxName] = true;

            presets.Save();
        }

        private void CheckBox_Unchecked_SavePresets(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)e.OriginalSource;
            string checkBoxName = TypeDescriptor.GetProperties(cb.DataContext)["SettingsKey"].GetValue(cb.DataContext).ToString();


            Presets presets = new Presets();

            presets[checkBoxName] = false;

            presets.Save();
        }

        private void CheckBox_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {

        }
    }
}