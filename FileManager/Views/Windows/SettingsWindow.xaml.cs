using Wpf.Ui.Appearance;

namespace FileManager.Views.Windows;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
public partial class SettingsWindow
{
    public SettingsWindow()
    {
        InitializeComponent();

        Wpf.Ui.Appearance.Background.Apply(this, BackgroundType.Mica);
    }
}