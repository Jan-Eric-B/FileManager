using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.TaskBar;
using FileManager.Services;
using FileManager.ViewModels;
using System.Windows.Input;
using FileManager.Views.Windows;
using FileManager.Resources;
using System.Windows.Forms;

namespace FileManager.Views;

/// <summary>
/// Interaction logic for Container.xaml
/// </summary>
public partial class Container : INavigationWindow
{
    private readonly ITaskBarService _taskBarService;
    private readonly IWindowService _windowService;
    private readonly IThemeService _themeService;
    private readonly bool _initialized = false;

    private DateTime LastClick;
    private bool InDoubleClick;
    private TimeSpan DoubleClickMaxTime;
    private Action DoubleClickAction;
    private Action SingleClickAction;
    private Timer ClickTimer;

    private string ToOpenFile = string.Empty;
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

        LoadSettings();

        ItemsControl.ItemsSource = ViewModel.Files;
        
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
        SaveSettings();

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

    // Settings
    private void OpenSettings_OnClick(object sender, RoutedEventArgs e)
    {
        _windowService.Show<Views.Windows.SettingsWindow>();
    }


    // Settings
    private void SaveSettings()
    {
        SearchFilters settings = new()
        {
            MainPath = ViewModel.MainPath,
            SearchOptionsCaseSensitive = ViewModel.SearchOptionsCaseSensitive,
            SearchOptionsFileContent = ViewModel.SearchOptionsFileContent,
            SearchOptionsFileName = ViewModel.SearchOptionsFileName,
            SearchOptionsSubdirectories = ViewModel.SearchOptionsSubdirectories,
        };
        settings.Save();
    }
    private void LoadSettings()
    {
        SearchFilters settings = new();
        ViewModel.MainPath = settings.MainPath;
        ViewModel.SearchOptionsCaseSensitive = settings.SearchOptionsCaseSensitive;
        ViewModel.SearchOptionsFileContent = settings.SearchOptionsFileContent;
        ViewModel.SearchOptionsFileName = settings.SearchOptionsFileName;
        ViewModel.SearchOptionsSubdirectories = settings.SearchOptionsSubdirectories;
        //RenameInsertCountUp = settings.RenameInsertCountUp;
        //CopyNameRemoveCopied = settings.CopyNameRemoveCopied;
        //CopyNameSingle = settings.CopyNameSingle;
        //CopyNameWithExtension = settings.CopyNameWithExtension;
    }




    // Checkboxes
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

    //Search Input
    private async void SearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Return || e.Key == Key.Tab)
        {
            await ViewModel.SearchingAsync();
        }
    }

    // Search Filter
    private async void CheckBox_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SearchingAsync();
    }


    private void ClickTimer_Tick(object sender, EventArgs e)
    {
        // Clear double click watcher and timer
        InDoubleClick = false;
        ClickTimer.Stop();

        SingleClickAction();
    }
    // Item TextBlock
    private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        ToOpenFile = ViewModel.MainPath + ((TextBlock)sender).Text;
        MouseDownChange();
    }
    // Main Path
    private void Path_MouseDown(object sender, MouseButtonEventArgs e)
    {
        ToOpenFile = ViewModel.MainPath;
        MouseDownChange();
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


}