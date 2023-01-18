using Wpf.Ui.Appearance;
using Wpf.Ui.Interop;

namespace FileManager.Views.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();

            Wpf.Ui.Appearance.Background.Apply(this, BackgroundType.Mica);

            // You can use native methods, but remember that their use is not safe.
            UnsafeNativeMethods.RemoveWindowTitlebar(this);
            UnsafeNativeMethods.ApplyWindowCornerPreference(this, WindowCornerPreference.Round);
        }
    }
}