using FileManager.Resources;
using FileManager.Services;
using FileManager.ViewModels;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace FileManager.Views;

/// <summary>
/// Interaction logic for Container.xaml
/// </summary>
public partial class Container : INavigationWindow
{
    private readonly bool _initialized = false;
    private readonly ITaskBarService _taskBarService;
    private readonly IThemeService _themeService;
    private readonly IWindowService _windowService;


    public Container(ContainerViewModel viewModel, INavigationService navigationService, IPageService pageService, IThemeService themeService, ITaskBarService taskBarService, ISnackbarService snackbarService, IDialogService dialogService, IWindowService windowService)
    {
        // Assign the view model
        ViewModel = viewModel;
        DataContext = this;

        // Attach the theme service
        _themeService = themeService;

        // Attach the taskbar service
        _taskBarService = taskBarService;

        // Initial preparation of the window.
        InitializeComponent();

        // We define a page provider for navigation
        SetPageService(pageService);

        // If you want to use INavigationService instead of INavigationWindow you can define its navigation here.
        navigationService.SetNavigationControl(RootNavigation);

        // Allows you to use the Snackbar control defined in this window in other pages or windows
        snackbarService.SetSnackbarControl(RootSnackbar);

        // Allows you to use the Dialog control defined in this window in other pages or windows
        dialogService.SetDialogControl(RootDialog);

        // We register a window in the Watcher class, which changes the application's theme if the system theme changes.
        // Wpf.Ui.Appearance.Watcher.Watch(this, Appearance.BackgroundType.Mica, true, false);
        _windowService = windowService;

        SettingsLoad();

        ItemsControl.ItemsSource = ViewModel.FileModel.Files;

        ViewModel.SearchingAsync().Wait();

        DoubleClickMaxTime = TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime);

        ClickTimer = new Timer();
        ClickTimer.Interval = SystemInformation.DoubleClickTime;
        ClickTimer.Tick += ClickTimer_Tick;

        // Click to copy Path to Clipboard and Doubleclick on Item, tries to open it
        SingleClickAction = () => System.Windows.Clipboard.SetText(ToOpenFile);
        DoubleClickAction = () => StartProcess.Start(ToOpenFile, true);
    }

    public ContainerViewModel ViewModel
    {
        get;
    }

    // Close Event
    protected override void OnClosed(EventArgs e)
    {
        SettingsSave();

        base.OnClosed(e);

        // Make sure that closing this window will begin the process of closing the application.
        System.Windows.Application.Current.Shutdown();
    }

    #region INavigationWindow methods

    public void CloseWindow()
        => Close();

    public Frame GetFrame()
            => RootFrame;

    public INavigation GetNavigation()
        => RootNavigation;

    public bool Navigate(Type pageType)
        => RootNavigation.Navigate(pageType);

    public void SetPageService(IPageService pageService)
        => RootNavigation.PageService = pageService;

    public void ShowWindow()
        => Show();

    #endregion INavigationWindow methods

    // Theme
    private void NavigationButtonTheme_OnClick(object sender, RoutedEventArgs e)
    {
        _themeService.SetTheme(_themeService.GetTheme() == ThemeType.Dark ? ThemeType.Light : ThemeType.Dark);
    }

    // Settings Window
    private void OpenSettings_OnClick(object sender, RoutedEventArgs e)
    {
        _windowService.Show<Views.Windows.SettingsWindow>();
    }

    // Checkbox
    #region Checkboxes

    // Search Filter, Refresh search
    private async void CheckBox_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SearchingAsync();
    }

    // Items selected count
    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        ViewModel.FileCountSelected++;
    }
    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        ViewModel.FileCountSelected--;
    }

    //Click and Drag over Checkboxes
    private void Checkbox_OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;

        if (e.LeftButton == MouseButtonState.Pressed)
        {
            if (checkbox != null)
            {
                checkbox.IsChecked = !checkbox.IsChecked;
            }
        }
    }
    private void Checkbox_OnGotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            if (checkbox != null)
            {
                checkbox.IsChecked = !checkbox.IsChecked;
                checkbox.ReleaseMouseCapture();
            }
        }
    }

    #endregion

    // Search Input
    private async void SearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Return || e.Key == Key.Tab)
        {
            await ViewModel.SearchingAsync();
        }
    }

    // Settings
    private void SettingsSave()
    {
        SearchFilters settings = new()
        {
            MainPath = ViewModel.MainPath,
            SearchOptionsCaseSensitive = ViewModel.SearchOptionsCaseSensitive,
            SearchOptionsFileContent = ViewModel.SearchOptionsFileContent,
            SearchOptionsFileName = ViewModel.SearchOptionsFileName,
            SearchOptionsSubdirectories = ViewModel.SearchOptionsSubdirectories,
        };

        ThemeSettings theme = new();
        if (_themeService.GetTheme() == ThemeType.Dark)
        {
            theme.DarkMode = true;
        }
        else
        {
            theme.DarkMode = false;
        }

        theme.Save();
        settings.Save();
    }



    // FIXEN
    private void SettingsLoad()
    {
        SearchFilters settings = new();

        if (CheckExistenceService.Check(settings.MainPath))
        {
            ViewModel.MainPath = settings.MainPath;
        }
        else
        {
            ViewModel.MainPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
        ViewModel.SearchOptionsCaseSensitive = settings.SearchOptionsCaseSensitive;
        ViewModel.SearchOptionsFileContent = settings.SearchOptionsFileContent;
        ViewModel.SearchOptionsFileName = settings.SearchOptionsFileName;
        ViewModel.SearchOptionsSubdirectories = settings.SearchOptionsSubdirectories;
        //RenameInsertCountUp = settings.RenameInsertCountUp;
        //CopyNameRemoveCopied = settings.CopyNameRemoveCopied;
        //CopyNameSingle = settings.CopyNameSingle;
        //CopyNameWithExtension = settings.CopyNameWithExtension;
        ThemeSettings theme = new();

        if (theme.DarkMode)
        {
            _themeService.SetTheme(ThemeType.Dark);
        }
        else
        {
            _themeService.SetTheme(ThemeType.Light);
        }

    }

    // Click and Doubleclick on Path and Items
    #region MouseDown

    private Timer ClickTimer;
    private Action DoubleClickAction;
    private TimeSpan DoubleClickMaxTime;
    private bool InDoubleClick;
    private DateTime LastClick;
    private Action SingleClickAction;
    private string ToOpenFile = string.Empty;

    private void ClickTimer_Tick(object sender, EventArgs e)
    {
        // Clear double click watcher and timer
        InDoubleClick = false;
        ClickTimer.Stop();

        SingleClickAction();
    }
    private void MouseDownChange()
    {
        if (InDoubleClick)
        {
            InDoubleClick = false;

            TimeSpan length = DateTime.Now - LastClick;

            // If double click is valid, respond
            if (length < DoubleClickMaxTime)
            {
                ClickTimer.Stop();
                DoubleClickAction();
            }

            return;
        }

        // Double click was invalid, restart
        ClickTimer.Stop();
        ClickTimer.Start();
        LastClick = DateTime.Now;
        InDoubleClick = true;
    }

    // Main Path
    private void Path_MouseDown(object sender, MouseButtonEventArgs e)
    {
        ToOpenFile = ViewModel.MainPath;
        MouseDownChange();
    }
    // Item TextBlock
    private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        ToOpenFile = ViewModel.MainPath + ((TextBlock)sender).Text;
        MouseDownChange();
    }
    #endregion

    private void Button_Click(object sender, RoutedEventArgs e)
    {
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {

    }


    private void ProgressBar_Drop(object sender, System.Windows.DragEventArgs e)
    {
        string[] directories = e.Data.GetData(System.Windows.DataFormats.FileDrop, true) as string[];
        if(directories.Length == 1)
        {
            ViewModel.MainPath = directories[0] + Path.DirectorySeparatorChar;
            ViewModel.SearchingAsync().Wait();
        }
    }

    // Drag over event - only accepts one directory
    private void ProgressBar_DragOver(object sender, System.Windows.DragEventArgs e)
    {
        bool dropEnabled = true;
        if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
        {
            string[] items = e.Data.GetData(System.Windows.DataFormats.FileDrop, true) as string[];

            if(items.Length >= 2)
            {
                dropEnabled = false;
            }

            foreach (string filename in items)
            {
                if (!CheckExistenceService.IsDirectory(filename))
                {
                    dropEnabled = false;
                    break;
                }
            }
        }
        else
        {
            dropEnabled = false;
        }

        if (!dropEnabled)
        {
            e.Effects = System.Windows.DragDropEffects.None;
            e.Handled = true;
        }
    }


}