using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PongGame
{
    public class Player
    {
        private int _points;
        private string _nickname;
        private Color _color;
        private int _won;

        public Player(string nickname, Color color)
        {
            _nickname = nickname;
            _color = color;
            _points = 0;
            _won = 0;
        }

        public void addPoints()
        {
            _points++;
        }

        public Color getColor()
        {
            return _color;
        }

        public int getWinStatus()
        {
            return _won;
        }

        public int getPoints()
        {
            return _points;
        }

        public string getNickname()
        {
            return _nickname;
        }

        public void playerWon()
        {
            _won = 1;
        }

        public void resetPoints()
        {
            _points = 0;
        }

        public void resetWonStatus()
        {
            _won = 0;
        }
    }
}
