   M S B A M L   `   `   `  ���� L  HOutlook_Clipboard, Version=0.1.4.0, Culture=neutral, PublicKeyToken=null)clr-namespace:WpfUI.ViewsWpfUI.Views      WpfUI.Views.ShellView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:WpfUI.Views     \ XHardcodet.Wpf.TaskbarNotification, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null)tb http://www.hardcodet.net/taskbar  ���#36507e$	��600��$	��740��  � WindowStartupLocation$  CenterScreen=� � WindowStyle$ ThreeDBorderWindow=����	#FF3F3F46 J� OpacityMask
 �   � � 
ResizeMode$ NoResize=�$y�Visible=� � Title �� *Title+.���  � ColumnDefinitions �� $��20 ��� $��21.333 ��� $��188.667 ��� $��60.667 ��� $��149.333 ��� $
��auto ��� $
��auto ��� $��* ��� $
��auto � � RowDefinitions �� $��20 ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $��* ��� $��20 � � 	Resources  �� 
TargetType	 �� Property
 �� Value��%  '��       '�� �     'm�     �� " ��.��� 	 � ��
 �#36507e�� 	 � ��
 �#f1f7fa�� 	 � ��$
 2q��� 	 � ��$	
 210���� 	 � ��$
 35���� 	 � ��$
 0,5,5,6q��� " ��.��� 	 � ��$
 1,1,1,1q��� 	 � ��
 �	#FF3F3F46�� 	 � ��
 �#f1f7fa�� 	 � ��$
 11(��� " m�.��� 	 � ��
 �#f1f7fa�� 	 � ��
 �	#FF3F3F46.+�3  -Hardcodet.Wpf.TaskbarNotification.TaskbarIcon  -     Name$ tb��$	��Top=�   
IconSource$ ../icons/app_icon.ico��   DoubleClickCommand �� *DoubleClickCommand+   LeftClickCommand �� *LeftClickCommand+ � ContextMenu �� $��11(�.@�m� $��Configuration��.@�m� $��Open Log Directory�� m� Command �� *OpenLogDirectory+m� $!��Open Installation Directory�� �� *OpenInstallDirectory+m� $��Open Window�� �� *DoubleClickCommand+m� $��Reset Window�� �� *ResetPosition+m� $��Restart�� �� *RestartApplication+m� $
��Exit�� �� *ExitApplication+�� $��1��$��1��$��2���� $��Center=� �� Orientation$ 
Horizontal=�$��1��$��3��$��5��.+��� -    ��Name$ LoadView�� �� 	ClickMode$ Hover=� � DataContext �� *	Primary+���� *DisplayNameView+{��� *ToLoad+�� -   $ 	LoadView2��$ Hover=� �� *	Secondary+���� *DisplayNameView+{��� *ToLoad+�� -   $ 	LoadView3��$ Hover=� �� *
Tertiary+���� *DisplayNameView+{��� *ToLoad+�� -   $ LoadConfiguration�� � ToolTip$ Configuration��$��35��$��35��$��0q�$ Press=�.���� .+��� $��../icons/config.png���� -    ��Name$ 
ActiveItem��$��5��$��1��$��5��