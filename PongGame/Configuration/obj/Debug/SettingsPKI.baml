   M S B A M L   `   `   `  ����H  DConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:ConfigurationConfiguration  5   	       Configuration.SettingsPKI   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   X TXceed.Wpf.Toolkit, Version=3.2.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   U QMaterialDesignThemes.Wpf, Version=2.3.1.953, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   'localclr-namespace:Configuration  5   	   -   5           MaterialDesignBody#���   5	   	   $��Regular"�5
   	   $��14(�5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignPaper#���  5   	     MaterialDesignFont#���  5   	    � Title$ SettingsPKI��5   	   $��383.273��6   $��366.176��6.    � 
ResizeMode$ NoResize=�6U   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��80* �6   �� 5      $
��227* �6   �� 5      $	��51* �6   .+�5   
   #  MaterialDesignThemes.Wpf.Card  $��20q�6   $��
10,10,10,0q�6+   $��3��6?   .��5      ��    MaterialDesignTitleTextBlock#���  6   $��Center=�6O   $
��Auto��6l   5      5      �� 5   
   -   5         ��Name$ TBNicknamePlayer��5      $
��Left=�6,   $��45��6G   $��
75,102,0,0q�6S    �� TextWrapping$
 Wrap=�6g   $	��Top=�6{   $	��240��6�   $��Nickname��6  $��20(�6  $��3��6)  #  Xceed.Wpf.Toolkit.ColorPicker  5   
   -   5          Name$ ColorPlayer��5      $��34,159,45,0q�60   $��89��6E   $	��Top=�6Q   $��3��6�   �� 5   
   -   5         ��Name$ 	BTN_Start��5      $��Start the Game!��6$   $
��Left=�6>   $��
34,269,0,0q�6Y   $	��Top=�6m   $	��281��6�   $��44��6�   
���   �$��3��6�   �� 5    
   -   5        	 ��Name$	 image1��5       $
��Left=�6    $��46��6;   $��	34,31,0,0q�6G   $	��Top=�6Z   $	��281��6r   $��Images/Player1.png��6~   $��3��6�   �� 5!   
   -   5        $	 image2��5!      $
��Left=�6    $��26��6;   $��
39,121,0,0q�6G   $	��Top=�6[   $��26��6s   $��Images/User.png��6~   
 J� RenderTransformOrigin$
 0.5,0.5&�6�   z�5"      V� 5#      .~�5$      ��  �� ScaleX$ -1Q�6%   �� 5%      �� 5&      T� 5'      5(      5*      �� 5+   
   -   5         ��Name$ 	rectColor��5+      ���	#FFF4F4F5$
��Left=�68   $��89��6S   $��
34,159,0,0q�6_   
���   �$	��Top=�6�   $	��255��6�   $��2��6�   5-      5.      