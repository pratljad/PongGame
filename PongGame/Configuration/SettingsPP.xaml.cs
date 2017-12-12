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
            ColorPlayer1.SelectedColor = Colors.Blue;
            ColorPlayer2.SelectedColor = Colors.Green;
        }

        private void BTN_Start_Click(object sender, RoutedEventArgs e)
        {
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

        private void Window_Closed(object sender, EventArgs e)
        {
            _mw.opened = false;
        }
    }
}
