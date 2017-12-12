using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongGame
{
    public class Game
    {
        private Player[] _participatingPlayers;
        private string _timePlayed;
        private int[] _endScore;

        public Game(Player[] allPlayers)
        {
            _participatingPlayers = allPlayers;
        }
    }
}
