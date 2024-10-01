namespace final_work;

public partial class NewGamePage : ContentPage
{
    public static Player playerX; // Class for storing selected players info to GamePage
    public static Player playerO; // (static so information stays between pages)

    public NewGamePage(MainPage mp)
    {
        InitializeComponent();
    }

    protected override void OnAppearing() // When page appears 
    {
        base.OnAppearing();
        Player playerFile = new Player();
        PlayerOLV.ItemsSource = playerFile.DeSerializePlayers(); // listviews are updated from Tic-Tac-Toe file.
        PlayerXLV.ItemsSource = playerFile.DeSerializePlayers();
    }

    private void NewPlayerBtn_Clicked(object sender, EventArgs e) // Button for creating new player. Button navigates to NewPlayerPage.
    {
        Navigation.PushAsync(new NewPlayerPage(this));
    }

    private async void StartBtn_Clicked(object sender, EventArgs e) // Button for starting the game
    {
        if (PlayerXName.Text == null || PlayerOName.Text == null) // if players haven´t been chosen user gets error message
        {
            await DisplayAlert("Huom!", "Choose both players to game", "OK");
        }

        else if (PlayerXName.Text == PlayerOName.Text) // if players are same user gets error message
        {
            await DisplayAlert("Huom!", "Player X cannot be same as Player O", "OK");
        }
        else
        {
            await Navigation.PushAsync(new GamePage(playerX, playerO)); // if checks are ok navigate to GamePage.
        }
    }

    private void BackBtn_Clicked(object sender, EventArgs e) // Button to navigate back to mainpage.
    {
        Navigation.PopAsync();
    }

    private void PlayerXLV_ItemSelected(object sender, SelectedItemChangedEventArgs e) // Event for choosing player X from listview.
    {
        playerX = (Player)e.SelectedItem; // Set selected players informations to Player O.
        PlayerXName.Text = playerX.GetPlayerName(); // Change Player O name label for user to see which player is chosen.
    }

    private void PlayerOLV_ItemSelected(object sender, SelectedItemChangedEventArgs e) // Event for choosing player O from listview.
    {
        playerO = (Player)e.SelectedItem; // Set selected players informations to Player O.
        PlayerOName.Text = playerO.GetPlayerName(); // Change Player O name label for user to see which player is chosen.
    }
}