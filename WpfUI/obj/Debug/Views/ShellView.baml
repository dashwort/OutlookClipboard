   M S B A M L   `   `   `  ����L  HOutlook_Clipboard, Version=0.1.4.0, Culture=neutral, PublicKeyToken=null)clr-namespace:WpfUI.ViewsWpfUI.Views  5   	       WpfUI.Views.ShellView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:WpfUI.Views     5   	   \ XHardcodet.Wpf.TaskbarNotification, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null)tb http://www.hardcodet.net/taskbar  61   ���#36507e$	��600��5   !   $	��740��6.     � WindowStartupLocation$  CenterScreen=�5	   	    � WindowStyle$ ThreeDBorderWindow=�6.   ���	#FF3F3F46 J� OpacityMask
 �   � � 
ResizeMode$ NoResize=�5
   5   $y�Visible=�5   	    � Title 5   	   �� *Title+.��5      �  � ColumnDefinitions 5   
   �� 5      $��20 �6   �� 5      $��21.333 �6   �� 5      $��188.667 �6   �� 5      $��60.667 �6   �� 5      $��149.333 �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   �� 5      $
��auto �6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5       $��* �6   �� 5!      $��20 �6    � 	Resources 5%   
    �� 
TargetType	 �� Property
 �� Value��%p  '��       '��     'm� �    �� 5&      " ��6   .�5'      �� 	 � ��
 �#36507e6/   �� 5(      	 � ��
 �#f1f7fa6/   �� 5)      	 � ��$
 2q�64   �� 5*      	 � ��$	
 210��6*   �� 5+      	 � ��$
 35��6+   �� 5,      	 � ��$
 0,5,5,6q�6+   5-      �� 5/      " ��6   .�50      �� 	 � ��$
 1,1,1,1q�51      �� 52      	 � ��
 �	#FF3F3F466/   �� 53      	 � ��
 �#f1f7fa6/   �� 54      	 � ��$
 11(�6-   55      �� 57      " m�6   .�58      �� 	 � ��
 �#f1f7fa6/   �� 59      	 � ��
 �	#FF3F3F466/   5:      5<      .+�5>   
   3  -Hardcodet.Wpf.TaskbarNotification.TaskbarIcon  -   5          Name$ tb��5?      $	��Top=�5@         
IconSource$ ../icons/app_icon.ico��5A         DoubleClickCommand 5B      �� *DoubleClickCommand+   LeftClickCommand 5C      �� *LeftClickCommand+ � ContextMenu 5D      �� 5E      $��11(�5F      .@�5G      m� $��Configuration��5H      .@�5I      m� $��Open Log Directory��6#    m� Command 5J       �� *OpenLogDirectory+m� 5K      $!��Open Installation Directory��6#    5L       �� *OpenInstallDirectory+5M      m� 5N      $��Open Window��6    5O      �� *DoubleClickCommand+m� 5P      $��Reset Window��6    5Q      �� *ResetPosition+m� 5R      $��Restart��6    5S       �� *RestartApplication+m� 5T      $
��Exit��6    5U       �� *ExitApplication+5V      5X      �� 5\   
   $��1��5]      $��1��5^      $��2��6   5_      �� 5a   
   $��Center=�5b       �� Orientation$ 
Horizontal=�5c      $��1��5d      $��3��5e      $��5��5f      .+�5h      �� -   5         ��Name$ LoadView��5h       �� 	ClickMode$ Hover=�5i       � DataContext 5j      �� *	Primary+��5k      �� *DisplayNameView+{�5l      �� *ToLoad+5m      �� 5n      -   5        $ 	LoadView2��5n      $ Hover=�5o       5p      �� *	Secondary+��5q      �� *DisplayNameView+{�5r      �� *ToLoad+5s      �� 5t      -   5        $ 	LoadView3��5t      $ Hover=�5u       5v      �� *
Tertiary+��5w      �� *DisplayNameView+{�5x      �� *ToLoad+5y      �� 5z      -   5        $ LoadConfiguration��5{       � ToolTip$ Configuration��5|      $��35��5}      $��35��5~      $��0q�5      $ Press=�5�      .��5�      �� .+�5�      �� $��../icons/config.png��6   5�      5�      5�      �� 5�   
   -   5         ��Name$ 
ActiveItem��5�      $��5��5�      $��1��5�      $��5��5�      5�      5�      5�      