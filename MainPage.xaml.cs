namespace final_work;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        Player player = new Player(); // Player class so mainpage have acces to DeSerializePlayer function.
        player.DeSerializePlayers(); 

        /*In this instance "DeSerializePlayer" main goal is to check if Tic-Tac-Toe file exists 
        and if not it is created. This is because computer opponents information is created if file does not exist.*/
    }

    private void NewGameBtn_Clicked(object sender, EventArgs e) // Button to navigate to NewGamePage.
    {
        Navigation.PushAsync(new NewGamePage(this));
    }

    private void StatsBtn_Clicked(object sender, EventArgs e) // Button to navigate to GameStatsPage (Leaderboard).
    {
        Navigation.PushAsync(new GameStatsPage(this));
    }

    private void QuitBtn_Clicked(object sender, EventArgs e) // Function for user to shut down software.
    {
        Application.Current.Quit();
    }
}

