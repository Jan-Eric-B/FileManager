﻿#pragma checksum "..\..\..\..\Views\Container.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F7FA4C33EF6C9E84FD4D6C3B4F28A2BA4D0900C7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FileManager.Converters;
using FileManager.Views.Pages;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Wpf.Ui;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Converters;
using Wpf.Ui.Markup;
using Wpf.Ui.ValidationRules;


namespace FileManager.Views {
    
    
    /// <summary>
    /// Container
    /// </summary>
    public partial class Container : Wpf.Ui.Controls.UiWindow, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 33 "..\..\..\..\Views\Container.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid RootMainGrid;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Views\Container.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Wpf.Ui.Controls.NavigationCompact RootNavigation;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\..\Views\Container.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame RootFrame;
        
        #line default
        #line hidden
        
        
        #line 167 "..\..\..\..\Views\Container.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ItemsControl ItemsControl;
        
        #line default
        #line hidden
        
        
        #line 196 "..\..\..\..\Views\Container.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Wpf.Ui.Controls.Snackbar RootSnackbar;
        
        #line default
        #line hidden
        
        
        #line 211 "..\..\..\..\Views\Container.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Wpf.Ui.Controls.Dialog RootDialog;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.7.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WPF UI - Demo App;component/views/container.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Container.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.7.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.RootMainGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.RootNavigation = ((Wpf.Ui.Controls.NavigationCompact)(target));
            return;
            case 3:
            
            #line 72 "..\..\..\..\Views\Container.xaml"
            ((Wpf.Ui.Controls.NavigationItem)(target)).Click += new System.Windows.RoutedEventHandler(this.NavigationButtonTheme_OnClick);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 76 "..\..\..\..\Views\Container.xaml"
            ((Wpf.Ui.Controls.NavigationItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenSettings_OnClick);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 102 "..\..\..\..\Views\Container.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Path_MouseDown);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 103 "..\..\..\..\Views\Container.xaml"
            ((System.Windows.Controls.ProgressBar)(target)).Drop += new System.Windows.DragEventHandler(this.ProgressBar_Drop);
            
            #line default
            #line hidden
            
            #line 103 "..\..\..\..\Views\Container.xaml"
            ((System.Windows.Controls.ProgressBar)(target)).DragOver += new System.Windows.DragEventHandler(this.ProgressBar_DragOver);
            
            #line default
            #line hidden
            return;
            case 7:
            this.RootFrame = ((System.Windows.Controls.Frame)(target));
            return;
            case 8:
            
            #line 147 "..\..\..\..\Views\Container.xaml"
            ((Wpf.Ui.Controls.TextBox)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.SearchBox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 152 "..\..\..\..\Views\Container.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Click += new System.Windows.RoutedEventHandler(this.CheckBox_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.ItemsControl = ((System.Windows.Controls.ItemsControl)(target));
            return;
            case 13:
            this.RootSnackbar = ((Wpf.Ui.Controls.Snackbar)(target));
            return;
            case 14:
            this.RootDialog = ((Wpf.Ui.Controls.Dialog)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.7.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 11:
            
            #line 172 "..\..\..\..\Views\Container.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            
            #line 172 "..\..\..\..\Views\Container.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.CheckBox_Unchecked);
            
            #line default
            #line hidden
            
            #line 172 "..\..\..\..\Views\Container.xaml"
            ((System.Windows.Controls.CheckBox)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.Checkbox_OnMouseEnter);
            
            #line default
            #line hidden
            
            #line 172 "..\..\..\..\Views\Container.xaml"
            ((System.Windows.Controls.CheckBox)(target)).GotMouseCapture += new System.Windows.Input.MouseEventHandler(this.Checkbox_OnGotMouseCapture);
            
            #line default
            #line hidden
            break;
            case 12:
            
            #line 173 "..\..\..\..\Views\Container.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.TextBlock_MouseLeftButtonDown);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

