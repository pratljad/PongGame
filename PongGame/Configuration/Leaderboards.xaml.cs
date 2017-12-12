using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Configuration
{
    /// <summary>
    /// Interaktionslogik für Leaderboards.xaml
    /// </summary>
    public partial class Leaderboards : Window
    {
        private string selection;
        private Dictionary<string, string> comboboxDictionary;
        public Leaderboards()
        {
            InitializeComponent();
            initComboBox();
        }

        private void initComboBox()
        {
            comboboxDictionary = new Dictionary<string, string>();
            comboboxDictionary.Add("Leaderboard by score", "mg");
            comboboxDictionary.Add("Leaderboard by wins", "mw");
            comboboxDictionary.Add("Leaderboard by losses", "ml");

            CB_LeaderboardsChoices.DisplayMemberPath = "Key";
            CB_LeaderboardsChoices.SelectedValuePath = "Value";
            CB_LeaderboardsChoices.ItemsSource = comboboxDictionary;

            CB_LeaderboardsChoices.SelectedIndex = 0;
        }

        private void BTN_RefreshLeaderboards_Click(object sender, RoutedEventArgs e)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(User));
            int rank = 1;
            string finalServerResponse;

            try
            {
                if (selection != null)
                {
                    WebRequest request = WebRequest.Create("http://127.0.0.1:3000/leaderboard?req=" + selection);
                    request.Method = "GET";
                    WebResponse response = request.GetResponse();
                    Stream webstream = response.GetResponseStream();
                    StreamReader mywebstreamreader = new StreamReader(webstream);

                    var jsSerializer = new JavaScriptSerializer();
                    finalServerResponse = mywebstreamreader.ReadToEnd();
                    User[] nu = jsSerializer.Deserialize<User[]>(finalServerResponse);

                    
                    LB_LeaderboardView.Items.Clear();
                    foreach (User u in nu)
                    {
                        LB_LeaderboardView.Items.Add(new { rank, u.Username, u.Score, u.Wins, u.Losses });
                        rank++;
                    }
                    
                    webstream.Close();
                    mywebstreamreader.Close();
                    rank = 1;
                }

                else
                {
                    throw new Exception("No comboxbox item selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private void CB_LeaderboardsChoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            
            KeyValuePair<string, string> selectedEntry = (KeyValuePair<string, string>)comboBox.SelectedItem;
            
            if(comboboxDictionary.ContainsKey(selectedEntry.Key))
            {
                selection = selectedEntry.Value;
                BTN_RefreshLeaderboards_Click(sender, e);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
        }
    }
}
