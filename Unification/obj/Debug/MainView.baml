   M S B A M L   `   `   `  ����  Unification    Unification.MainView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      H DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=null6mah,http://metro.mahapps.com/winfx/xaml/controls  5        � Title$   ��5      $	��500��5	      $	��400��6    � WindowStartupLocation$ CenterScreen=�5
       � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6%   �� 5      $F @pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml?�6%   �� 5      $G Apack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml?�6%   �� 5      $M Gpack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml?�6%   �� 5      $R Lpack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml?�6%   %    5      (  "MahApps.Metro.Controls.MetroWindow   TitleTemplate 5      �� 5   
   .R�5      � $��10,0,0,0q�6    � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $
��Auto �6'   .+�5"      �� $4��.УНИФИКАЦИЯ ДЛИН СТЕРЖНЕЙ��6   $��15(�5#      $��Center=�5$      $��0��5%      �� 5&      -   5        $��Инструкция��5&      $��10(�5'      
������ 
������ $��0q�5*       � Cursor$
 Hand��5+      $��1��5-      
�������5.      5/      50      .��52      �� .+�53   
   �� .+�54      �� -   5         ��Name$ TopmostCheckBox��54      $(��"Поверх других окон��60   $��	10,5,0,10q�6M   $��14(�56   =      MetroCheckBox#��[  57      58      �� 59   
   .+�5:      �� -   5        	 ��Name$	 Длины_Олимпа��5:      $
��Wrap=�6.   $?��9Стандартные длины Олимпроекта:��5;      $��
10,10,0,10q�5<      $��14(�6-   $��Center=�5=        MetroTextBlock#��[ 5>      �� 5?      -   5        $	 Список_длин��5?      $
��Wrap=�6-   $r��l1300, 1650, 1950, 2300, 2900, 3300, 3900, 4600, 5200, 5850, 6500, 7100, 7800, 8800, 9400, 9750, 10400, 11700��5@      $��
10,0,10,10q�5A      $��14(�6!   $��Center=�6/   #��[ 5B      5C      �� 5D   
   .+�5E      �� -   5        $ User_lenghts_check��5E      $7��1Применить свой список длин��5F      
 d� 	IsChecked$
 FalseY�6<   $��
10,10,0,10q�5G      $��14(�6+   #��[  5H      �� 5I      -   5        $!	 Введите_список��5I      $
��Wrap=�60   $X��RВведите свой список длин. Например: 1000, 3000, 10000��5J      $��
10,10,0,10q�5K      $��14(�6.   #��[ 5L      �� 5M      -   5         ��Name$ User_lenghts��5M       �� TextWrapping$
 Wrap=�6,   $��1000, 3000, 5000��6@   $��10,10,10,10q�5N      $��25��6+   $��14(�5O       J� RenderTransformOrigin$ 10,0.5&�6$   $	��460��5P      �� 5Q      -   5        $ LongOrShort��5Q      $g��aИзменить короткий отгиб (True) или длинный (False) для П и Г��5R      $
 FalseY�6^   $��
10,10,0,10q�6p   $��14(�6�   #��[  5S      �� 5T      -	   5         ��Name$ Start��5T      $��Выполнить��6$   $��10,10,10,10q�5U   0   $��14(�5V        MetroButton#��[ 5W      5X      5Y      5Z      