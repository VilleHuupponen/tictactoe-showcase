using System.Collections.ObjectModel;

namespace final_work;

public partial class GamePage : ContentPage
{
    Player playerXClass; // Player X information from NewGamePage.
    Player playerOClass; // Player O information from NewGamePage.
    TurnNumber turnNumber = new TurnNumber(); // Class for keeping track of player turns.
    ComputerOpponent computerOpponent = new ComputerOpponent(); // Computer opponent functios are introduced.

    IDispatcherTimer timer = Application.Current.Dispatcher.CreateTimer(); // Timer for keeping track of played game time.
    DateTime startTime = DateTime.Now; // startTime variable tracks game starting time.

    bool bPlayerTurn = true; // Variable to prevent user button presses on computer oppponents turn.
    bool bWinnerOrDraw; // Variable to keep track if any player have won or if game is a draw.
    string[,] array; // Array to track which square has already been played.

    public GamePage(Player playerX, Player playerO) // Players informations are received from NewGamePage.
    {
        InitializeComponent();
        this.playerXClass = playerX; // Player information is renamed and set to use in GamePage.
        this.playerOClass = playerO;
        PlayerXNameLabel.Text = playerXClass.GetPlayerName(); // Players names are showed to user(s).
        PlayerONameLabel.Text = playerOClass.GetPlayerName();

        StartNewGame(); // Functions for starting new game
    }

    public void StartNewGame() // Function where game is initialized and gameplay indicators are reseted and restarted.
    {
        startTime = DateTime.Now; // starTime is initialized about current time.

        Button00Btn.ImageSource = "empty.jpg"; // Buttons image sources are reseted
        Button01Btn.ImageSource = "empty.jpg";
        Button02Btn.ImageSource = "empty.jpg";
        Button10Btn.ImageSource = "empty.jpg";
        Button11Btn.ImageSource = "empty.jpg";
        Button12Btn.ImageSource = "empty.jpg";
        Button20Btn.ImageSource = "empty.jpg";
        Button21Btn.ImageSource = "empty.jpg";
        Button22Btn.ImageSource = "empty.jpg";

        bWinnerOrDraw = false;
        array = new string[3, 3];
        turnNumber.ResetTurnNumber(); // Turn number is set to 1.
        StartTimeTimer(); // Function where game duration timer is initialized and started.
        ChangePlayerTurnColors(); // Function where player turn order indicators are changed.
        CheckComputerOpponent(); // Function where is checked if computer opponent is in play.

        if (PlayerXNameLabel.Text == "Computer Opponent") // If computer opponent is player x it goes first.
        {
            ComputerOpponentPlay();
        }
    }

    public void StartTimeTimer() // Game duration timer functions
    {
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (s, e) => UpdateTime();
        timer.Start();
        UpdateTime();
    }

    public void UpdateTime() // A Function where timer label and players data are updated with game times.
    {
        TimeSpan elapsedTime = DateTime.Now - startTime;
        TimerLabel.Text = elapsedTime.Seconds.ToString() + "s";

        if (bWinnerOrDraw == true) // If game is ended players played game times are updated.
        {
            playerXClass.UpdatePlayerTime(elapsedTime.Seconds);
            playerOClass.UpdatePlayerTime(elapsedTime.Seconds);
        }
        return;
    }

    private void ChangePlayerTurnColors() // Functions for user to see whose turn it is. Color is changed from the one in turn.
    {

        if (turnNumber.GetTurnNumber() % 2 == 0)
        {
            PlayerO.BackgroundColor = Color.FromRgb(137, 207, 240);
            PlayerONameLabel.BackgroundColor = Color.FromRgb(137, 207, 240);
            PlayerX.BackgroundColor = default;
            PlayerXNameLabel.BackgroundColor = default;
        }
        else
        {
            PlayerX.BackgroundColor = Color.FromRgb(137, 207, 240);
            PlayerXNameLabel.BackgroundColor = Color.FromRgb(137, 207, 240);
            PlayerO.BackgroundColor = default;
            PlayerONameLabel.BackgroundColor = default;
        }
    }

    private void CheckComputerOpponent() // Function checks if any players name is "Computer Opponent"
    {
        if (playerXClass.GetPlayerName() == "Computer Opponent" || playerOClass.GetPlayerName() == "Computer Opponent")
        {
            computerOpponent.ComputerInPlay(); // if computer is in play information is changed in class.
        }
    }

    private async void ComputerOpponentPlay() // Computer opponents turn actions.
    {
        bPlayerTurn = false;
        await RandomDelay(); // Function for random delay (0,5s - 2s).

        bool bComputerOpponentClicked = false; // Loop to press legal move.
        do
        {
            Button button = GetRandomButton(); // Function to get random press.
            bComputerOpponentClicked = GameAction(button); // If play is legal loop breaks.
        } while (bComputerOpponentClicked == false);

        CheckWinner(); // After the play winner is checked.
        if (bWinnerOrDraw == false) // If the play does not lead to a victory
        {
            CheckIfDraw(); // check if the game is a draw
            if (bWinnerOrDraw == false)
            {
                turnNumber.AddTurn(); // if game is still ongoing increase the turn number
                ChangePlayerTurnColors(); // and change indicators for whose turn it is.
            }
        }
        bPlayerTurn = true;
    }

    public async Task RandomDelay() // Function for get random delay for computer opponent.
    {
        Random rnd = new Random();
        int iDelay = rnd.Next(500, 2001); // Delay is 0,5s - 2s.
        await Task.Delay(iDelay); // Delay is waited and then returned to continue computeropponents turn.
    }

    public Button GetRandomButton() // functions for random button press for computer opponent.
    {/* First all buttons are placed to array */
        Button[] buttons = { Button00Btn, Button01Btn, Button02Btn, Button10Btn, Button11Btn, Button12Btn, Button20Btn, Button21Btn, Button22Btn };
        int iRandomIndex = new Random().Next(0, buttons.Length); //Then random button is chosen.

        return buttons[iRandomIndex]; // Random button is returned.
    }

    private bool GameAction(Button button) // functions when player presses a button.
    {
        string sImage;
        int iRow = Grid.GetRow(button); // The Buttons row information is collected.
        int iColumn = Grid.GetColumn(button); // The Buttons column information is collected.

        if (array[iRow, iColumn] == null) // Check if array (button) is free to play.
        {
            if (turnNumber.GetTurnNumber() % 2 == 0) // Check for whose turn it is.
            {
                sImage = "zero.jpg"; // Buttons image is changed.
                array[iRow, iColumn] = "Z"; // The status of the array (button) has been updated from free to played by Player X
            }
            else
            {
                sImage = "cross.jpg"; // Same actions for Player O.
                array[iRow, iColumn] = "C";
            }

            button.ImageSource = sImage; // Buttons image is changed if play was legal
            return true; // True is returned to indicate play was legal.
        }
        else
        {
            return false; // If checks fail false is returned to indicate illegal move.
        }
    }

    private void CheckWinner() // Functions for checking the winner
    {
        {
            for (int i = 0; i < 3; i++) // Loop to check rows and columns for C character (bWinnerOrDraw prevents winning multible times)
            {
                if (CheckLinePlayerX(array[i, 0], array[i, 1], array[i, 2]) == true && bWinnerOrDraw == false || CheckLinePlayerX(array[0, i], array[1, i], array[2, i]) == true && bWinnerOrDraw == false)
                {
                    bWinnerOrDraw = true; // Boolean is changed and game ends.
                    AddWinnerScore('C'); // if there are three same characters winner is Player X                 
                }
            }

            if (CheckLinePlayerX(array[0, 0], array[1, 1], array[2, 2]) && bWinnerOrDraw == false || CheckLinePlayerX(array[0, 2], array[1, 1], array[2, 0]) && bWinnerOrDraw == false)
            { // Loop to check diagonals for C character (bWinnerOrDraw prevents winning multible times)
                bWinnerOrDraw = true; // Boolean is changed and game ends.
                AddWinnerScore('C'); // if there are three same characters winner is Player X               
            }

            for (int i = 0; i < 3; i++) // Loop to check rows and columns for Z character (bWinnerOrDraw prevents winning multible times)
            {
                if (CheckLinePlayerO(array[i, 0], array[i, 1], array[i, 2]) == true && bWinnerOrDraw == false || CheckLinePlayerO(array[0, i], array[1, i], array[2, i]) == true && bWinnerOrDraw == false)
                {
                    bWinnerOrDraw = true; // Boolean is changed and game ends.
                    AddWinnerScore('Z'); // if there are three same characters winner is Player O                   
                }
            }

            if (CheckLinePlayerO(array[0, 0], array[1, 1], array[2, 2]) == true && bWinnerOrDraw == false || CheckLinePlayerO(array[0, 2], array[1, 1], array[2, 0]) == true && bWinnerOrDraw == false)
            { // Loop to check diagonals for Z character (bWinnerOrDraw prevents winning multible times)
                bWinnerOrDraw = true; // Boolean is changed and game ends.
                AddWinnerScore('Z'); // if there are three same characters winner is Player O          
            }
        }
    }
    private bool CheckLinePlayerX(string a, string b, string c) // Check if all characters are C
    {

        if (a == "C" && b == "C" && c == "C")
        {
            return true;
        }

        return false;
    }
    private bool CheckLinePlayerO(string a, string b, string c) // Check if all characters are Z
    {

        if (a == "Z" && b == "Z" && c == "Z")
        {
            return true;
        }
        return false;
    }

    public async void AddWinnerScore(char cWinner) // Functions for increase and decrease score.
    {
        timer.Stop();
        if (cWinner == 'C') // If winner is player X
        {
            playerXClass.AddPlayerWins(); // player x wins increase
            playerOClass.AddPlayerLosses(); // and player O losses increse
            UpdateTime(); // Game time is updated to players
            await DisplayAlert("Winner!", playerXClass.GetPlayerName() + " is the winner!", "OK"); // User(s) gets message about winner.            
        }
        else // if winner is player O
        {
            playerOClass.AddPlayerWins(); // player O wins increase
            playerXClass.AddPlayerLosses(); // player X losses increse
            UpdateTime(); // Game time is updated to players
            await DisplayAlert("Winner!", playerOClass.GetPlayerName() + " is the winner!", "OK"); // User(s) gets message about winner.
        }
    }

    private void CheckIfDraw() // Function for check if turn order is 9 so there are no leagal moves left.
    {
        if (turnNumber.GetTurnNumber() == 9)
        {
            timer.Stop();
            UpdateTime(); // Game time is updated to players
            bWinnerOrDraw = true;
            DisplayAlert("Draw!", "The game is a draw.", "OK"); // User gets a message for draw.
            playerXClass.AddPlayerDraws(); // Players draws score is increased.
            playerOClass.AddPlayerDraws();
        }
    }

    public void SaveGame() // Played game information is saved to Tic-Tac-Toe file
    {
        
        Player PlayerFile = new Player();
        ObservableCollection<Player> allplayers = PlayerFile.DeSerializePlayers(); // Information from Tic-Tac-Toe file is received.

        var playersToRemove = allplayers.Where(p => p.sFirstName == playerXClass.sFirstName || p.sFirstName == playerOClass.sFirstName || p.sLastName == playerXClass.sLastName || p.sLastName == playerOClass.sLastName).ToList();
        foreach (var playerToRemove in playersToRemove)
        {
            allplayers.Remove(playerToRemove); // Current players are removed from observablecollection
        }

        allplayers.Add(playerXClass); // Current players are added to list
        allplayers.Add(playerOClass); // this way wins, losses and draws information is correct in Tic-Tac-Toe file 
        PlayerFile.SerializePlayers(allplayers); // then list is stored to file

        computerOpponent.ComputerOutPlay(); // Computer opponent is removed from the play.
    }

    private void QuitBtn_Clicked(object sender, EventArgs e) // Button event for quitting game.
    {
        SaveGame(); // Previous game is saved if it ended.
        var desiredPage = new MainPage();
        Navigation.PushAsync(desiredPage); // Button takes to mainpage.
    }

    private void RestartBtn_Clicked(object sender, EventArgs e) // Button to restart game.
    {
        SaveGame(); // Previous game is saved if it ended.
        StartNewGame(); // New game is initialized.    
    }

    private void Button_Clicked(object sender, EventArgs e) // Event for user button presses.
    {
        if (bPlayerTurn == true) // Check is it human player turn or computer opponents turn.
        {
            Button button = sender as Button; // what button is pressed information received.
            bool bLeagalMove = GameAction(button); // Check if play is legal.

            if (bLeagalMove == true)
            {
                CheckWinner(); // If the play was legal check if the result is victory.
                if (bWinnerOrDraw == false)
                {
                    CheckIfDraw(); // If not check if play is a draw
                    if (bWinnerOrDraw == false)
                    {
                        turnNumber.AddTurn(); // if not increase turn number
                        ChangePlayerTurnColors(); // and change indicators whose turn it is.
                    }
                }
            }

            if (computerOpponent.IsComputerInPlay() == true && bWinnerOrDraw == false) // Check if it is another users turn or computers turn and is game still going.
            {
                ComputerOpponentPlay();
            }
        }
    }
}