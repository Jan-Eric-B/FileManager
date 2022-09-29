using FileManager.Resources.Settings;
using FileManager.Services;
using FileManager.ViewModels;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows;
using System;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using FileManager.Services.ApplicationStructure;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Forms.Integration;
using FileManager.Models;
using System.Linq;
using CheckBox = System.Windows.Controls.CheckBox;
using System.Collections.Generic;

namespace FileManager.Views;

/// <summary>
/// Interaction logic for Container.xaml
/// </summary>
public partial class Container : INavigationWindow
{
    #region Services

    private readonly IThemeService _themeService;
    private readonly IWindowService _windowService;

    #endregion

    #region MouseDown Variables

    private bool InDoubleClick;
    private DateTime LastClick;
    private readonly Action DoubleClickAction;
    private readonly Action SingleClickAction;
    private readonly Timer ClickTimer;
    private readonly TimeSpan DoubleClickMaxTime;
    private string ToOpenFile = string.Empty;

    #endregion

    //____________________________________________________________

#region Main

public Container(ContainerViewModel viewModel, INavigationService navigationService, IPageService pageService, IThemeService themeService, ISnackbarService snackbarService, IDialogService dialogService, IWindowService windowService)
    {
        // Assign the view model
        ViewModel = viewModel;
        DataContext = this;

        // Attach the theme service
        _themeService = themeService;

        // Initial preparation of the window.
        InitializeComponent();

        // We define a page provider for navigation
        SetPageService(pageService);

        // If you want to use INavigationService instead of INavigationWindow you can define its navigation here.
        navigationService.SetNavigationControl(RootNavigation);

        // Allows you to use the Snackbar control defined in this window in other pages or windows
        snackbarService.SetSnackbarControl(RootSnackbar);

        // Allows you to use the Dialog control defined in this window in other pages or windows
        dialogService.SetDialogControl(RootDialogOK);

        // We register a window in the Watcher class, which changes the application's theme if the system theme changes.
        // Wpf.Ui.Appearance.Watcher.Watch(this, Appearance.BackgroundType.Mica, true, false);
        _windowService = windowService;

        //ItemsControl.ItemsSource = ViewModel.Files;




        cmbFileExtensions.ItemsSource = ViewModel.FileExtensions;

        SettingsLoad();

        string[] args = App.Args;

        if (args.Length > 0)
        {
            string newMainPath = args[0] + Path.DirectorySeparatorChar;

            if (EditFileSevice.IsDirectory(newMainPath))
            {
                ViewModel.MainPath = newMainPath;
            }
        }

        ViewModel.SearchAsync();

        DoubleClickMaxTime = TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime);

        ClickTimer = new Timer
        {
            Interval = SystemInformation.DoubleClickTime
        };
        ClickTimer.Tick += ClickTimer_Tick;

        // Click to copy Path to Clipboard and Doubleclick on Item, tries to open it
        SingleClickAction = () => System.Windows.Clipboard.SetText(ToOpenFile);
        DoubleClickAction = () => StartProcessService.Start(ToOpenFile, true, false);
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

#endregion

#region INavigationWindow methods

public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);
    public Frame GetFrame() => RootFrame;
    public INavigation GetNavigation() => RootNavigation;
    public void CloseWindow() => Close();
    public void SetPageService(IPageService pageService) => RootNavigation.PageService = pageService;
    public void ShowWindow() => Show();

    #endregion INavigationWindow methods

    //Need to be fixed!!!!!!
    #region Settings

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

        ThemeSettings theme = new()
        {
            DarkMode = _themeService.GetTheme() == ThemeType.Dark
        };

        theme.Save();
        settings.Save();
    }
    private void SettingsLoad()
    {
        SearchFilters settings = new();

        if (EditFileSevice.Check(settings.MainPath))
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

    #endregion

    // Search
    #region Search

    // start new search when enabling subdirectories
    private async void CheckBox_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SearchAsync();
    }

    // Filter List
    private void cvsFiles_Filter(object sender, FilterEventArgs e)
    {
        FileModel a = e.Item as FileModel;
        if ((a.FileName.Contains(txSearch.Text)))
        {
            if (cmbFileExtensions.SelectedIndex != 0)
            {
                if (a.Extension.Equals(cmbFileExtensions.SelectedValue))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
            else
            {
                e.Accepted = true;
            }
        }
        else
        {
            e.Accepted = false;
        }

    }

    // When Search Input changes
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        ((CollectionViewSource)this.Resources["cvsFiles"]).View.Refresh();
    }

    // When selecting a combobox item
    private void cmbFileExtensions_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ((CollectionViewSource)this.Resources["cvsFiles"]).View.Refresh();
    }

    // When checking selection Checkboxes
    private void cboSelect_Click(object sender, RoutedEventArgs e)
    {
        ICollectionView view = ((CollectionViewSource)this.Resources["cvsFiles"]).View;
        List<FileModel> viewList = view.Cast<FileModel>().ToList();

        if ((sender as CheckBox).IsChecked == true)
        {
            if ((sender as CheckBox).Name.Equals("cboSelectAll"))
            {
                foreach (FileModel file in viewList)
                {
                    file.IsChecked = true;
                }
            }
            if ((sender as CheckBox).Name.Equals("cboSelectEverySecondFirst"))
            {
                foreach (FileModel file in viewList.Where((_, i) => i % 2 == 0))
                {
                    file.IsChecked = true;
                }
            }
            if ((sender as CheckBox).Name.Equals("cboSelectEverySecondLast"))
            {
                foreach (FileModel file in viewList.Where((_, i) => i % 2 == 1))
                {
                    file.IsChecked = true;
                }
            }
        }
        else
        {
            if ((sender as CheckBox).Name.Equals("cboSelectAll"))
            {
                foreach (FileModel file in viewList)
                {
                    file.IsChecked = false;
                }
            }
            if ((sender as CheckBox).Name.Equals("cboSelectEverySecondFirst"))
            {
                foreach (FileModel file in viewList.Where((_, i) => i % 2 == 0))
                {
                    file.IsChecked = false;
                }
            }
            if ((sender as CheckBox).Name.Equals("cboSelectEverySecondLast"))
            {
                foreach (FileModel file in viewList.Where((_, i) => i % 2 == 1))
                {
                    file.IsChecked = false;
                }
            }
        }
    }

    #endregion

    // ItemList
    #region ItemList

    // Items selected count
    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        ViewModel.FileCountSelected++;
    }
    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        ViewModel.FileCountSelected--;
    }

    //Click and Drag-Over Checkboxes
    private void Checkbox_OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            if (sender is System.Windows.Controls.CheckBox checkbox)
            {
                checkbox.IsChecked = !checkbox.IsChecked;
            }
        }
    }
    private void Checkbox_OnGotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            if (sender is System.Windows.Controls.CheckBox checkbox)
            {
                checkbox.IsChecked = !checkbox.IsChecked;
                checkbox.ReleaseMouseCapture();
            }
        }
    }

    #endregion

    // Click and Doubleclick on Path and Items
    #region MouseDown

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

    // Drag-Over-Event - only accepts one directory
    #region Path drag over

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
                if (!EditFileSevice.IsDirectory(filename))
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

    private async void ProgressBar_Drop(object sender, System.Windows.DragEventArgs e)
    {
        string[] directories = e.Data.GetData(System.Windows.DataFormats.FileDrop, true) as string[];
        if (directories.Length == 1)
        {
            ViewModel.MainPath = directories[0] + Path.DirectorySeparatorChar;
            await ViewModel.SearchAsync();
        }
    }

    #endregion

    

}