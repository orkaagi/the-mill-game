   M S B A M L   `   `   `  ����  MillGame.WPF"    MillGame.WPF.View.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Sudoku játék��5   	   $	��650��6   $	��600��6+   $	��550��67   $	��500��6G    � 	Resources 5         VisibilityConverter��%   &	        �� 5   
   6'   5      .��5
      � ��5   
   �� 5       �� ImageSource$ /background.jpg��6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   o� $��0��6   .@�5      m� $��Fájl��6   .@�5      m� $��Új játék��6    m� Command 6-   �� *NewGameCommand+�� 5      m� 5      $��Játék betöltése...��6    67   �� *LoadGameCommand+m� 5      $��Játék mentése...��6    65   �� *SaveGameCommand+�� 5      m� 5      $��	Kilépés��6    6,   �� *ExitCommand+5      5      �� 5   
   $��1��6   ��6$   �� *Fields+��5      �� 5      .R�5       A�  A� Rows$ 7��6"    A� Columns$ 7��6+   5!      ��5#      �� 5$      .R�5%      �� $|�False��6]    J� RenderTransformOrigin$ 0.5, 0.5&�6o   $��50��5&   t   $��50��6   ��5%      �� *StepCommand+��6=   �� *XY+y�5&      �� *IsButtonVisible+ �� 	Converter# [  z�5'      �� 5(      	 �� ScaleX$	 1Q�6-   
 �� ScaleY$
 1Q�68   ��5*      �� 5+       �� 
TargetType" ��6$    �� Triggers 5,   "   �� 5-   &    �� Binding 62   �� *Player+ �� Value -16M   .a�5.   *   ��  �� Property � �� �� Value
 �����6G   5/   '   �� 50   &    62   �� *Player+ 06M   .a�51   *   ��  � ��
 �  ��6G   52   '   �� 53   &    62   �� *Player+ 16M   .a�54   *   ��  � ��
 ��� �6G   55   '   .�57   "   ��  � �� 58   &   �� 59   *    �� 
TargetType" ��6:   .R�5:   .   �� #��z��65   
���   �$��3q�6s    �� CornerRadius$ 25��6�   ._�5;   2   �� $��Center=�6C   $��Center=�6`   5<   /   5=   +   5?   #   5@       � Triggers 5B      :� 5C       :� RoutedEvent$ Button.Click��6+   .[�5D   "   �� .��5E   &   �� .��5F   *   W�  g� Duration$ 	0:00:00.1D�6:    W� From 16O   	 W� To 0.56X    g� AutoReverse$
 True��6a    �� TargetProperty$/ )(RenderTransform).(ScaleTransform.ScaleX)�6t   W� 5G   *   $ 	0:00:00.1D�6:    16O    0.56X   $
 True��6a   $/ )(RenderTransform).(ScaleTransform.ScaleY)�6t   5H   '   5I   #   5J      5L      5M      ��5O      �� 5P      .�5Q      ��  � �� 61   �� *X+�� 5R       � �� 64   �� *Y+5S      5U      5V      5W      