using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PongGame
{
    [DataContract(Namespace = "Userdata")]
    public class Userdata
    {
        private int[] scores;
        private string[] usernames;
        private int[] results;

        public Userdata(int[] scores, string[] usernames, int[] results)
        {
            this.scores = scores;
            this.usernames = usernames;
            this.results = results;
        }

        [DataMember]
        public int[] Scores
        {
            get { return scores; }
            set { scores = value; }
        }

        [DataMember]
        public string[] Usernames
        {
            get { return usernames; }
            set { usernames = value; }
        }

        [DataMember]
        public int[] Results
        {
            get { return results; }
            set { results = value; }
        }
    }
}
