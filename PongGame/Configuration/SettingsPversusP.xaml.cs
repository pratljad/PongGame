﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace Configuration
{
    /// <summary>
    /// Interaktionslogik für SettingsPversusP.xaml
    /// </summary>
    public partial class SettingsPversusP : Window
    {
        public SettingsPversusP()
        {
            InitializeComponent();
        }

        private void BTN_Start_Click(object sender, RoutedEventArgs e)
        {
            // am server noch checken ob username schon vorhanden

            if (TBNicknamePlayer1.Text != "" && TBNicknamePlayer2.Text != "" && ColorPlayer1.SelectedColorText != "" && ColorPlayer2.SelectedColorText != "" && TBNicknamePlayer1.Text != TBNicknamePlayer2.Text)
            {
                PongGame.MainWindow pg = new PongGame.MainWindow(TBNicknamePlayer1.Text, TBNicknamePlayer2.Text, (Color)ColorPlayer1.SelectedColor, (Color)ColorPlayer2.SelectedColor);
                this.Close();
                pg.Show();
            }
            else
            {
                BTN_Start.BorderBrush = Brushes.Red;
            }
        }
    }
}
