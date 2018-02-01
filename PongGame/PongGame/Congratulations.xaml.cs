using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace PongGame
{
    /// <summary>
    /// Interaktionslogik für Congratulations.xaml
    /// </summary>
    public partial class Congratulations : Window
    {
        private int seconds = 0;
        private Timer tt = null;
        private MainWindow mw = null;
        private string directory = System.IO.Path.GetFullPath("../../../Images/");

        public Congratulations(MainWindow mw)
        {
            InitializeComponent();

            tt = new Timer(1000);
            tt.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            tt.Start();
            this.mw = mw;

            this.Icon = BitmapFrame.Create(new Uri(directory + "Icon.ico", UriKind.Absolute));
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(directory + "Giphy.gif", UriKind.Absolute);
            image.EndInit();
            ImageBehavior.SetAnimatedSource(GIF, image);
        }

        public void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            seconds += 1;

            if (seconds == 4)
            {
                tt.Stop();

                Dispatcher.InvokeAsync(
                new Action(() =>
                {
                    this.Close();
                    mw.Close();
                }));
            }           
        }
    }
}
