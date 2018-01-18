using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PongGame
{
    public class Pong
    {
        private MainWindow mw;
        private BallControl ballControl;

        public bool directionisLeft = false;
        private int maxAngle = 50;

        private bool directionChanged = false;
        Random rnd = new Random();

        int differenceTopBallSlider = 0;

        public Pong(MainWindow mw)
        {
            this.mw = mw;
            ballControl = new BallControl(mw.Ball, 5, 0);

            mw.Dispatcher.InvokeAsync(
                         new Action(() =>
                         {
                             differenceTopBallSlider = Convert.ToInt32(Canvas.GetTop(mw.Ball)) - Convert.ToInt32(Canvas.GetTop(mw.Slider_Player1));
                         }));
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
                    if (directionisLeft == false)
                    {
                        // Ball goes right
                        rndNumber = getRandomForKI();
                    }
                    else if (directionisLeft == true)
                    {
                        // Ball goes left
                        rndNumber = getRandomForKI();
                    }

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

                             if(rndNumber == 1)
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
            while (mw.getIsPlaying() && Thread.CurrentThread.ThreadState.Equals(ThreadState.Running))
            {
                Thread.Sleep(10);
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
                        if(sec % 5 == 0)
                        {
                            if(sequenzDone == false)
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

            Canvas.SetLeft(ballControl.ball, Canvas.GetLeft(ballControl.ball) + ballControl.movingXDistance);
            Canvas.SetTop(ballControl.ball, Canvas.GetTop(ballControl.ball) + ballControl.movingYDistance);

            if (Canvas.GetLeft(ballControl.ball) < slider.Width)
            {
                Canvas.SetLeft(ballControl.ball, slider.Width);
                bySlider = true;
            }

            if (Canvas.GetLeft(ballControl.ball) > (mw.Playground.Width - slider.Width - ballControl.ball.Width))
            {
                Canvas.SetLeft(ballControl.ball, (mw.Playground.Width - slider.Width - ballControl.ball.Width));
                bySlider = true;
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
                directionChanged = true;
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
            return (getMiddleOfBall(ball) - getMiddleOfSlider(slider)) * maxAngle / (slider.Height)*-1;
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
    }
}
