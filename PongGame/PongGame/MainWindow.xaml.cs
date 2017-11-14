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
using System.Threading;
using Arduino;

namespace PongGame
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Pong myGame;

        Thread pongThreadPlayerOne;
        Thread pongThreadPlayerTwo;

        bool isPlaying;
        bool isSpacePressed;

        ArduinoController arduinoController;

        public MainWindow()
        {
            InitializeComponent();

            myGame = new Pong(this);
            isPlaying = false;
            isSpacePressed = false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space: 
                    if (!isSpacePressed)
                    {
                        arduinoController = new ArduinoController("COM3");

                        pongThreadPlayerOne = new Thread(myGame.runPlayerOne);
                        pongThreadPlayerTwo = new Thread(myGame.runPlayerTwo);

                        pongThreadPlayerOne.Start();
                        pongThreadPlayerTwo.Start();

                        isSpacePressed = true;
                        isPlaying = true; 
                    }
                    else
                    {
                        MessageBox.Show("Game ended.");
                        isSpacePressed = false;
                        isPlaying = false;
                    }

                    break;
            }
        }

        public bool getIsPlaying()
        {
            return isPlaying;
        }

        public ArduinoController getArduinoController()
        {
            return arduinoController;
        }
    }
}
