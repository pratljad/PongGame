using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PongGame
{
    public struct PopUp
    {
        public UIElement element;
        public int id;
        public double xCooardinate; //position in Canvas X
        public double yCooardinate; //position in Canvas Y
        public double height;       //height of element
        public double width;        //width of element

        public PopUp(UIElement element, int id, double xCooardinate, double yCooardinate, double height, double width)
        {
            this.element = element;
            this.id = id;
            this.xCooardinate = xCooardinate;
            this.yCooardinate = yCooardinate;
            this.height = height;
            this.width = width;
        }
    }

    public struct PopUpArea
    {
        public double minXCooardinate;
        public double maxXCooardinate;
        public double minYCooardinate;
        public double maxYCooardinate;

        public PopUpArea(double minXCooardinate, double maxXCooardinate, double minYCooardinate, double maxYCooardinate)
        {
            this.minXCooardinate = minXCooardinate;
            this.maxXCooardinate = maxXCooardinate;
            this.minYCooardinate = minYCooardinate;
            this.maxYCooardinate = maxYCooardinate;
        }
    }

    public class Pong
    {
        private MainWindow mw;
        private BallControl ballControl;
        private PopUpArea popUpAreaBorders;

        private Thread popUpCreationThread;
        private Thread collisionThread;

        public bool directionisLeft = false;
        private int maxAngle = 50;

        private bool directionChanged = false;
        private Random rnd = new Random();

        private int differenceTopBallSlider = 0;
        private string path = "../../../Images/";

        private int popUpCreationDelay = 200;
        private int popUpActiveTime = 3000;
        private List<PopUp> popUps;
        private int maxPopUps = 15;
        private int popUpSize = 20;
        private List<Thread> popUpThreads;
        private int threadCounter = 0;

        public Pong(MainWindow mw)
        {
            this.mw = mw;
            ballControl = new BallControl(
                mw.Ball,
                mw.Playground.Width / 2,
                mw.Playground.Height / 2,
                5, 0, 10, Brushes.Red);
            mw.Ball.Fill = ballControl.color;

            popUpAreaBorders = new PopUpArea(
                mw.Playground.Width / 4,                        //left
                mw.Playground.Width - mw.Playground.Width / 4,  //right
                0,                                              //top
                mw.Playground.Height);                          //bottom

            mw.Dispatcher.InvokeAsync(
                         new Action(() =>
                         {
                             differenceTopBallSlider = Convert.ToInt32(Canvas.GetTop(mw.Ball)) - Convert.ToInt32(Canvas.GetTop(mw.Slider_Player1));
                         }));

            popUps = new List<PopUp>();
            popUpThreads = new List<Thread>();
            popUpCreationThread = new Thread(createPopUp);
            collisionThread = new Thread(checkCollision);
        }

        private void createPopUp()
        {
            bool sleep = true;
            while (mw.getIsPlaying() && Thread.CurrentThread.ThreadState.Equals(ThreadState.Running))
            {
                if (sleep)
                    Thread.Sleep(popUpCreationDelay);

                else
                    Thread.Sleep(30);

                //pop up can be createt and ball is not in the ground where pop up can be created.
                if (popUps.Count < maxPopUps &&
                    (ballControl.currentXPosition - ballControl.width / 2 < popUpAreaBorders.minXCooardinate ||
                    ballControl.currentXPosition - ballControl.width / 2 > popUpAreaBorders.maxXCooardinate))
                {
                    createNewPopUp();
                    sleep = true;
                }
                else
                    sleep = false;
            }
        }

        private void createNewPopUp()
        {
            Application.Current.Dispatcher.InvokeAsync(
            new Action(() =>
            {
                PopUp newPopUpElement;
                UIElement element = getUIElement();
                newPopUpElement = new PopUp(element, 1, -1, -1, ((Ellipse)(element)).Height, ((Ellipse)(element)).Width);
                setRandomImageAndId(ref newPopUpElement);
                drawPopUpOnCanvas(newPopUpElement);
            })
            );
        }

        private UIElement getUIElement()
        {
            Ellipse element = new Ellipse();
            element.Stroke = Brushes.Black;
            ImageBrush img = new ImageBrush();
            element.HorizontalAlignment = HorizontalAlignment.Left;
            element.VerticalAlignment = VerticalAlignment.Center;
            element.Height = popUpSize;
            element.Width = popUpSize;

            return element;
        }

        private void setRandomImageAndId(ref PopUp item)
        {
            ImageBrush img = new ImageBrush();
            int id = rnd.Next(1, 5);

            switch (id)
            {
                //bigger
                case 1:
                    id = rnd.Next(1, 3);
                    switch (id)
                    {
                        case 1: //i get bigger
                            img.ImageSource = new BitmapImage(new Uri(path + "GreenPlus.jpg", UriKind.Relative));
                            item.id = 1;
                            break;
                        case 2: //opponent get bigger
                            img.ImageSource = new BitmapImage(new Uri(path + "RedPlus.jpg", UriKind.Relative));
                            item.id = 2;
                            break;
                    }
                    break;
                //smaller
                case 2:
                    id = rnd.Next(1, 3);
                    switch (id)
                    {
                        case 1: //opponent get smaller
                            img.ImageSource = new BitmapImage(new Uri(path + "GreenMinus.jpg", UriKind.Relative));
                            item.id = 3;
                            break;
                        case 2: //i get smaller
                            img.ImageSource = new BitmapImage(new Uri(path + "RedMinus.jpg", UriKind.Relative));
                            item.id = 4;
                            break;
                    }
                    break;
                //ball
                case 3:
                    id = rnd.Next(1, 3);
                    switch (id)
                    {
                        case 1:
                            img.ImageSource = new BitmapImage(new Uri(path + "BallFaster.jpg", UriKind.Relative));
                            item.id = 5;
                            break;
                        case 2:
                            img.ImageSource = new BitmapImage(new Uri(path + "BallSlower.jpg", UriKind.Relative));
                            item.id = 6;
                            break;
                    }
                    break;
                //special
                case 4:
                    id = rnd.Next(1, 3);
                    switch (id)
                    {
                        case 1:
                            img.ImageSource = new BitmapImage(new Uri(path + "Clear.jpg", UriKind.Relative));
                            item.id = 7;
                            break;
                        case 2:
                            img.ImageSource = new BitmapImage(new Uri(path + "Questionmark.jpg", UriKind.Relative));
                            item.id = 8;
                            break;
                    }
                    break;
            }

            ((Ellipse)item.element).Fill = img;
        }

        private void drawPopUpOnCanvas(PopUp item)
        {
            double positionX;
            double positionY;

            do
            {
                positionX = getRandomXCooardinate(item);
                positionY = getRandomYCooardinate(item);
            } while (checkPopUpOnPosition(positionX, positionY));

            item.xCooardinate = positionX;
            item.yCooardinate = positionY;

            lock (popUps)
                popUps.Add(item);
            mw.Playground.Children.Add(item.element);
            Canvas.SetTop(item.element, item.yCooardinate);
            Canvas.SetLeft(item.element, item.xCooardinate);
        }

        private double getRandomXCooardinate(PopUp item)
        {
            int min = (int)(popUpAreaBorders.minXCooardinate + item.width);
            int max = (int)(popUpAreaBorders.maxXCooardinate - item.width);
            return rnd.Next(min, max);
        }

        public double getRandomYCooardinate(PopUp item)
        {
            int min = (int)(popUpAreaBorders.minYCooardinate + item.width);
            int max = (int)(popUpAreaBorders.maxYCooardinate - item.width);
            return rnd.Next(min, max);
        }

        private bool checkPopUpOnPosition(double positionX, double positionY)
        {
            foreach (PopUp popUp in popUps)
            {
                if (checkIfElementOnPoisition(popUp, positionX, positionY, popUpSize))
                    return true;
            }

            return false;
        }

        private bool checkIfElementOnPoisition(PopUp popUp, double positionX, double positionY, double popUpSize)
        {
            //check if popUp has any interact with a new PopUp
            if (
                (positionX >= popUp.xCooardinate) && (positionX <= (popUp.xCooardinate + popUpSize)) && (positionY >= (popUp.yCooardinate)) && (positionY <= (popUp.yCooardinate + popUpSize)) ||                                             //left top corner is in a other Popup
                (positionX >= popUp.xCooardinate) && (positionX <= (popUp.xCooardinate + popUpSize)) && (positionY + popUpSize >= (popUp.yCooardinate)) && (positionY + popUpSize <= (popUp.yCooardinate + popUpSize)) ||                         //left bottom corner is in a other Popup
                (positionX + popUpSize >= popUp.xCooardinate) && (positionX + popUpSize <= (popUp.xCooardinate + popUpSize)) && (positionY >= (popUp.yCooardinate)) && (positionY <= (popUp.yCooardinate + popUpSize)) ||                         //right top corner is in a ohter Popup
                (positionX + popUpSize >= popUp.xCooardinate) && (positionX + popUpSize <= (popUp.xCooardinate + popUpSize)) && (positionY + popUpSize >= (popUp.yCooardinate)) && (positionY + popUpSize <= (popUp.yCooardinate + popUpSize)))
                return true;

            return false;
        }

        private void checkCollision()
        {
            while (mw.getIsPlaying() && Thread.CurrentThread.ThreadState.Equals(ThreadState.Running))
            {
                //Ball out of PopUp Ground Borders
                if (ballControl.currentXPosition > popUpAreaBorders.minXCooardinate && ballControl.currentXPosition < popUpAreaBorders.maxXCooardinate &&
                    ballControl.currentYPosition > popUpAreaBorders.minYCooardinate && ballControl.currentYPosition < popUpAreaBorders.maxYCooardinate)
                {
                    try
                    {
                        lock (popUps)
                        {
                            int idx = 0;
                            bool found = false;
                            foreach (PopUp popUp in popUps)
                            {
                                if (checkConntectWithPopUpElement(ballControl, popUp))
                                {
                                    found = true;
                                    break;
                                }
                                else
                                    idx++;
                            }

                            if (found)
                            {
                                doPopUpFunction(popUps.ElementAt(idx).id, directionisLeft);

                                if (popUps.Count > 0)
                                {
                                    mw.Playground.Dispatcher.Invoke(new Action(() => { mw.Playground.Children.Remove(popUps.ElementAt(idx).element); }));
                                    popUps.RemoveAt(idx);
                                }
                            }
                        }
                    }

                    catch (Exception)
                    {

                    }
                }
            }
        }

        //The diagonal get calculated with the Pythagoras, if the diagonal is smaller than the radius of the ball + the radius of the element
        //then a collision has occured.
        private bool checkConntectWithPopUpElement(BallControl ball, PopUp element)
        {
            double xPopUp = element.xCooardinate + element.width / 2;
            double yPopUp = element.yCooardinate + element.height / 2;

            double deltaX = (ball.currentXPosition - xPopUp) >= 0 ? (ball.currentXPosition - xPopUp) : (xPopUp - ball.currentXPosition);
            double deltaY = (ball.currentYPosition - yPopUp) >= 0 ? (ball.currentYPosition - yPopUp) : (yPopUp - ball.currentYPosition);

            double diagonal = Math.Sqrt(
                Math.Pow(deltaX, 2) +
                Math.Pow(deltaY, 2));
            return diagonal <= (ball.width / 2 + element.width / 2);
        }

        private void doPopUpFunction(int id, bool isCurentDirectionLeft, bool runThread = true)
        {
            Thread thread = null;

            switch (id)
            {
                #region //green plus
                case 1:
                    if (!isCurentDirectionLeft)
                        lock (mw.Slider_Player1)
                            mw.Slider_Player1.Dispatcher.Invoke(new Action(() => { PopUpControl.MakeSliderBigger(mw.Slider_Player1, mw.Playground_Slider1.Height); }));

                    else
                        lock (mw.Slider_Player2)
                            mw.Slider_Player2.Dispatcher.Invoke(new Action(() => { PopUpControl.MakeSliderBigger(mw.Slider_Player2, mw.Playground_Slider2.Height); }));

                    thread = new Thread(() => runPopUpThread(1, directionisLeft));
                    break;
                #endregion

                #region//red plus
                case 2:
                    if (!isCurentDirectionLeft)
                        lock (mw.Slider_Player2)
                            mw.Slider_Player2.Dispatcher.Invoke(new Action(() => { PopUpControl.MakeSliderBigger(mw.Slider_Player2, mw.Playground_Slider2.Height); }));

                    else
                        lock (mw.Slider_Player1)
                            mw.Slider_Player1.Dispatcher.Invoke(new Action(() => { PopUpControl.MakeSliderBigger(mw.Slider_Player1, mw.Playground_Slider1.Height); }));

                    thread = new Thread(() => runPopUpThread(2, directionisLeft));
                    break;
                #endregion

                #region//green minus
                case 3:
                    if (!isCurentDirectionLeft)
                        lock (mw.Slider_Player2)
                            mw.Slider_Player2.Dispatcher.Invoke(new Action(() => { PopUpControl.MakeSliderSmaller(mw.Slider_Player2, mw.Playground_Slider2.Height); }));

                    else
                        lock (mw.Slider_Player1)
                            mw.Slider_Player1.Dispatcher.Invoke(new Action(() => { PopUpControl.MakeSliderSmaller(mw.Slider_Player1, mw.Playground_Slider1.Height); }));
                    thread = new Thread(() => runPopUpThread(3, directionisLeft));
                    break;
                #endregion

                #region//red minus
                case 4:
                    if (!isCurentDirectionLeft)
                        lock (mw.Slider_Player1)
                            mw.Slider_Player1.Dispatcher.Invoke(new Action(() => { PopUpControl.MakeSliderSmaller(mw.Slider_Player1, mw.Playground_Slider1.Height); }));

                    else
                        lock (mw.Slider_Player2)
                            mw.Slider_Player2.Dispatcher.Invoke(new Action(() => { PopUpControl.MakeSliderSmaller(mw.Slider_Player2, mw.Playground_Slider2.Height); }));

                    thread = new Thread(() => runPopUpThread(4, directionisLeft));
                    break;
                #endregion

                #region//faster
                case 5:
                    PopUpControl.MakeBallFaster(ref ballControl);
                    thread = new Thread(() => runPopUpThread(5, directionisLeft));
                    break;
                #endregion

                #region//slower
                case 6:
                    PopUpControl.MakeBallSlower(ref ballControl);
                    thread = new Thread(() => runPopUpThread(6, directionisLeft));
                    break;
                #endregion

                #region //clear
                case 7:
                    PopUpControl.ClearPopUps(mw.Playground, popUps);
                    runThread = false;
                    break;
                #endregion

                #region //questionmark
                case 8:
                    doPopUpFunction(rnd.Next(1, id), isCurentDirectionLeft);
                    break;
                    #endregion
            }

            if (runThread && thread != null)
            {
                popUpThreads.Add(thread);
                thread.Name = "Thread_" + threadCounter;
                threadCounter++;
                thread.Start();
            }
        }

        private void runPopUpThread(int id, bool isCurentDirectionLeft)
        {
            while (mw.getIsPlaying() && Thread.CurrentThread.ThreadState.Equals(ThreadState.Running))
            {
                Thread.Sleep(popUpActiveTime);
                switch (id)
                {
                    case 1:
                        doPopUpFunction(4, isCurentDirectionLeft, false); //red minus. My Slider is getting smaller.
                        break;

                    case 2:
                        doPopUpFunction(3, isCurentDirectionLeft, false); //gree minus. Slider of the opponent getting smaller.
                        break;

                    case 3:
                        doPopUpFunction(2, isCurentDirectionLeft, false); //red plus. Slider of opponent is getting bigger.
                        break;

                    case 4:
                        doPopUpFunction(1, isCurentDirectionLeft, false); //green plus. My Slider is getting bigger.
                        break;

                    case 5:
                        doPopUpFunction(6, isCurentDirectionLeft, false); //slower. The ball is getting slower.
                        break;

                    case 6:
                        doPopUpFunction(5, isCurentDirectionLeft, false); //faster. The ball is getting faster.
                        break;
                }
                popUpThreads.Remove(Thread.CurrentThread);
                Thread.CurrentThread.Abort();
            }
        }

        public void readDataFromArduino()
        {
            string value;
            while (mw.getIsPlaying())
            {
                value = mw.getArduinoController().returnValue();
                if (value != null)
                    moveSlider(value.Trim());
            }
        }

        private void moveSlider(string value)
        {
            string[] values = value.Split(':');
            int moving = int.Parse(values[1]); ;

            if (values[0].Trim() == "JoyStick1_Y_axis")
            {
                //move Slider for Player1
                if (moving < 0)
                    sliderDown(mw.Slider_Player1);

                else if (moving > 0)
                    sliderUp(mw.Slider_Player1);
            }

            else if (values[0].Trim() == "JoyStick2_Y_axis")
            {
                //move Slider for Player2
                if (moving < 0)
                    sliderDown(mw.Slider_Player2);

                else if (moving > 0)
                    sliderUp(mw.Slider_Player2);
            }
        }

        public void runPlayerOneKeys()
        {
            while (mw.getIsPlaying() && Thread.CurrentThread.ThreadState.Equals(ThreadState.Running))
            {
                Thread.Sleep(30);
                PlayerControl player = mw.getControlPlayer1();

                if (player.up)
                    sliderUp(mw.Slider_Player1);

                if (player.down)
                    sliderDown(mw.Slider_Player1);
            }
        }

        public void runPlayerTwoKeys()
        {
            while (mw.getIsPlaying() && Thread.CurrentThread.ThreadState.Equals(ThreadState.Running))
            {
                Thread.Sleep(30);
                PlayerControl player = mw.getControlPlayer2();

                if (player.up)
                    sliderUp(mw.Slider_Player2);

                if (player.down)
                    sliderDown(mw.Slider_Player2);
            }
        }

        public int getRandomForKI()
        {
            return rnd.Next(1, 3);
        }

        public void runKI()
        {
            // If you want to change the "difficulty" of the KI, change distanceMidAndImpactingPoint, the higher the value is the bigger will be the angle of the ball

            int distanceMidAndImpactingPoint = 35;

            int rndNumber = getRandomForKI();

            int heightBall = 0;
            int heightSlider = 0;

            while (mw.getIsPlaying() && Thread.CurrentThread.ThreadState.Equals(ThreadState.Running))
            {
                Thread.Sleep(1);

                if (directionChanged == true)
                {
                    rndNumber = getRandomForKI();
                    directionChanged = false;
                }

                ballControl.ball.Dispatcher.InvokeAsync(
                     new Action(() =>
                     {
                         heightBall = Convert.ToInt32(Canvas.GetTop(mw.Ball));
                         mw.Dispatcher.InvokeAsync(
                         new Action(() =>
                         {
                             heightSlider = Convert.ToInt32(Canvas.GetTop(mw.Slider_Player2));

                             if (rndNumber == 1)
                             {
                                 if (heightSlider < (heightBall - differenceTopBallSlider) + distanceMidAndImpactingPoint)
                                 {
                                     sliderDownKI(mw.Slider_Player2);
                                 }
                                 else
                                 {
                                     sliderUpKI(mw.Slider_Player2);
                                 }
                             }
                             else
                             {
                                 if (heightSlider < (heightBall - differenceTopBallSlider) - distanceMidAndImpactingPoint)
                                 {
                                     sliderDownKI(mw.Slider_Player2);
                                 }
                                 else
                                 {
                                     sliderUpKI(mw.Slider_Player2);
                                 }
                             }
                         }));
                     }));
            }
        }

        public void ballMove()
        {
            //setBallInMiddleOfPlayground();
            while (mw.getIsPlaying() && Thread.CurrentThread.ThreadState.Equals(ThreadState.Running))
            {
                Thread.Sleep(ballControl.sleepingTime);
                ballControl.ball.Dispatcher.InvokeAsync(
                    new Action(() =>
                    {
                        if (mw.getIsPlaying() && Thread.CurrentThread.ThreadState.Equals(ThreadState.Running))
                        {
                            if (directionisLeft)
                                moveBall(ref ballControl, mw.Slider_Player1);

                            else
                                moveBall(ref ballControl, mw.Slider_Player2);
                        }
                    })
                    );
            }
        }

        public void ballMoveKI()
        {
            // If you want to change the setting change threshSleep as the start speed an "sec % value" at which interval the speed should be increased

            int threadSleep = 10;
            int sec = 0;

            bool sequenzDone = false;

            while (mw.getIsPlaying() && Thread.CurrentThread.ThreadState.Equals(ThreadState.Running))
            {
                Thread.Sleep(threadSleep);

                mw.Dispatcher.InvokeAsync(
                    new Action(() =>
                    {
                        string[] split = Convert.ToString(mw.TF_Timer.Content).Split(':');

                        sec = (Convert.ToInt32(split[0]) * 60) + Convert.ToInt32(split[1]);
                        if (sec % 5 == 0)
                        {
                            if (sequenzDone == false)
                            {
                                if (threadSleep > 2)
                                    threadSleep -= 1;

                                sequenzDone = true;
                            }
                        }
                        else
                        {
                            sequenzDone = false;
                        }
                    }));

                ballControl.ball.Dispatcher.InvokeAsync(
                    new Action(() =>
                    {
                        if (mw.getIsPlaying() && Thread.CurrentThread.ThreadState.Equals(ThreadState.Running))
                        {
                            if (directionisLeft)
                                moveBall(ref ballControl, mw.Slider_Player1);

                            else
                                moveBall(ref ballControl, mw.Slider_Player2);
                        }
                    })
                    );
            }
        }

        private void moveBall(ref BallControl ballControl, Rectangle slider)
        {
            bool bySlider = false;

            ballControl.currentXPosition += ballControl.movingXDistance;
            ballControl.currentYPosition += ballControl.movingYDistance;

            if (ballControl.currentXPosition - ballControl.width / 2 < slider.Width)
            {
                ballControl.currentXPosition = (slider.Width + ballControl.width / 2);
                Canvas.SetLeft(ballControl.ball, slider.Width);
                bySlider = true;
            }

            else if (ballControl.currentXPosition + ballControl.width / 2 > (mw.Playground.Width - slider.Width))
            {
                ballControl.currentXPosition = (mw.Playground.Width - slider.Width - ballControl.width / 2);
                Canvas.SetLeft(ballControl.ball, ballControl.currentXPosition - ballControl.width / 2);
                bySlider = true;
            }

            else
            {
                Canvas.SetLeft(ballControl.ball, ballControl.currentXPosition - ballControl.width / 2);
                Canvas.SetTop(ballControl.ball, ballControl.currentYPosition - ballControl.height / 2);
            }


            if (bySlider)
            {
                bool contact = checkContactWithSlider();
                if (!contact)
                {
                    int player = (directionisLeft) ? 1 : 2;
                    mw.noContactWithBallAndPlayer(player);
                }

                else
                    ballControl.movingYDistance = getYAxisForBall(slider, ballControl.ball, ballControl.movingXDistance);

                directionisLeft = (directionisLeft) ? false : true;
                ballControl.movingXDistance *= -1;

            }

            if (checkContactWithBorder())
                ballControl.movingYDistance *= -1;
        }

        private void sliderUp(Rectangle slider)
        {
            slider.Dispatcher.InvokeAsync(
            new Action(() =>
            {
                if (Canvas.GetTop(slider) >= 5)
                    Canvas.SetTop(slider, Canvas.GetTop(slider) - 5);

                else
                    Canvas.SetTop(slider, 0);

            })
            );
        }

        private void sliderDown(Rectangle slider)
        {
            slider.Dispatcher.InvokeAsync(
            new Action(() =>
            {
                if (Canvas.GetTop(slider) < (mw.Playground.Height - slider.Height - 5))
                    Canvas.SetTop(slider, Canvas.GetTop(slider) + 5);

                else
                    Canvas.SetTop(slider, mw.Playground.Height - slider.Height);
            })
            );
        }

        private void sliderUpKI(Rectangle slider)
        {
            slider.Dispatcher.InvokeAsync(
            new Action(() =>
            {
                if (Canvas.GetTop(slider) >= 1)
                    Canvas.SetTop(slider, Canvas.GetTop(slider) - 1);

                else
                    Canvas.SetTop(slider, 0);

            })
            );
        }

        private void sliderDownKI(Rectangle slider)
        {
            slider.Dispatcher.InvokeAsync(
            new Action(() =>
            {
                if (Canvas.GetTop(slider) < (mw.Playground.Height - slider.Height - 1))
                    Canvas.SetTop(slider, Canvas.GetTop(slider) + 1);

                else
                    Canvas.SetTop(slider, mw.Playground.Height - slider.Height);
            })
            );
        }

        private double getYAxisForBall(Rectangle slider, Ellipse ball, int x)
        {
            //X ... steps = 5
            double y = 0;
            double angleOfImpact = getAngleOfImpact(ball, slider);
            angleOfImpact *= Math.PI / 180;
            y = x * Math.Tan(angleOfImpact);
            y = (!directionisLeft) ? y * -1 : y;

            return y;
        }

        private int getMiddleOfBall(Ellipse ball)
        {
            return (int)(Canvas.GetTop(ball) + ball.Height / 2);
        }

        private int getMiddleOfSlider(Rectangle slider)
        {
            return (int)(Canvas.GetTop(slider) + slider.Height / 2);
        }

        private double getAngleOfImpact(Ellipse ball, Rectangle slider)
        {
            //Schlussrechnung ausbaufähig
            return (getMiddleOfBall(ball) - getMiddleOfSlider(slider)) * maxAngle / (slider.Height) * -1;
        }

        private bool checkContactWithSlider()
        {
            bool contact = false;
            if (directionisLeft)
            {
                mw.Slider_Player1.Dispatcher.Invoke(
                    new Action(() =>
                    {
                        if (!isBallOverSlider(mw.Slider_Player1, ballControl.ball) && !isBallUnderSlider(mw.Slider_Player1, ballControl.ball))
                            contact = true;
                    })
                );
            }

            if (!directionisLeft)
            {
                mw.Slider_Player2.Dispatcher.Invoke(
                    new Action(() =>
                    {
                        if (!isBallOverSlider(mw.Slider_Player2, ballControl.ball) && !isBallUnderSlider(mw.Slider_Player2, ballControl.ball))
                            contact = true;
                    })
                );
            }

            return contact;
        }

        private bool checkContactWithBorder()
        {
            //First expression under or on Bottom of Playground
            //Second expression over or on Top of Playground
            return (Canvas.GetTop(mw.Ball) + mw.Ball.Height >= mw.Playground.Height || (Canvas.GetTop(mw.Ball) <= 0)) ? true : false;
        }

        private bool isBallOverSlider(Rectangle slider, Ellipse ball)
        {
            bool over = true;

            if (Canvas.GetTop(slider) <= (Canvas.GetTop(ball) + ball.Height))
                over = false;

            return over;
        }

        private bool isBallUnderSlider(Rectangle slider, Ellipse ball)
        {
            bool under = true;

            if (Canvas.GetTop(slider) + slider.Height >= (Canvas.GetTop(ball)))
                under = false;

            return under;
        }

        public void setBallInMiddleOfPlayground()
        {
            ballControl.ball.Dispatcher.Invoke(
            new Action(() =>
            {
                ballControl.currentXPosition = mw.Playground.Width / 2;
                ballControl.currentYPosition = mw.Playground.Height / 2;
                Canvas.SetLeft(ballControl.ball, ballControl.currentXPosition - ballControl.width / 2);
                Canvas.SetTop(ballControl.ball, ballControl.currentYPosition - ballControl.height / 2);
            }));
        }

        public void resetPlaygroud()
        {
            PopUpControl.ClearPopUps(mw.Playground, popUps);
            stopPopUpThreads();
            popUpThreads.Clear();

            PopUpControl.ResetBallSpeed(ref ballControl);
            PopUpControl.ResetSliderHeight(mw.Slider_Player1, mw.Playground_Slider1.Height);
            PopUpControl.ResetSliderHeight(mw.Slider_Player2, mw.Playground_Slider2.Height);
            setBallInMiddleOfPlayground();
            //directionisLeft = directionisLeft ? directionisLeft = false : directionisLeft = true;
            ballControl.movingYDistance = 0;
        }

        public void stopPopUpThreads()
        {
            foreach (Thread thread in popUpThreads)
            {
                if (thread != null && thread.IsAlive)
                    thread.Abort();
            }
        }

        public void interruptThreads()
        {
            foreach (Thread thread in popUpThreads)
            {
                if (thread != null && thread.IsAlive)
                    thread.Join();
            }
        }

        public void runThreads()
        {
            foreach (Thread thread in popUpThreads)
            {
                if (thread != null && thread.ThreadState.Equals(ThreadState.Stopped))
                    thread.Start();
            }
        }

        public void stopPopUpTimer()
        {
            if (popUpCreationThread.IsAlive)
            {
                popUpCreationThread.Abort();
                collisionThread.Abort();
            }
        }

        public void startPopUpTimer()
        {
            if (!popUpCreationThread.IsAlive)
            {
                popUpCreationThread = new Thread(createPopUp);
                popUpCreationThread.SetApartmentState(ApartmentState.STA);
                collisionThread = new Thread(checkCollision);
                popUpCreationThread.Start();
                collisionThread.Start();
            }
        }
    }
}
