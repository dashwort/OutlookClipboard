   M S B A M L   `   `   `  ���� L  HOutlook_Clipboard, Version=0.1.4.0, Culture=neutral, PublicKeyToken=null)clr-namespace:WpfUI.ViewsWpfUI.Views  $    WpfUI.Views.InitialStartupView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:WpfUI.Views       � Title$  Configuration Wizard��$	��360��$	��570�����#36507e � 
ResizeMode$ NoResize=�.���  � 	Resources  �� 
TargetType �� Property �� Value��%�  '��       '�� �     '�� B    '�� �    �� " ��.���  � �� �#36507e��  � �� �#f1f7fa��  � ��$ 2q���  � ��$	 120����  � ��$ 40����  � ��$ 10q���  � ��$ 16(���  � ��$ Light"��� " ��.���  � �� �#f1f7fa��  � ��$ 10,2,2,5q���  � ��$ 5,2,2,5q���  � ��$ 20(��� " ��.���  � ��$ 5q���  � ��$ 10,5,2,5q���  � ��$ 35����  � ��$
 Left=���  � ��$ Center=���  � ��$ 15(��� " ��.���  � �� �#36507e��  � �� �#f1f7fa��  � ��$ 10,10,10,10q���  � ��$ 10q���  � ��$
 Left=���  � ��$ Center=���  � ��$ Center=� � ColumnDefinitions �� $��10 ��� $
��auto ��� $
��auto ��� $��10 � � RowDefinitions �� $��10 ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $��10 �.+���  �� Orientation$ 
Horizontal=�$��2��$��1��$��1��.+��� $��Email Address��$
��Bold"�$	��300���� $��Display Name��$
��Bold"�$��1���� $ 
Horizontal=�$��2��$��2��$��1��.+��� -   	 ��Name$	 Account1��$��2��$��2��$	��300��
 � ToolTip$5
 /Enter the Email Address, value cannot be blank.��.���� -   $	 Account1Displayname��$��2��$��3��$8
 2Enter display name for account. 15 max characters.��$	��150�� �� 	MaxLength$ 15��.���� $��4��$��2��$��10q�$��45��$��5q����� *Account1Colour+�� $ 
Horizontal=�$��2��$��3��$��1��.+��� -   $	 Account2��$��3��$��2��$	��300��$5
 /Enter the Email Address, value cannot be blank.��.���� -   $	 Account2Displayname��$��3��$��3��$8
 2Enter display name for account. 15 max characters.��$	��150��$ 15��.���� $��3��$��3��$��10q�$��45��$��5q����� *Account2Colour+�� $ 
Horizontal=�$��2��$��4��$��1��.+��� -   $	 Account3��$��4��$��2��$	��300��$5
 /Enter the Email Address, value cannot be blank.��.���� -   $	 Account3Displayname��$��4��$��3��$8
 2Enter display name for account. 15 max characters.��$	��150��$ 15��.���� $��3��$��4��$��10q�$��45��$��5q����� *Account3Colour+�� $ Vertical=�$��1��$��5��$��1��$��10q�.+��� $��Run at startup��$��16(� d� 	IsChecked �� *RunAtStartup+�� $��Search Tag:��$��16(�$��28��$��10,0,0,0q�$��0q��� -   $	 	SearchTag��$��25��$	��150��$��10,0,0,0q�$��0q�.���� $ 
Horizontal=�$��Right=�$��Bottom=�$��5��$��2��$��1��.+��� -    ��Name$
 Save��$
��Save��$��5q�{��� *CanAdd+�� -	   $ cancel��$��Cancel��$��5q�