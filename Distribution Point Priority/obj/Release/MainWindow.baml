   M S B A M L   `   `   `  ���� V  RDistribution Point Priority, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:WPF.JoshSmith.ServiceProviders.UI!WPF.JoshSmith.ServiceProviders.UI  ,    &Distribution_Point_Priority.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     CdragMgr/clr-namespace:WPF.JoshSmith.ServiceProviders.UI       � Title$.  (DP Content Distribution Priority Manager��$	��800��$	��650��$��12(� � WindowStartupLocation$ CenterScreen=� � 	Resources    ItemContStyle �� 
TargetType �� 	Resources  Key �� 
StartPoint �� EndPoint � Color � Offset	 �� Property
 �� Value �� Triggers=   7WPF.JoshSmith.ServiceProviders.UI.ListViewItemDragState P� Property P� Value  MouseOverBrush��%�  &	        7  �� " �� ���� MouseOverBrush $ 0.5, 0&�$ 0.5, 1&�.��� $ 	#22000000��$ 0Q�� $ 	#44000000��$	 0.4Q�� $ 	#55000000��$	 0.6Q�� $ 	#33000000��$	 0.9Q�� $ 	#22000000��$ 1Q�.��� 	 � ��$	
 0,4q��� 	 � ��$
 Stretch=��� 	 � ��$
 	0,0,0,0.5q��� 	 � ��

 ����� P�  �@ IsBeingDragged . . ��� 	 � ��$
 DemiBold"�P�  �@ IsUnderDragCursor . . ��� 	 � ��8
   .���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto �.+��� $��Instructions��$��0��$��4q�$��2q�.���� .+��� $j��d1. Configure settings below, then click 'Get Priorities' to load existing data for your environment.��$
��Wrap=�$��10,5,4,0q��� $����2. Drag and drop in the list below to reorder the distribution points. New priority values will recalculate automatically. NOTE: Actual priority values don't matter; only the sequence is important. Lower numbers will receive content first.��$
��Wrap=�$��10,5,4,0q��� $g��a3. When you are satisfied with the order, click 'Set Priorities' to write new values back to WMI.��$
��Wrap=�$��10,5,4,0q��� $��Site Settings��$��1��$��4q�$��2q�.���� .+���  �� Orientation$ 
Horizontal=�$��10,5,0,0q�.+��� $��Site Server:��$��70���� -   $��tbSiteServer��$��siteserver.contoso.com��$	��200��$
��Left=��� $ 
Horizontal=�$��10,5,0,0q�.+��� $��
Site Code:��$��70���� -   $��
tbSiteCode��$	��ABC��$	��200��$
��Left=��� $ 
Horizontal=�$��10,5,0,0q�$��Right=�.+��� -    ��Name$ 
btnRefresh��$��Get Priorities��$	��120��$��10,5,0,0q��� -   $ btnReset��$��	Reset All��$	��120��$��10,5,0,0q��� -   $ btnApply��$��Save Changes��$	��120��$��10,5,0,0q��� -   $��tbStatus��$��2��$��Status: Ready��$��10,5,4,5q��� -   $��listView��$��3��#��[  $��4q�$��2q� �� SelectionMode$ Single=� �� View �� .P��� $��Distribution Point�� �� Width$	 200�� �� CellTemplate �� .R��� ���� *Server+�� $��Current Priority��$	 105�� �� .R���  �� TextAlignment$ Center=����� *
Priority+�� $��New Priority��$	 105�� �� .R��� $ Center=����� *NewPriority+�� $��Use Shared Priority��$	 150�� �� .R��� $��chkBox��-   $��Center=� d� 	IsChecked �� *Shared+�� $��4��$7��1Copyright 2019 by Justin Holloman (adminsccm.com)��$
��Wrap=�$��10,5,4,0q�