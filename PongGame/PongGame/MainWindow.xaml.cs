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
using System.Timers;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net;

namespace PongGame
{
    public struct PlayerControl
    {
        public bool up;
        public bool down;
    }

    public struct BallControl
    {
        public Ellipse ball;
        public double movingYDistance;
        public int movingXDistance;

        public BallControl(Ellipse ball, int movingXDistance, double movingYDistance)
        {
            this.ball = ball;
            this.movingXDistance = movingXDistance;
            this.movingYDistance = movingYDistance;
        }
    }

    public struct SliderControl
    {
        public Rectangle slider;
        public int movingDistance;
    }

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Pong myGame;

        Thread pongThreadBall;
        Thread arduinoGetDataAndMovingSliderThread;
        Thread pongThreadPlayerOne;
        Thread pongThreadPlayerTwo;

        bool isPlaying;
        int winner = -1;

        PlayerControl player1 = new PlayerControl();
        PlayerControl player2 = new PlayerControl();
        ArduinoController arduinoController;

        // Timer
        System.Timers.Timer tt = new System.Timers.Timer(1000);
        int seconds = 0;
        int minutes = 0;
        string actualTime = "";
        
        Player firstPlayer;
        Player secondPlayer;

        Rectangle[] firstPlayerRectangles;
        Rectangle[] secondPlayerRectangles;

        public MainWindow(string nickname1, string nickname2, Color colorPlayer1, Color colorPlayer2)
        {
            InitializeComponent();

            myGame = new Pong(this);
            isPlaying = false;

            initRectangleArrays();
            firstPlayer = new Player(nickname1, colorPlayer1);
            secondPlayer = new Player(nickname2, colorPlayer2);

            setNicknameAndColor();

            tt.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            arduinoController = new ArduinoController("COM3");
        }

        private void initRectangleArrays()
        {
            firstPlayerRectangles = new Rectangle[] { p1p1, p1p2, p1p3, p1p4, p1p5 };
            secondPlayerRectangles = new Rectangle[] { p2p1, p2p2, p2p3, p2p4, p2p5 };
        }

        private void setNicknameAndColor()
        {
            Slider_Player1.Fill = new SolidColorBrush(firstPlayer.getColor());
            Slider_Player2.Fill = new SolidColorBrush(secondPlayer.getColor());

            TF_Nickname1.Content = firstPlayer.getNickname();
            TF_Nickname2.Content = secondPlayer.getNickname();
        }

        public void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            seconds += 1;

            if (seconds == 60)
            {
                minutes += 1;
                seconds = 0;
            }

            if (seconds < 10 && minutes < 10)
            {
                actualTime = "0" + minutes + ":0" + seconds;
            }
            else if (seconds < 10 && minutes > 9)
            {
                actualTime = minutes + ":0" + seconds;
            }
            else if (seconds > 9 && minutes < 10)
            {
                actualTime = "0" + minutes + ":" + seconds;
            }

            Dispatcher.Invoke(
                new Action(() =>
                {
                    TF_Timer.Content = actualTime;
                })
            );
        }

        private void reset()
        {
            firstPlayer.resetPoints();
            secondPlayer.resetPoints();

            firstPlayer.resetWonStatus();
            secondPlayer.resetWonStatus();

            resetBrushesForRectangle(firstPlayerRectangles);
            resetBrushesForRectangle(secondPlayerRectangles);

            seconds = 0;
            minutes = 0;
            actualTime = "";

            Dispatcher.Invoke(
                new Action(() =>
                {
                    TF_Timer.Content = "00:00";
                })
            );
        }

        private void resetBrushesForRectangle(Rectangle[] rectangles)
        {
            foreach(Rectangle r in rectangles)
            {
                r.Fill = Brushes.White;
            }
        }

        private void addPoint(int player)
        {
            if (player == 1)
            {
                setPoints(firstPlayer, firstPlayerRectangles);
            }
            else
            {
                setPoints(secondPlayer, secondPlayerRectangles);
            }
        }

        public void setPoints(Player player, Rectangle[] playerRectangles)
        {
            switch (player.getPoints())
            {
                case 0:
                    playerRectangles[0].Fill = new SolidColorBrush(player.getColor());
                    player.addPoints();
                    break;
                case 1:
                    playerRectangles[1].Fill = new SolidColorBrush(player.getColor());
                    player.addPoints();
                    break;
                case 2:
                    playerRectangles[2].Fill = new SolidColorBrush(player.getColor());
                    player.addPoints();
                    break;
                case 3:
                    playerRectangles[3].Fill = new SolidColorBrush(player.getColor());
                    player.addPoints();
                    break;
                case 4:
                    playerRectangles[4].Fill = new SolidColorBrush(player.getColor());
                    player.addPoints();
                    player.playerWon();
                    

                    MessageBox.Show("Player: " + player.getNickname() + " has won!! Congratulations!");
                    addStatisticsToWebserver();
                    reset();
                    break;
            }
        }

        private void addStatisticsToWebserver()
        {
            string requestin = "http://127.0.0.1:3000/addscore";

            object data = new Userdata(new int[] { firstPlayer.getPoints(), secondPlayer.getPoints() }
                                            , new string[] { firstPlayer.getNickname(), secondPlayer.getNickname() }
                                            , new int[] { firstPlayer.getWinStatus(), secondPlayer.getWinStatus() });

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestin);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(data.GetType());
                    MemoryStream ms = new MemoryStream();
                    ser.WriteObject(ms, data);
                    String json = Encoding.UTF8.GetString(ms.ToArray());

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space: 
                    if (!isPlaying)
                    {
                        setIsPlaying(true);

                        tt.Start();
                    }
                    else
                    {
                        setIsPlaying(false);
                        MessageBox.Show("Game paused.");

                        tt.Stop();
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
                tt.Stop();

                pongThreadBall.Abort();
                arduinoGetDataAndMovingSliderThread.Abort();

                pongThreadPlayerOne.Abort();
                pongThreadPlayerTwo.Abort();

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

                arduinoGetDataAndMovingSliderThread = new Thread(myGame.readDataFromArduino);
                pongThreadBall = new Thread(myGame.ballMove);

                pongThreadPlayerOne = new Thread(myGame.runPlayerOneKeys);
                pongThreadPlayerTwo = new Thread(myGame.runPlayerTwoKeys);
                //pongThreadPlayerOne = new Thread(myGame.runPlayerOne);
                //pongThreadPlayerTwo = new Thread(myGame.runPlayerTwo);
                

                arduinoGetDataAndMovingSliderThread.Start();
                pongThreadPlayerOne.Start();
                pongThreadPlayerTwo.Start();
                pongThreadBall.Start();
            }
        }

        public ArduinoController getArduinoController()
        {
            return arduinoController;
        }

        public PlayerControl getControlPlayer1()
        {
            return player1;
        }

        public PlayerControl getControlPlayer2()
        {
            return player2;
        }

        public void noContactWithBallAndPlayer(int player)
        {
            if (player == 1 || player == 2)
                winner = (player==1) ? 2 : 1;

            setIsPlaying(false);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            setIsPlaying(false);
        }
    }
}
