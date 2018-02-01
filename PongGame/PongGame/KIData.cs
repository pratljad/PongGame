using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace PongGame
{
    [DataContract(Namespace = "KIData")]
    public class KIData
    {
        private string[] usernames;
        private int time;
        public KIData(string[] usernames, int time)
        {
            this.usernames = usernames;
            this.time = time;
        }
        [DataMember]
        public string[] Usernames
        {
            get { return usernames; }
            set { usernames = value; }
        }
        [DataMember]
        public int Time
        {
            get { return time; }
            set { time = value; }
        }
    }
}
