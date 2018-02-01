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
using System.IO;
using System.Diagnostics;
using PongGame;

namespace Configuration
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public bool windowPlayerVsPlayerOpened = false;
        public bool windowPlayerVsKiOpened = false;
        private string directory = GameControle.getAbsoluteImagePath();

        public MainWindow()
        {
            Console.WriteLine("PongGame started... ");
            InitializeComponent();
            init();
            this.Background = Brushes.DarkSlateBlue;
            WordFilter.init();
        }

        private void init()
        {
            if (!Directory.Exists(GameControle.getAbsoluteImagePath()))
                Console.WriteLine("Folder not found! Check if Image folder is located at :'" + GameControle.getAbsoluteImagePath() + "'");
            
            else
            {
                this.Icon = GameControle.getIcon();
                image1.Source = GameControle.getImageFigurePlayerTwo();
                image2.Source = GameControle.getImageKI();
                image3.Source = GameControle.getImageControllerTwo();
                image4.Source = GameControle.getImageControllerOne();
                image5.Source = GameControle.getImageFigurePlayerOne();
                image6.Source = GameControle.getImageFigurePlayerOne();
            }
        }

        private void BTN_PlayervsPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (!windowPlayerVsPlayerOpened && !windowPlayerVsKiOpened && !GameControle.isAGameRunning())
            {
                windowPlayerVsPlayerOpened = true;
                SettingsPP w = new SettingsPP(this);
                w.Show();
            }
        }

        private void BTN_PlayervsKI_Click(object sender, RoutedEventArgs e)
        {
            if (!windowPlayerVsPlayerOpened && !windowPlayerVsKiOpened && !GameControle.isAGameRunning())
            {
                windowPlayerVsKiOpened = true;
                SettingsPKI w = new SettingsPKI(this);
                w.Show();
            }
        }
    }
}
