   M S B A M L   `   `   `  ����L  HOutlook_Clipboard, Version=0.1.4.0, Culture=neutral, PublicKeyToken=null)clr-namespace:WpfUI.ViewsWpfUI.Views  5          WpfUI.Views.AppConfigView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:WpfUI.Views     5      ���#36507e.��5	      �   � 	Resources  5   
    �� 
TargetType �� Property �� Value��%�  '��       '�� N     '�� �     '�� �    '��      �� 5      " ��6   .�5      ��  � �� �#36507e60   5      �� 5      " ��6   .�5      ��  � �� �#f1f7fa6/   �� 5       � ��$ 80��6*   �� 5       � ��$ 10q�6+   5      �� 5      " ��6   .�5      ��  � �� �#36507e6/   �� 5       � �� �#f1f7fa6/   �� 5       � ��$ 90��6*   �� 5       � ��$ 0,5,5,0q�6+   �� 5       � ��$
 Left=�68   �� 5       � ��$
 Bold"�6/   5      �� 5      " ��6   .�5      ��  � ��
 �����6/   �� 5       � ��$
 Left=�68   �� 5       � ��$
 Left=�6?   �� 5        � ��$ Bottom=�66   �� 5!       � ��$ 5,0,0,0q�6+   �� 5"       � ��$ 25��6+   5#      �� 5$      " ��6   .�5%      ��  � �� �#36507e6/   �� 5&       � ��$ 0,5,5,2q�6+   �� 5'       � ��$	 300��6*   5(      5)       � ColumnDefinitions 5+   
   �� 5,      $
��auto �6   �� 5-      $
��auto �6   �� 5.      $��* �6   �� 5/      $��5 �6    � RowDefinitions 51   
   �� 52      $��5 �6   �� 53      $
��auto �6   �� 54      $
��auto �6   �� 55      $
��auto �6   �� 56      $
��auto �6   �� 57      $��5 �6   .+�5:   
   �� $��1��5;      $��1��5<       �� Orientation$ Vertical=�5=      .+�5>      �� $��2��5?      $ 
Horizontal=�5@      .+�5A      �� $��Account Settings��5B      $	��220��5C      $��50��5D      $��25(�5E      5F      5H      �� 5J      $ Vertical=�5K      $��1��5L      $��1��5M      $
��auto��5N      .+�5O      �� $��1��6   $��1��6+   $��5��68    �� AlternationCount$ 2��5Q      ���#f1f7fa �� SelectionMode$ Single=�5T      $	��100��5U      $	��300��5V      $��5q�5W      ��5P      �� 	 �� Path$	 Accounts�
 �� SelectedItem
 5S      �� $	 SelectedConfiguration���5Y      �� 5Z      .R�5[      �� ���#CCCCCC$��1q�5\   %   $	��267��5]   %   ._�5^   "   �   5_   &   ��%�   '��       �� 5`   *   " ��60   .�5a   .   ��  � �� �#36507e6K   �� 5b   .    � ��$
 auto��6F   �� 5c   .    � ��$ 0,5,5,0q�6G   �� 5d   .    � ��$
 Left=�6T   �� 5e   .    � ��$
 Bold"�6K   5f   +   5g   '    5j   &   �� 5k   *   $
��auto �6;   6K   �� 5l   *   $
��auto �6;   6K    5n   &   �� 5o   *   $
��auto �68   6H   .+�5s   &   �� $��14(�5t   (   $��0��5u   (   $��0��5v   (   $
��Bold"�5w   (   ��5s   ,   �� *EmailAddress+5x   '   5y   #   5z      5{      5}      �� 5~      -   5         ��Name$ 
RemoveItem��5�      $��Right=�5~      $��0q�5      $��0q�5�      $��25��5�      {�5�      �� *	CanRemove+.��69   Remove5�      �� 5�      $��2��5�      $ 
Horizontal=�5�      .+�5�      �� -   5         ��Name$ AccountField��5�      $	��200��5�      $��	5,10,5,10q�5�      .��6,   5�      �� 5�      -   5        $	 Add��5�      $	��Add��5�      5�      5�      5�      5�      �� 5�   
   $��2��5�      $��1��5�      .+�5�      �� $��2��5�      $ 
Horizontal=�5�      .+�5�      �� $��
Properties��5�      $	��220��5�      $��40��5�      $��20(�5�      5�      5�      �� 5�      $��2��5�      $ 
Horizontal=�5�      .+�5�      �� $��Display Name��5�      $��2��6+   5�      �� 5�      -   5        $' !SelectedConfiguration_DisplayName��5�      $	��200��5�      .��5�   @   5�      5�      �� 5�      $��2��6   $ 
Horizontal=�5�      .+�5�      �� $��Email Address��5�      $��90��5�      5�      �� 5�      -   5        $( "SelectedConfiguration_EmailAddress��5�      $	��200��5�      .��5�   A   5�      5�      �� 5�      $��2��5�      $ 
Horizontal=�5�      .+�5�      �� $��
Search Tag��5�      5�      �� 5�      -   5        $% SelectedConfiguration_SearchTag��5�      $	��150��5�      .��6!   5�      5�      �� 5�      $��2��5�      $ 
Horizontal=�5�      .+�5�      �� $��Search Size��5�      5�      �� 5�      -   5        $&  SelectedConfiguration_SearchSize��5�      $��40��5�      .��6    5�      �� 5�      $��200-300 Emails��5�      $	��120��5�      $��
ExtraLight"�5�      5�      5�      �� 5�      $��2��5�      $ 
Horizontal=�5�      .+�5�      �� $��Display Items��5�      $��2��6,   5�      �� 5�      -   5        $( "SelectedConfiguration_DisplayItems��5�      $��25��5�      .��6    5�      �� 5�      $��
5-10 Items��5�      $	��120��5�      $��
ExtraLight"�5�      5�      5�      �� 5�      $��2��5�      $ 
Horizontal=�5�      .+�5�      �� $��Search Timer��5�      5�      �� 5�      -	   5        $) #SelectedConfiguration_TimerInterval��5�      $��25��5�      .��6    5�      �� 5�      $��30-360 seconds��5�      $	��120��5�      $��
ExtraLight"�5�      5�      5�      �� 5�      $��2��5�      $ 
Horizontal=�5�      .+�5�      �� $��Display Index��5�      5�      �� 5�      -
   5        $( "SelectedConfiguration_DisplayIndex��5�      $��25��5�      .��5�   A   5�      �� 5�      $��1-3 must be unique��5      $	��110��5     $��
ExtraLight"�5     5     5     �� 5     $��2��5     $ 
Horizontal=�5     .+�5	     �� -   5        $ 
SaveConfig��5
     $��Save Configuration��5     $	��150��5     5     5     5     5     5     