using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration
{
    public static class WordFilter
    {
        public static List<string> filter = new List<string>();
        private static string path = @"../../../wordFilter.txt";    // relativen Pfad ändern, bei Bildern auch
        
        public static void init()
        {
            string line;
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fileStream);

            while ((line = streamReader.ReadLine()) != null)
            {
                filter.Add(line);
            }
        } 
    }
}
