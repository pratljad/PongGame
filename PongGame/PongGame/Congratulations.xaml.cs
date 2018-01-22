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
        int seconds = 0;
        Timer tt = null;
        MainWindow mw = null;

        public Congratulations(MainWindow mw)
        {
            InitializeComponent();

            tt = new Timer(1000);
            tt.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            tt.Start();

            this.mw = mw;

            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(@"C:\HTL\5.Schuljahr\SYP PRE\PongGame\PongGame\Images\giphy.gif");
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
