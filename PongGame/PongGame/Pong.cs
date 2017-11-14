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

                if (value != null)
                {
                    string[] values = value.Split(':');

                    if (int.Parse(values[1]) < valuePlayerOne -20 || int.Parse(values[1]) > valuePlayerOne + 20)
                    {
                        if (values[0] == "JoyStick1_Y_axis")
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
    }
}
