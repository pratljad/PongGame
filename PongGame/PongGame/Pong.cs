﻿using System;
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
                valuePlayerTwo = 500;

                if (value != null)
                {
                    string[] values = value.Split(':');

                    if (values[0] == "JoyStick1_Y_axis")
                    {
                        valuePlayerOne = int.Parse(values[1]);
                    }

                    if (valuePlayerOne < 475)
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
                    else if (valuePlayerOne > 535)
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
                valuePlayerOne = 500;
                valuePlayerTwo = 500;

                if (value != null)
                {
                    string[] values = value.Split(':');

                    if (values[0] == "JoyStick2_Y_axis")
                    {
                        valuePlayerTwo = int.Parse(values[1]);
                    }

                    if (valuePlayerTwo < 475)
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
                    else if (valuePlayerTwo > 535)
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
                }
            }
        }

        /*
        public void runPongGame(int player)
        {
            while (mw.getIsPlaying())
            {
                Thread.Sleep(100);

                value = mw.getArduinoController().returnValue();

                // Standard setzen
                valuePlayerOne = 500;
                valuePlayerTwo = 500;

                if (value != null)
                {
                    string[] values = value.Split(':');

                    if (values[0] == "JoyStick1_Y_axis")
                    {
                        valuePlayerOne = int.Parse(values[1]);
                    }
                    else if (values[0] == "JoyStick2_Y_axis")
                    {
                        valuePlayerTwo = int.Parse(values[1]);
                    }
                }

                if (player == 1)
                {
                    if (valuePlayerOne < 475)
                    {
                        mw.Playground_Slider1.Dispatcher.BeginInvoke(
                            new Action(() =>
                            {
                                if (Canvas.GetTop(mw.Slider_Player1) > 0)
                                {
                                    Canvas.SetTop(mw.Slider_Player1, Canvas.GetTop(mw.Slider_Player1) - 5);
                                }
                            })
                        );
                    }
                    else if (valuePlayerOne > 535)
                    {
                        mw.Playground_Slider1.Dispatcher.BeginInvoke(
                            new Action(() =>
                            {
                                if (Canvas.GetTop(mw.Slider_Player1) < mw.Playground_Slider1.Height - mw.Slider_Player1.Height)
                                {
                                    Canvas.SetTop(mw.Slider_Player1, Canvas.GetTop(mw.Slider_Player1) + 5);
                                }
                            })
                        );
                    }
                }
                else if (player == 2)
                {
                    if (valuePlayerTwo < 475)
                    {
                        mw.Playground_Slider2.Dispatcher.BeginInvoke(
                            new Action(() =>
                            {
                                if (Canvas.GetTop(mw.Slider_Player2) > 0)
                                {
                                    Canvas.SetTop(mw.Slider_Player2, Canvas.GetTop(mw.Slider_Player2) - 5);
                                }
                            })
                        );
                    }
                    else if (valuePlayerTwo > 535)
                    {
                        mw.Playground_Slider2.Dispatcher.BeginInvoke(
                            new Action(() =>
                            {
                                if (Canvas.GetTop(mw.Slider_Player2) < mw.Playground_Slider2.Height - mw.Slider_Player2.Height)
                                {
                                    Canvas.SetTop(mw.Slider_Player2, Canvas.GetTop(mw.Slider_Player2) + 5);
                                }
                            })
                        );
                    }
                }
            }
        }*/
    }
}
