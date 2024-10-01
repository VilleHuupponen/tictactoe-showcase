using System.Collections.ObjectModel;

namespace final_work;

public partial class NewPlayerPage : ContentPage
{
    public NewPlayerPage(NewGamePage ngp)
    {
        InitializeComponent();
    }
    private void PlayerFirstNameEntry_Completed(object sender, EventArgs e) // If user wants to use enter to shift entrys
    {
        PlayerLastNameEntry.Focus();
    }

    private void PlayerLastNameEntry_Completed(object sender, EventArgs e)
    {
        PlayerBirthYearEntry.Focus();
    }

    private void PlayerBirthYearEntry_Completed(object sender, EventArgs e)
    {
        SaveNewPlayer();
    }

    private void SaveNewPlayerNameBtn_Clicked(object sender, EventArgs e) 
    {
        SaveNewPlayer();
    }

    public async void SaveNewPlayer() // Functions for user to create and save new player.
    {
        Player newPlayer = new Player();
        ObservableCollection<Player> players = newPlayer.DeSerializePlayers(); // First information is retrieved from Tic-Tac-Toe file and placed to observablecollection.

        bool bPlayerExists = players.Any(player => player.sFirstName + player.sLastName == PlayerFirstNameEntry.Text + PlayerLastNameEntry.Text); // The list is checked so that the player does not already exist.
        bool bAgeIsNumber = int.TryParse(PlayerBirthYearEntry.Text, out int iAge); // The year of birth is checked that numbers have been entered.

        if (bPlayerExists == true) // If player already exist user gets error message.
        {
            await DisplayAlert("Huom", "Player already exists", "OK");
        }
        else if (string.IsNullOrEmpty(PlayerFirstNameEntry.Text) || string.IsNullOrEmpty(PlayerLastNameEntry.Text) || string.IsNullOrEmpty(PlayerBirthYearEntry.Text)) // If any name entry are empty user gets error message.
        {
            await DisplayAlert("Huom", "Give player full name", "OK");

            if (string.IsNullOrEmpty(PlayerFirstNameEntry.Text))
            {
                PlayerFirstNameEntry.Focus();
            }
            else
            {
                PlayerLastNameEntry.Focus();
            }
        }
        else if (bAgeIsNumber == false) // If birth year is not numbers user gets error message.
        {
            await DisplayAlert("Huom", "Age can only be numbers", "OK");
            PlayerBirthYearEntry.Focus();
        }
        else if (iAge < 1900 || iAge > DateTime.Now.Year) // Check that age is in correct range.
        {
            await DisplayAlert("Huom", "Please check birth year", "OK");
            PlayerBirthYearEntry.Focus();
        }
        else // If all checks are correct new player is created
        {
            newPlayer.sFirstName = PlayerFirstNameEntry.Text;
            newPlayer.sLastName = PlayerLastNameEntry.Text;
            newPlayer.iBirthYear = int.Parse(PlayerBirthYearEntry.Text);
            newPlayer.iWins = 0; // player scores are set to 0
            newPlayer.iLosses = 0;
            newPlayer.iDraws = 0;
            newPlayer.iTime = 0; // player played game time is set to 0

            players.Add(newPlayer);
            newPlayer.SerializePlayers(players); // and updated list of players is saved to Tic-Tac-Toe file.

            await Navigation.PopAsync();
        }
    }

    private void CancelNewPlayerNameBtn_Clicked(object sender, EventArgs e) // Button if user desides not to create user.
    {                                                                       // any writings and changes are ignored  
        Navigation.PopAsync(); // and user is returned back to NewGamePage.
    }

}