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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Configuration
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool opened = false;
        public bool opened2 = false;
        public MainWindow()
        {
            InitializeComponent();
            this.Background = Brushes.DarkSlateBlue;
            WordFilter.init();
        }

        private void BTN_PlayervsPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (!opened && !opened2)
            {
                SettingsPP w = new SettingsPP(this);
                w.Show();
                opened = true;
            }
        }

        private void BTN_Leaderboard_Click(object sender, RoutedEventArgs e)
        {
            Leaderboards l = new Leaderboards();
            l.Show();

            this.Close();
        }

        private void BTN_PlayervsKI_Click(object sender, RoutedEventArgs e)
        {
            if (!opened && !opened2)
            {
                SettingsPKI w = new SettingsPKI(this);
                w.Show();
                opened2 = true;
            }
        }
    }
}
