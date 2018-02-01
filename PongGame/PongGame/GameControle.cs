using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PongGame
{
    public static class GameControle
    {
        private static string relativeImagePath = "../../../Images/";
        private static bool gameRunning = false;

        public static string getAbsoluteImagePath()
        {
            return Path.GetFullPath(relativeImagePath);
        }

        public static void gameStarted()
        {
            gameRunning = true;
        }

        public static void gameStoped()
        {
            gameRunning = false;
        }

        public static bool isAGameRunning()
        {
            return gameRunning;
        }

        public static ImageSource getIcon()
        {
            return BitmapFrame.Create(new Uri(getAbsoluteImagePath() + "Icon.ico", UriKind.Absolute));
        }

        public static ImageSource getImageFigurePlayerOne()
        {
            return new BitmapImage(new Uri(getAbsoluteImagePath() + "Goku.png", UriKind.Absolute));
        }

        public static ImageSource getImageFigurePlayerTwo()
        {
            return new BitmapImage(new Uri(getAbsoluteImagePath() + "Chara.png", UriKind.Absolute));
        }

        public static ImageSource getImageControllerOne()
        {
            return new BitmapImage(new Uri(getAbsoluteImagePath() + "Joystick.png", UriKind.Absolute));
        }

        public static ImageSource getImageControllerTwo()
        {
            return new BitmapImage(new Uri(getAbsoluteImagePath() + "Controller2.png", UriKind.Absolute));
        }

        public static ImageSource getImageKI()
        {
            return new BitmapImage(new Uri(getAbsoluteImagePath() + "KI.png", UriKind.Absolute));
        }

        public static ImageSource getImageUser()
        {
            return new BitmapImage(new Uri(getAbsoluteImagePath() + "User.png", UriKind.Absolute));
        }

        public static ImageSource getImagePlayerOne()
        {
            return new BitmapImage(new Uri(getAbsoluteImagePath() + "Player1.png", UriKind.Absolute));
        }

        public static ImageSource getImagePlayerTwo()
        {
            return new BitmapImage(new Uri(getAbsoluteImagePath() + "Player2.png", UriKind.Absolute));
        }

        public static ImageSource getImagePlayground()
        {
            return new BitmapImage(new Uri(getAbsoluteImagePath() + "PongField.jpg", UriKind.Absolute));
        }

        public static ImageSource getImagePlaygroundLeft()
        {
            return new BitmapImage(new Uri(getAbsoluteImagePath() + "PongFieldLeft.jpg", UriKind.Absolute));
        }

        public static ImageSource getImagePlaygroundRight()
        {
            return new BitmapImage(new Uri(getAbsoluteImagePath() + "PongFieldRight.jpg", UriKind.Absolute));
        }
    }
}
