using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace final_work
{
    public class Player // Player - class with name, age, wins, losses and draws and time variables and functions to access and save the Tic-Tac-Toe - file
    {                   // Tic-Tac-Toe - file saves players and computer opponent informations.
        public string sFirstName { get; set; }
        public string sLastName { get; set; }
        public int iBirthYear { get; set; }
        public int iAgeNow { get; set; }
        public int iWins { get; set; }
        public int iLosses { get; set; }
        public int iDraws { get; set; }
        public int iTime { get; set; }

        public string GetTimeString // Property for showing user gameplay duration with minutes and seconds.
        {
            get
            {
                string sTime = "";
                if (iTime > 60)
                {
                    int iMinutes = iTime / 60;
                    int iSeconds = iTime % 60;
                    sTime = iMinutes + ":" + iSeconds;
                }
                else if (iTime < 10)
                {
                    sTime = "0:0" + iTime;
                }
                else
                {
                    sTime = "0:" + iTime;
                }
                return sTime;
            }
            set {; }            
        }

        public string GetPlayerName() // Function to get Players full name out of the class.
        {
            return sFirstName + " " + sLastName;
        }

        public void AddPlayerWins() // Function to add 1 to Players wins.
        {
            iWins++;
        }

        public void AddPlayerLosses() // Function to add 1 to Players losses.
        {
            iLosses++;
        }

        public void AddPlayerDraws() // Function to add 1 to Players draws.
        {
            iDraws++;
        }

        public void UpdatePlayerTime(int iGameTime) // Function to update players played game time.
        {
            iTime = iTime + iGameTime;
        }

        public void SerializePlayers(ObservableCollection<Player> players) // Function to write Player info to Tic-Tac-Toe - file 
        {

            string sBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFilePath = Path.Combine(sBaseDirectory, "Tic-Tac-Toe.JSON");

            string sJsonString = JsonSerializer.Serialize(players);
            File.WriteAllText(sFilePath, sJsonString);
        }

        public ObservableCollection<Player> DeSerializePlayers() // Function to read Player info from file.
        {
            string sBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFilePath = Path.Combine(sBaseDirectory, "Tic-Tac-Toe.JSON");

            try // Try read JSON - file and return its info to ObservableCollection.
            {
                string sJsonString = File.ReadAllText(sFilePath);
                ObservableCollection<Player> players = JsonSerializer.Deserialize<ObservableCollection<Player>>(sJsonString);

                foreach (var player in players)
                {
                    player.iAgeNow = DateTime.Now.Year - player.iBirthYear;

                }

                return players;

            }
            catch (Exception ex) // If JSON - file doesn´t exist create new JSON - file with Computer Opponents info in it.
            {
                Player computerOpponent = new Player
                {
                    sFirstName = "Computer",
                    sLastName = "Opponent",
                    iBirthYear = DateTime.Now.Year - 5,
                    iWins = 0,
                    iLosses = 0,
                    iDraws = 0,
                    iTime = 0,
                };

                ObservableCollection<Player> players = new ObservableCollection<Player>
                    {
                        computerOpponent
                    };

                string sNewJsonString = JsonSerializer.Serialize(players);
                File.WriteAllText(sFilePath, sNewJsonString);

                return players;
            }
        }
    }
}
