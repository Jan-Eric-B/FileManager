   M S B A M L   `   `   `  ����L  HWPF UI - Demo App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:WPFUI_Clean.Views.WindowsWPFUI_Clean.Views.Windows  5      A%clr-namespace:WPFUI_Clean.Views.PagesWPFUI_Clean.Views.Pages  5      1    +WPFUI_Clean.Views.Windows.TaskManagerWindow   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:WPFUI_Clean.Views.Windows     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      7pages%clr-namespace:WPFUI_Clean.Views.Pages     5      A =Wpf.Ui, Version=2.0.2.0, Culture=neutral, PublicKeyToken=null/ui&http://schemas.lepo.co/wpfui/2022/xaml  5	        � Title$  WPF UI - MS Store Window��5
      $
��1200��5      $	��654��5         ApplicationBackgroundBrush#���   5        Wpf.Ui.Controls.UiWindow!   ExtendsContentIntoTitleBar$
 True��5         WindowBackdropType$
 Mica=�5         WindowCornerPreference$ Round=�5       � WindowStartupLocation$ CenterScreen=�5      .��5      �  � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   '  !Wpf.Ui.Controls.NavigationCompact  -   5          Name$ RootNavigation��5      $��0��5      /  )Wpf.Ui.Controls.Navigation.NavigationBase   Footer 5       �� *ViewModel.NavigationFooter+ �� Mode$ OneWay=�	   Frame	 5!      �� 
 �� ElementName$
 	RootFrame��$ OneWay=�   Items 5"      �� *ViewModel.NavigationItems+$ OneWay=�  Wpf.Ui.Controls.TitleBar  5$   
      Title$! WPF UI - Compact navigation��5%      $��0��5&      $��0��5'      $��2��5(      $��42,0,0,0q�5)         Icon$0 *pack://application:,,,/Resources/wpfui.png��5*      � 5,   
   $��1��6   .+�5-      �� $��0,46,0,0q�5.         ControlFillColorDefaultBrush#���  5/       �� CornerRadius$ 8,0,0,0��50      ._�51      �  � RowDefinitions 52      �� 53      $
��Auto �6(   �� 54      $��* �6(   .+�56         Wpf.Ui.Controls.Breadcrumb  $��0��57      $��38q�58      $
��Left=�59      $	��Top=�5:      $��26(�5;      $
��Bold"�5<         
Navigation 5=      �� $
 RootNavigation��$ OneWay=� � 5>      -   5          �Name$ 	RootFrame��5?      $��1��5@      $
��38,0q�5A      5B      5C      5D      5E      5F      