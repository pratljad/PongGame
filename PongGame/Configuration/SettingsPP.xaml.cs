using System;
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

namespace Configuration
{
    /// <summary>
    /// Interaktionslogik für SettingsPP.xaml
    /// </summary>
    public partial class SettingsPP : Window
    {
        private MainWindow _mw = null;
        public SettingsPP(MainWindow mw)
        {
            InitializeComponent();
            _mw = mw;

            // Settings
            ColorPlayer1.SelectedColor = Colors.Blue;
            ColorPlayer2.SelectedColor = Colors.Green;
            TBNicknamePlayer1.Foreground = Brushes.Gray;
            TBNicknamePlayer2.Foreground = Brushes.Gray;
        }

        private void BTN_Start_Click(object sender, RoutedEventArgs e)
        {
            if (TBNicknamePlayer1.Text != "" && TBNicknamePlayer2.Text != "" && ColorPlayer1.SelectedColorText != "" && ColorPlayer2.SelectedColorText != "" && TBNicknamePlayer1.Text != TBNicknamePlayer2.Text)
            {
                if(TBNicknamePlayer1.Text != "Nickname" && TBNicknamePlayer2.Text != "Nickname")
                {
                    if(!(WordFilter.filter.Contains(TBNicknamePlayer1.Text) || WordFilter.filter.Contains(TBNicknamePlayer2.Text)))
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
                else
                {
                    BTN_Start.BorderBrush = Brushes.Red;
                }
            }
            else
            {
                BTN_Start.BorderBrush = Brushes.Red;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _mw.opened = false;
        }

        private void TBNicknamePlayer1_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Foreground == Brushes.Gray)
                {
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Foreground = Brushes.Black;
                }
            }
        }

        private void TBNicknamePlayer1_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Text.Trim().Equals(""))
                {
                    ((TextBox)sender).Foreground = Brushes.Gray;
                    ((TextBox)sender).Text = "Nickname";
                }
            }
        }

        private void TBNicknamePlayer2_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Foreground == Brushes.Gray)
                {
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Foreground = Brushes.Black;
                }
            }
        }

        private void TBNicknamePlayer2_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Text.Trim().Equals(""))
                {
                    ((TextBox)sender).Foreground = Brushes.Gray;
                    ((TextBox)sender).Text = "Nickname";
                }
            }
        }

        private void ColorPlayer2_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            rectColor2.Fill = new SolidColorBrush((Color)ColorPlayer2.SelectedColor);
        }

        private void ColorPlayer1_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            rectColor1.Fill = new SolidColorBrush((Color)ColorPlayer1.SelectedColor);
        }
    }
}

