using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Configuration
{

    [DataContract(Namespace = "Configuration")]
    public class User
    {
        private string username;
        private int score;
        private int wins;
        private int losses;

        public User(string username, int score, int wins, int losses)
        {
            this.username = username;
            this.score = score;
            this.wins = wins;
            this.losses = losses;
        }

        public User()
        {

        }

        [DataMember]
        public int Score
        {
            get { return this.score; }
            set { score = value; }
        }

        [DataMember]
        public string Username
        {
            get { return this.username; }
            set { username = value; }
        }

        [DataMember]
        public int Wins
        {
            get { return this.wins; }
            set { wins = value; }
        }

        [DataMember]
        public int Losses
        {
            get { return this.losses; }
            set { losses = value; }
        }
    }
}
