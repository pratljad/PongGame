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
    public struct ControlePlayer
    {
        public bool up;
        public bool down;
    }

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Pong myGame;

        Thread pongThreadPlayerOne;
        Thread pongThreadPlayerTwo;
        Thread pongThreadBall;

        bool isPlaying;

        ControlePlayer player1 = new ControlePlayer();
        ControlePlayer player2 = new ControlePlayer();

        ArduinoController arduinoController;

        int winner = -1;

        public MainWindow()
        {
            InitializeComponent();

            myGame = new Pong(this);
            isPlaying = false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space: 
                    if (!isPlaying)
                    {
                        arduinoController = new ArduinoController("COM6");
                        setIsPlaying(true);
                    }
                    else
                    {
                        setIsPlaying(false);
                        MessageBox.Show("Game ended.");
                    }

                    break;

                case Key.W:
                    player1.up = true;
                    break;

                case Key.S:
                    player1.down = true;
                    break;

                case Key.Up:
                    player2.up = true;
                    break;

                case Key.Down:
                    player2.down = true;
                    break;
            }
        }

        public void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    player1.up = false;
                    break;

                case Key.S:
                    player1.down = false;
                    break;

                case Key.Up:
                    player2.up = false;
                    break;

                case Key.Down:
                    player2.down = false;
                    break;
            }
        }

        public bool getIsPlaying()
        {
            return isPlaying;
        }

        public void setIsPlaying(bool x)
        {
            isPlaying = x;

            if (!isPlaying)
            {
                pongThreadPlayerOne.Abort();
                pongThreadPlayerTwo.Abort();
                pongThreadBall.Abort();

                if (winner != -1)
                {
                    MessageBox.Show("Player " + winner + " WON!");
                    winner = -1;
                }
            }

            if (isPlaying)
            {
                player1.up = false;
                player1.down = false;
                player2.up = false;
                player2.down = false;

                pongThreadPlayerOne = new Thread(myGame.runPlayerOneKeys);
                pongThreadPlayerTwo = new Thread(myGame.runPlayerTwoKeys);
                pongThreadBall = new Thread(myGame.ballMove);

                pongThreadPlayerOne.Start();
                pongThreadPlayerTwo.Start();
                pongThreadBall.Start();
            }
        }

        public ArduinoController getArduinoController()
        {
            return arduinoController;
        }

        public ControlePlayer getControlPlayer1()
        {
            return player1;
        }

        public ControlePlayer getControlPlayer2()
        {
            return player2;
        }

        public void noContactWithBallAndPlayer(int player)
        {
            if (player == 1 || player == 2)
                winner = (player==1) ? 2 : 1;

            setIsPlaying(false);
        }
    }
}
