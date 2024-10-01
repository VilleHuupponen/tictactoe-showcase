namespace final_work;

public partial class GameStatsPage : ContentPage
{
    public GameStatsPage(MainPage mp)
    {
        InitializeComponent();
        Player playerFile = new Player(); // Player Class so GameStatsPage have acces to DeSerializePlayers function.
        PlayersLV.ItemsSource = playerFile.DeSerializePlayers(); // DeSerializePlayers function reads information from Tic-Tac-Toe file and returns it to listview for user.
    }

    private void BackBtn_Clicked(object sender, EventArgs e) // Button to navigate back to mainpage.
    {
        Navigation.PopAsync();
    }
}