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

        string nicknamePlayer1;
        string nicknamePlayer2;

        int pointsPlayer1;
        int pointsPlayer2;

        Color colorPlayer1;
        Color colorPlayer2;

        public MainWindow(string nickname1, string nickname2, Color colorPlayer1, Color colorPlayer2)
        {
            InitializeComponent();

            myGame = new Pong(this);
            isPlaying = false;

            nicknamePlayer1 = nickname1;
            nicknamePlayer2 = nickname2;

            this.colorPlayer1 = colorPlayer1;
            this.colorPlayer2 = colorPlayer2;

            pointsPlayer1 = 0;
            pointsPlayer2 = 0;

            setNicknameAndColor();
        }

        private void setNicknameAndColor()
        {
            Slider_Player1.Fill = new SolidColorBrush(colorPlayer1);
            Slider_Player2.Fill = new SolidColorBrush(colorPlayer2);

            TF_Nickname1.Content = nicknamePlayer1;
            TF_Nickname2.Content = nicknamePlayer2;
        }

        private void addPoint(int player)
        {
            if(player == 1)
            {
                switch(pointsPlayer1)
                {
                    case 0:
                        p1p1.Fill = new SolidColorBrush(colorPlayer1);
                        pointsPlayer1++;
                        break;
                    case 1:
                        p1p2.Fill = new SolidColorBrush(colorPlayer1);
                        pointsPlayer1++;
                        break;
                    case 2:
                        p1p3.Fill = new SolidColorBrush(colorPlayer1);
                        pointsPlayer1++;
                        break;
                    case 3:
                        p1p4.Fill = new SolidColorBrush(colorPlayer1);
                        pointsPlayer1++;
                        break;
                    case 4:
                        p1p5.Fill = new SolidColorBrush(colorPlayer1);
                        pointsPlayer1++;

                        MessageBox.Show("Now they always say congratulationssss.");
                        break;
                }
            }
            else
            {
                switch (pointsPlayer2)
                {
                    case 0:
                        p2p1.Fill = new SolidColorBrush(colorPlayer2);
                        pointsPlayer2++;
                        break;
                    case 1:
                        p2p2.Fill = new SolidColorBrush(colorPlayer2);
                        pointsPlayer2++;
                        break;
                    case 2:
                        p2p3.Fill = new SolidColorBrush(colorPlayer2);
                        pointsPlayer2++;
                        break;
                    case 3:
                        p2p4.Fill = new SolidColorBrush(colorPlayer2);
                        pointsPlayer2++;
                        break;
                    case 4:
                        p2p5.Fill = new SolidColorBrush(colorPlayer2);
                        pointsPlayer2++;

                        MessageBox.Show("Now they always say congratulationssss.");
                        break;
                }
            }
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
                    if (winner == 1)
                        addPoint(1);
                    else
                        addPoint(2);

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
