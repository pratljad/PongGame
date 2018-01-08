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
    /// Interaktionslogik für SettingsPKI.xaml
    /// </summary>
    public partial class SettingsPKI : Window
    {
        MainWindow _mw = null;

        public SettingsPKI(MainWindow mw)
        {
            InitializeComponent();

            _mw = mw;

            // Settings
            ColorPlayer.SelectedColor = Colors.Blue;
            TBNicknamePlayer.Foreground = Brushes.Gray;
        }

        private void BTN_Start_Click(object sender, RoutedEventArgs e)
        {
            if (TBNicknamePlayer.Text != "" && ColorPlayer.SelectedColorText != "")
            {
                if (TBNicknamePlayer.Text != "Nickname")
                {
                    if (!(WordFilter.filter.Contains(TBNicknamePlayer.Text)))
                    {
                        PongGame.MainWindow pg = new PongGame.MainWindow(TBNicknamePlayer.Text, (Color)ColorPlayer.SelectedColor);
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

        private void TBNicknamePlayer_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
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

        private void TBNicknamePlayer_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
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

        private void ColorPlayer_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            rectColor.Fill = new SolidColorBrush((Color)ColorPlayer.SelectedColor);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _mw.opened2 = false;
        }
    }
}   
