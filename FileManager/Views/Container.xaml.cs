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

namespace FileManager.Views;

/// <summary>
/// Interaction logic for Container.xaml
/// </summary>
public partial class Container : INavigationWindow
{
    private readonly ITaskBarService _taskBarService;
    private readonly IWindowService _testWindowService;
    private readonly IThemeService _themeService;
    private bool _initialized = false;

    public Container(ContainerViewModel viewModel, INavigationService navigationService, IPageService pageService, IThemeService themeService, ITaskBarService taskBarService, ISnackbarService snackbarService, IDialogService dialogService, IWindowService testWindowService)
    {
        // Assign the view model
        ViewModel = viewModel;
        DataContext = this;

        // Attach the theme service
        _themeService = themeService;

        // Attach the taskbar service
        _taskBarService = taskBarService;

        //// Context provided by the service provider.
        //DataContext = viewModel;

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

        // !! Experimental option
        //RemoveTitlebar();

        // !! Experimental option
        //ApplyBackdrop(Wpf.Ui.Appearance.BackgroundType.Mica);

        // We initialize a cute and pointless loading splash that prepares the view and navigate at the end.
        Loaded += (_, _) => InvokeSplashScreen();

        // We register a window in the Watcher class, which changes the application's theme if the system theme changes.
        // Wpf.Ui.Appearance.Watcher.Watch(this, Appearance.BackgroundType.Mica, true, false);
        _testWindowService = testWindowService;
    }

    public ContainerViewModel ViewModel
    {
        get;
    }

    // NOTICE: In the case of this window, we navigate to the Dashboard after loading with Container.InitializeUi()
    /// <summary>
    /// Raises the closed event.
    /// </summary>
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        // Make sure that closing this window will begin the process of closing the application.
        Application.Current.Shutdown();
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

    private void InvokeSplashScreen()
    {
        if (_initialized)
            return;

        _initialized = true;

        RootMainGrid.Visibility = Visibility.Visible;

        _taskBarService.SetState(this, TaskBarProgressState.Indeterminate);

        Task.Run(async () =>
        {
            await Dispatcher.InvokeAsync(() =>
            {
                Navigate(typeof(Pages.Page1));

                _taskBarService.SetState(this, TaskBarProgressState.None);
            });

            return true;
        });
    }

    private void NavigationButtonTheme_OnClick(object sender, RoutedEventArgs e)
    {
        _themeService.SetTheme(_themeService.GetTheme() == ThemeType.Dark ? ThemeType.Light : ThemeType.Dark);
    }

    private void OpenSettings_OnClick(object sender, RoutedEventArgs e)
    {
        _testWindowService.Show<Views.Windows.SettingsWindow>();
    }

    private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem menuItem)
            return;

        System.Diagnostics.Debug.WriteLine($"DEBUG | WPF UI Tray clicked: {menuItem.Tag}", "FileManager");
    }
}