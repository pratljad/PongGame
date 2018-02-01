using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace PongGame
{
    public static class PopUpControl
    {
        private static double multiplierSlider = 0.3;
        private static int mulitplierBall = 2;
        private static int fasterSlowerIdx = 0;
        private static bool biggerOrSmaller = false;
        private static double standardSliderHeight = 0;

        public static void MakeSliderBigger(Rectangle slider, double heightOfSliderPlayground)
        {
            if (!biggerOrSmaller)
                standardSliderHeight = slider.Height;

            biggerOrSmaller = true;

            bool onTop = false, onBottom = false, makeBigger = true;
            double dif = slider.Height * (1 + multiplierSlider) - slider.Height;

            if ((Canvas.GetTop(slider) - (dif / 2)) < 0)
                onTop = true;

            if ((Canvas.GetTop(slider) + slider.Height + (dif / 2) > heightOfSliderPlayground))
            {
                if (!onTop)
                    onBottom = true;

                else
                    makeBigger = false;
            }

            if (makeBigger)
            {
                slider.Height += dif;

                if (onTop)
                    Canvas.SetTop(slider, 0);

                else if (onBottom)
                    Canvas.SetTop(slider, heightOfSliderPlayground - slider.Height);

                else
                    Canvas.SetTop(slider, (Canvas.GetTop(slider) - (dif / 2)));
            }
        }

        public static void MakeSliderSmaller(Rectangle slider, double heightOfSliderPlayground)
        {
            if (!biggerOrSmaller)
                standardSliderHeight = slider.Height;

            biggerOrSmaller = true;

            double dif = slider.Height - slider.Height / (1 + multiplierSlider);
            slider.Height -= dif;
            Canvas.SetTop(slider, Canvas.GetTop(slider) + (dif / 2));
        }

        public static void MakeBallFaster(ref BallControl ballControl)
        {
            if (fasterSlowerIdx < 3)
            {
                //ballControl.sleepingTime = Int32.Parse(((ballControl.sleepingTime / (1+mulitplierBall)).ToString().Split(',')[0]));
                ballControl.sleepingTime -= mulitplierBall;
                fasterSlowerIdx++;
            }
        }

        public static void MakeBallSlower(ref BallControl ballControl)
        {
            if (fasterSlowerIdx > -3)
            {
                //ballControl.sleepingTime = Int32.Parse(((ballControl.sleepingTime * (1+mulitplierBall)).ToString().Split(',')[0]));
                ballControl.sleepingTime += mulitplierBall;
                fasterSlowerIdx--;
            }
        }

        public static void ClearPopUps(Canvas playground, List<PopUp> popUps)
        {
            int idx = 0;
            foreach (PopUp popUp in popUps)
            {
                playground.Dispatcher.Invoke(
                new Action(() =>
                {
                    playground.Children.Remove(popUps.ElementAt(idx).element);
                }));
                idx++;
            }

            popUps.Clear();
        }

        public static void ResetBallSpeed(ref BallControl ballControl)
        {
            if (fasterSlowerIdx != 0)
            {
                while (fasterSlowerIdx < 0)
                {
                    MakeBallFaster(ref ballControl);
                    fasterSlowerIdx++;
                }

                while (fasterSlowerIdx > 0)
                {
                    MakeBallSlower(ref ballControl);
                    fasterSlowerIdx--;
                }
            }
        }

        public static void ResetSliderHeight(Rectangle slider, double heightOfSliderPlayground)
        {
            if (biggerOrSmaller)
            {
                while (slider.Height < standardSliderHeight)
                    MakeSliderBigger(slider, heightOfSliderPlayground);

                while (slider.Height > standardSliderHeight)
                    MakeSliderSmaller(slider, heightOfSliderPlayground);
            }
        }
    }
}
