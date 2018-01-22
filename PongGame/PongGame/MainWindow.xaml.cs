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
        public int width;
        public int height;
        public int sleepingTime;
        public double currentXPosition;
        public double currentYPosition;
        public SolidColorBrush color;

        public BallControl(Ellipse ball, double currentXPosition, double currentYPosition, int movingXDistance, double movingYDistance, int sleepingTime, SolidColorBrush color)
        {
            this.ball = ball;
            this.currentXPosition = currentXPosition;
            this.currentYPosition = currentYPosition;
            this.movingXDistance = movingXDistance;
            this.movingYDistance = movingYDistance;
            this.sleepingTime = sleepingTime;
            width = 30;
            height = 30;

            this.color = color;
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

        int mode = 0;

        // Player vs Player
        public MainWindow(string nickname1, string nickname2, Color colorPlayer1, Color colorPlayer2)
        {
            InitializeComponent();

            resetPositions();

            mode = 1;

            myGame = new Pong(this);
            myGame.setBallInMiddleOfPlayground();
            isPlaying = false;

            initRectangleArrays();
            firstPlayer = new Player(nickname1, colorPlayer1);
            secondPlayer = new Player(nickname2, colorPlayer2);

            setNicknameAndColor();

            tt.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            arduinoController = new ArduinoController("COM3");
        }

        public MainWindow(string nickname, Color colorPlayer)
        {
            InitializeComponent();

            resetPositions();

            mode = 2;

            myGame = new Pong(this);
            isPlaying = false;

            initRectangleArrays();
            firstPlayer = new Player(nickname, colorPlayer);
            secondPlayer = new Player("KI", colorPlayer);

            setNicknameAndColor();
            setVisible();

            tt.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            arduinoController = new ArduinoController("COM3");
            
        }

        private void setVisible()
        {
            p1p1.Visibility = Visibility.Hidden;
            p1p2.Visibility = Visibility.Hidden;
            p1p3.Visibility = Visibility.Hidden;
            p1p4.Visibility = Visibility.Hidden;
            p1p5.Visibility = Visibility.Hidden;
            p2p2.Visibility = Visibility.Hidden;
            p2p3.Visibility = Visibility.Hidden;
            p2p4.Visibility = Visibility.Hidden;
            p2p5.Visibility = Visibility.Hidden;
        }

        private void resetPositions()
        {
            Canvas.SetTop(Slider_Player1, (Playground_Slider1.Height / 2) - (Slider_Player1.Height) / 2);
            Canvas.SetTop(Slider_Player2, (Playground_Slider2.Height / 2) - (Slider_Player2.Height) / 2);
            Canvas.SetTop(Ball, (Playground.Height / 2) - (Ball.Height) / 2);
            Canvas.SetLeft(Ball, (Playground.Width / 2) - (Ball.Width / 2));
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
                    if(mode == 2)
                    {
                        Congratulations wi = new Congratulations(this);
                        wi.Show();
                        player.addPoints();
                        resetPositions();
                    }
                    else
                    {
                        playerRectangles[0].Fill = new SolidColorBrush(player.getColor());
                        player.addPoints();
                        resetPositions();
                    }
                    break;
                case 1:
                    playerRectangles[1].Fill = new SolidColorBrush(player.getColor());
                    player.addPoints();
                    resetPositions();
                    break;
                case 2:
                    playerRectangles[2].Fill = new SolidColorBrush(player.getColor());
                    player.addPoints();
                    resetPositions();
                    break;
                case 3:
                    playerRectangles[3].Fill = new SolidColorBrush(player.getColor());
                    player.addPoints();
                    resetPositions();
                    break;
                case 4:
                    playerRectangles[4].Fill = new SolidColorBrush(player.getColor());
                    player.addPoints();
                    player.playerWon();
                    resetPositions();

                    Congratulations w = new Congratulations(this);
                    w.Show();

                    //addStatisticsToWebserver();

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
                myGame.stopPopUpTimer();
                if (pongThreadBall != null)     // otherwise exception occures if the game never started
                {
                    pongThreadBall.Abort();
                    arduinoGetDataAndMovingSliderThread.Abort();

                    pongThreadPlayerOne.Abort();
                    pongThreadPlayerTwo.Abort();
                }

                if (winner != -1)
                {
                    myGame.stopPopUpThreads();

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

                if (mode == 1)
                    pongThreadBall = new Thread(myGame.ballMove);
                else if (mode == 2)
                    pongThreadBall = new Thread(myGame.ballMoveKI);

                pongThreadPlayerOne = new Thread(myGame.runPlayerOneKeys);

                if (mode == 1)
                    pongThreadPlayerTwo = new Thread(myGame.runPlayerTwoKeys);
                else if (mode == 2)
                    pongThreadPlayerTwo = new Thread(myGame.runKI);
                //pongThreadPlayerOne = new Thread(myGame.runPlayerOne);
                //pongThreadPlayerTwo = new Thread(myGame.runPlayerTwo);


                myGame.resetPlaygroud();

                arduinoGetDataAndMovingSliderThread.Start();
                pongThreadPlayerOne.Start();
                pongThreadPlayerTwo.Start();
                pongThreadBall.Start();
                myGame.startPopUpTimer();
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
