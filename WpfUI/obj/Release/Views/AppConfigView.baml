   M S B A M L   `   `   `  ���� L  HOutlook_Clipboard, Version=0.1.4.0, Culture=neutral, PublicKeyToken=null)clr-namespace:WpfUI.ViewsWpfUI.Views      WpfUI.Views.AppConfigView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     +localclr-namespace:WpfUI.Views     ���#36507e.���   � 	Resources   �� 
TargetType �� Property �� Value��%Q  '��       '�� )     '�� ~     '�� $    '�� �    �� " ��.���  � �� �#36507e�� " ��.���  � �� �#f1f7fa��  � ��$ 80����  � ��$ 10q��� " ��.���  � �� �#36507e��  � �� �#f1f7fa��  � ��$ 90����  � ��$ 0,5,5,0q���  � ��$
 Left=���  � ��$
 Bold"��� " ��.���  � ��
 �������  � ��$
 Left=���  � ��$
 Left=���  � ��$ Bottom=���  � ��$ 5,0,0,0q���  � ��$ 25���� " ��.���  � �� �#36507e��  � ��$ 0,5,5,2q���  � ��$	 300�� � ColumnDefinitions �� $
��auto ��� $
��auto ��� $��* ��� $��5 � � RowDefinitions �� $��5 ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $��5 �.+��� $��1��$��1�� �� Orientation$ Vertical=�.+��� $��2��$ 
Horizontal=�.+��� $��Account Settings��$	��220��$��50��$��25(��� $ Vertical=�$��1��$��1��$
��auto��.+��� $��1��$��1��$��5�� �� AlternationCount$ 2�����#f1f7fa �� SelectionMode$ Single=�$	��100��$	��300��$��5q����� 	 �� Path$	 Accounts�
 �� SelectedItem
 �� $	 SelectedConfiguration����� .R��� ���#CCCCCC$��1q�$	��267��._��   ��%�   '��       �� " ��.���  � �� �#36507e��  � ��$
 auto����  � ��$ 0,5,5,0q���  � ��$
 Left=���  � ��$
 Bold"� �� $
��auto ��� $
��auto � �� $
��auto �.+��� $��14(�$��0��$��0��$
��Bold"����� *EmailAddress+�� -    ��Name$ 
RemoveItem��$��Right=�$��0q�$��0q�$��25��{��� *	CanRemove+.��Remove�� $��2��$ 
Horizontal=�.+��� -    ��Name$ AccountField��$	��200��$��	5,10,5,10q�.���� -   $	 Add��$	��Add���� $��2��$��1��.+��� $��2��$ 
Horizontal=�.+��� $��
Properties��$	��220��$��40��$��20(��� $��2��$ 
Horizontal=�.+��� $��Display Name��$��2���� -   $' !SelectedConfiguration_DisplayName��$	��200��.���� $��2��$ 
Horizontal=�.+��� $��Email Address��$��90���� -   $( "SelectedConfiguration_EmailAddress��$	��200��.���� $��2��$ 
Horizontal=�.+��� $��
Search Tag���� -   $% SelectedConfiguration_SearchTag��$	��150��.���� $��2��$ 
Horizontal=�.+��� $��Search Size���� -   $&  SelectedConfiguration_SearchSize��$��40��.���� $��200-300 Emails��$	��120��$��
ExtraLight"��� $��2��$ 
Horizontal=�.+��� $��Display Items��$��2���� -   $( "SelectedConfiguration_DisplayItems��$��25��.���� $��
5-10 Items��$	��120��$��
ExtraLight"��� $��2��$ 
Horizontal=�.+��� $��Search Timer���� -	   $) #SelectedConfiguration_TimerInterval��$��25��.���� $��30-360 seconds��$	��120��$��
ExtraLight"��� $��2��$ 
Horizontal=�.+��� $��Display Index���� -
   $( "SelectedConfiguration_DisplayIndex��$��25��.���� $��1-3 must be unique��$	��110��$��
ExtraLight"��� $��2��$ 
Horizontal=�.+��� -   $ 
SaveConfig��$��Save Configuration��$	��150��