using System;
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
        int valuePlayerOne = 505;
        int valuePlayerTwo = 505;
        string value;
        public bool directionisLeft = false;
        private double yAxis;
        private int maxAngle = 30;

        public Pong(MainWindow mw)
        {
            this.mw = mw;
        }

        public void runPlayerOne()
        {
            while (mw.getIsPlaying())
            {
                Thread.Sleep(100);

                value = mw.getArduinoController().returnValue();

                // Standard setzen
                valuePlayerOne = 500;

                if (value != null && value.Trim() != "")
                {
                    Console.WriteLine("Value: " +value);

                    string[] values = value.Split(':');

                    

                    if (int.Parse(values[1].Trim()) < valuePlayerOne -20 || int.Parse(values[1].Trim()) > valuePlayerOne + 20)
                    {
                        if (values[0].Trim() == "JoyStick1_Y_axis")
                        {
                            valuePlayerOne = int.Parse(values[1]);
                        }
                    }
                    else
                    {
                        valuePlayerOne = 500;
                    }

                    if (valuePlayerOne < 400)
                    {
                        mw.Playground_Slider1.Dispatcher.InvokeAsync(
                            new Action(() =>
                            {
                                if (Canvas.GetTop(mw.Slider_Player1) < mw.Playground_Slider1.Height - mw.Slider_Player1.Height)
                                {
                                    Canvas.SetTop(mw.Slider_Player1, Canvas.GetTop(mw.Slider_Player1) + 5);
                                }
                            })
                        );
                    }
                    else if (valuePlayerOne > 600)
                    {
                        mw.Playground_Slider1.Dispatcher.InvokeAsync(
                            new Action(() =>
                            {
                                if (Canvas.GetTop(mw.Slider_Player1) > 0)
                                {
                                    Canvas.SetTop(mw.Slider_Player1, Canvas.GetTop(mw.Slider_Player1) - 5);
                                }
                            })
                        );
                    }
                }     
            }
        }

        public void runPlayerTwo()
        {
            while (mw.getIsPlaying())
            {
                Thread.Sleep(100);

                value = mw.getArduinoController().returnValue();

                // Standard setzen
                valuePlayerTwo = 500;

                if (value != null)
                {
                    string[] values = value.Split(':');
                    

                    if (int.Parse(values[1]) < valuePlayerTwo -20 || int.Parse(values[1]) > valuePlayerTwo + 20)
                    {
                        if (values[0] == "JoyStick2_Y_axis")
                        {
                            valuePlayerTwo = int.Parse(values[1]);
                        }
                    }
                    else
                    {
                        valuePlayerTwo = 500;
                    }

                    if (valuePlayerTwo < 400)
                    {
                        mw.Playground_Slider2.Dispatcher.InvokeAsync(
                            new Action(() =>
                            {
                                if (Canvas.GetTop(mw.Slider_Player2) < mw.Playground_Slider2.Height - mw.Slider_Player2.Height)
                                {
                                    Canvas.SetTop(mw.Slider_Player2, Canvas.GetTop(mw.Slider_Player2) + 5);
                                }
                            })
                        );
                    }
                    else if (valuePlayerTwo > 600)
                    {
                        mw.Playground_Slider2.Dispatcher.InvokeAsync(
                            new Action(() =>
                            {
                                if (Canvas.GetTop(mw.Slider_Player2) > 0)
                                {
                                    Canvas.SetTop(mw.Slider_Player2, Canvas.GetTop(mw.Slider_Player2) - 5);
                                }
                            })
                        );
                    }
                }
            }
        }

        public void runPlayerOneKeys()
        {
            while (mw.getIsPlaying() && Thread.CurrentThread.ThreadState.Equals(ThreadState.Running))
            {
                Thread.Sleep(30);
                ControlePlayer player = mw.getControlPlayer1();

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
                ControlePlayer player = mw.getControlPlayer2();

                if (player.up)
                    sliderUp(mw.Slider_Player2);

                if (player.down)
                    sliderDown(mw.Slider_Player2);
            }
        }

        public void ballMove()
        {
            while (mw.getIsPlaying() && Thread.CurrentThread.ThreadState.Equals(ThreadState.Running))
            {
                Thread.Sleep(10);
                mw.Ball.Dispatcher.InvokeAsync(
                    new Action(() =>
                    {
                        if (mw.getIsPlaying() && Thread.CurrentThread.ThreadState.Equals(ThreadState.Running))
                        {
                            if (directionisLeft)
                            {
                                Canvas.SetLeft(mw.Ball, Canvas.GetLeft(mw.Ball) - 5);
                                Canvas.SetTop(mw.Ball, Canvas.GetTop(mw.Ball) - yAxis);
                                if (Canvas.GetLeft(mw.Ball) < mw.Slider_Player1.Width)
                                {
                                    Canvas.SetLeft(mw.Ball, mw.Slider_Player1.Width);
                                    bool contact = checkContactWithSlider();
                                    if (!contact)
                                        mw.noContactWithBallAndPlayer(1);

                                    else
                                        yAxis = getYAxisForBall(mw.Slider_Player1, mw.Ball, 5);

                                    directionisLeft = false;
                                }

                                if (checkContactWithBorder())
                                    yAxis *= -1;
                            }

                            else
                            {
                                Canvas.SetLeft(mw.Ball, Canvas.GetLeft(mw.Ball) + 5);
                                Canvas.SetTop(mw.Ball, Canvas.GetTop(mw.Ball) - yAxis);
                                if (Canvas.GetLeft(mw.Ball) > (mw.Playground.Width - mw.Slider_Player2.Width - mw.Ball.Width))
                                {
                                    Canvas.SetLeft(mw.Ball, (mw.Playground.Width - mw.Slider_Player2.Width - mw.Ball.Width));
                                    
                                    bool contact = checkContactWithSlider();
                                    if (!contact)
                                        mw.noContactWithBallAndPlayer(2);

                                    else
                                        yAxis = getYAxisForBall(mw.Slider_Player2, mw.Ball, 5);

                                    directionisLeft = true;
                                }

                                if (checkContactWithBorder())
                                    yAxis *= -1;

                            }
                        }
                    })
                    );
            }
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

        private double getYAxisForBall(Rectangle slider, Ellipse ball, int x)
        {
            //X ... steps = 5
            double y = 0;
            double angleOfImpact = getAngleOfImpact(ball, slider);
            angleOfImpact *= Math.PI / 180;
            y = x * Math.Tan(angleOfImpact);

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
            //Schlussrechnung
            double angleOfImpact = (getMiddleOfBall(ball)- Canvas.GetTop(slider)) * maxAngle / (slider.Height);
            return (getMiddleOfBall(ball) < getMiddleOfSlider(slider)) ? angleOfImpact : angleOfImpact*-1;
        }

        private bool checkContactWithSlider()
        {
            bool contact = false;
            if (directionisLeft)
            {
                mw.Slider_Player1.Dispatcher.Invoke(
                    new Action(() =>
                    {
                        if (!isBallOverSlider(mw.Slider_Player1, mw.Ball) && !isBallUnderSlider(mw.Slider_Player1, mw.Ball))
                            contact = true;
                    })
                );
            }

            if (!directionisLeft)
            {
                mw.Slider_Player2.Dispatcher.Invoke(
                    new Action(() =>
                    {
                        if (!isBallOverSlider(mw.Slider_Player2, mw.Ball) && !isBallUnderSlider(mw.Slider_Player2, mw.Ball))
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
