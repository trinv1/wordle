using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Maui.Controls;
using wordle;

namespace WORDLE
{
    public partial class MainPage : ContentPage
    {
        // Define class-level variables
        private bool gamerunning;
        private int currentColumnIndex;
        private GameLogic gameLogic;
        private string guessedWord;
        private List<BoxView> currentRowBoxViews = new List<BoxView>();
        private int currentAttempt = 5;
        private int currentAttemptRow;
        private Stopwatch timer = new Stopwatch();
        private System.Timers.Timer wordletimer;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            gameLogic = new GameLogic();
            InitialiseGrid();
            NavigationPage.SetHasNavigationBar(this, false);

            // Hide various UI elements initially
            GameGrid.IsVisible = false;
            GameGrid.IsEnabled = false;
            RulesLabel.IsVisible = false;
            RulesLabel.IsEnabled = false;
            TimerLabel.IsEnabled = false;
            TimerLabel.IsVisible = false;
        }

        // Method to initialize the grid
        public void InitialiseGrid()
        {
            GameGrid.Children.Clear(); // Clear existing elements
            currentRowBoxViews.Clear(); // Clear the current row's BoxView list

            for (int i = 0; i < 6; i++) // Initialize 5x6 grid
            {
                for (int j = 0; j < 5; j++)
                {
                    BoxView box = new BoxView()
                    {
                        Color = Color.FromRgb(255, 255, 255),
                        CornerRadius = 20,
                        BackgroundColor = Color.FromRgb(255, 255, 255),
                        Margin = new Thickness(5), // Add margin to create gaps
                    };
                    Grid.SetRow(box, i); // Set the row position
                    Grid.SetColumn(box, j); // Set the column position
                    GameGrid.Children.Add(box); // Add the BoxView to the Children collection

                    if (i == currentAttempt) // Only add BoxViews of the current attempt
                    {
                        currentRowBoxViews.Add(box);
                    }
                }
            }

            currentAttempt--;
        }

        // Event handler for the "Start Game" button click
        private async void OnStartGameClicked(object sender, EventArgs e)
        {
            string playerName = NameEntry.Text;
            gamerunning = true;
            timer.Start();

            if (!string.IsNullOrEmpty(playerName))
            {
                PromptForName(playerName);

                // Hide various UI elements and show the game grid
                TimerLabel.IsEnabled = false;
                TimerLabel.IsVisible = false;
                StartGameButton.IsVisible = false;
                RulesButton.IsVisible = false;
                GameGrid.IsVisible = true;
                GameGrid.IsEnabled = true;
                RulesLabel.IsVisible = false;
                RulesLabel.IsEnabled = false;
                NameEntry.IsVisible = false;

                // Show the Home button and keyboard layout
                HomeButton.IsVisible = true;
                KeyboardLayout.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Error", "Please enter a valid name.", "OK");
            }
        }

        // Method to prompt for the player's name
        private async void PromptForName(string playerName)
        {
            string playerFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), playerName + ".txt");

            if (File.Exists(playerFile))
            {
                // If the file exists, load the file
                string data = File.ReadAllText(playerFile);
                await DisplayAlert("Welcome Back", "Welcome back, " + playerName, "OK");
                // Process the data as needed
            }
            else
            {
                // If the file does not exist, create the file
                await DisplayAlert("Welcome", "Welcome, new player!", "OK");
                File.WriteAllText(playerFile, playerName); // Creating a new file with the player's name
            }
        }

        // Event handler for the "Rules" button click
        private void OnRulesClicked(object sender, EventArgs e)
        {
            // Hide various UI elements and show the rules label
            StartGameButton.IsVisible = false;
            RulesButton.IsVisible = false;
            GameGrid.IsVisible = false;
            GameGrid.IsEnabled = false;
            RulesLabel.IsVisible = true;
            RulesLabel.IsEnabled = true;

            // Show the Home button
            HomeButton.IsVisible = true;
        }

        // Event handler for the "Settings" button click
        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            // Navigate to the SettingsPage
            await Navigation.PushAsync(new SettingsPage());
            SettingsButton.IsVisible = false;
        }

        // Event handler for the "Home" button click
        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Navigate back to the previous page
            gamerunning = false;
            GameGrid.IsVisible = false;
            GameGrid.IsEnabled = false;
            RulesButton.IsVisible = true;
            KeyboardLayout.IsVisible = false;
            HomeButton.IsVisible = false;
            StartGameButton.IsVisible = true;
            RulesLabel.IsVisible = false;
            RulesLabel.IsEnabled = false;
            NameEntry.IsVisible = true;
        }

        // Event handler for letter buttons click
        private void OnLetterClicked(object sender, EventArgs e)
        {
            gamerunning = true;

            if (sender is not Button button)
                return;

            // To handle the Back button functionality
            if (button.Text == "Back")
            {
                if (currentColumnIndex > 0)
                {
                    currentColumnIndex--;
                    UpdateLabel(currentColumnIndex, " ");
                    submitWord.IsEnabled = false; // Disabling submit button as the word is no longer complete
                }
                return; // Exit the method here as no further action is required for the Back button
            }

            // Handle adding a new letter
            if (gamerunning && currentColumnIndex < 5)
            {
                string letter = button.Text;
                UpdateLabel(currentColumnIndex, letter);

                // Create a new Label with the clicked letter
                Label label = new Label
                {
                    Text = letter,
                    FontSize = 24,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };

                // Add the Label to the current column and row in the grid
                Grid.SetRow(label, 0); // Always add to the top row
                Grid.SetColumn(label, currentColumnIndex);
                GameGrid.Children.Add(label); // Add the Label to the Children collection

                // Increment the current column index
                currentColumnIndex++;

                // Enable the submit button if 5 letters have been entered
                if (currentColumnIndex == 5)
                {
                    submitWord.IsEnabled = true;
                }
            }
        }

        // Method to update letter labels
        private void UpdateLabel(int index, string text)
        {
            switch (index)
            {
                case 0: letter1.Text = text; break;
                case 1: letter2.Text = text; break;
                case 2: letter3.Text = text; break;
                case 3: letter4.Text = text; break;
                case 4: letter5.Text = text; break;
            }
        }

        // Event handler for the "Submit" button click
        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            if (!gamerunning || currentColumnIndex != 5)
            {
                // Ensure the game is running and all letters are entered
                await DisplayAlert("Error", "Please enter a 5-letter word.", "OK");
                return;
            }

            guessedWord = string.Concat(
                letter1.Text, letter2.Text, letter3.Text,
                letter4.Text, letter5.Text);

            if (guessedWord.Length != 5)
            {
                await DisplayAlert("Error", "Incomplete word. Please enter 5 letters.", "OK");
                return;
            }

            string checkResult = gameLogic.CheckGuess(guessedWord);

            // Update the UI based on the check result
            UpdateUIWithCheckResult(checkResult);

            if (gameLogic.IsGameOver())
            {
                timer.Stop();
                HandleGameOver();
            }
            else
            {
                // Move to the next row and reset the columns
                currentAttemptRow++;
                ResetForNextAttempt();
            }
        }

        // Method to update the UI with check result
        private void UpdateUIWithCheckResult(string checkResult)
        {
            BoxView[] boxViews = { box1, box2, box3, box4, box5 };

            // Displaying results
            for (int i = 0; i < checkResult.Length; i++)
            {
                if (i < currentRowBoxViews.Count)
                {
                    var box = boxViews[i];
                    switch (checkResult[i])
                    {
                        case 'G':
                            boxViews[i].BackgroundColor = Color.FromRgb(0, 128, 0);
                            break;
                        case 'Y':
                            boxViews[i].BackgroundColor = Color.FromRgb(255, 255, 0);
                            break;
                        case 'X':
                            boxViews[i].BackgroundColor = Color.FromRgb(128, 128, 128);
                            break;
                    }
                }
            }

            // Check if the game is over
            if (gameLogic.IsGameOver())
            {
                HandleGameOver();
            }
        }

        // Method to reset for the next attempt
        private void ResetForNextAttempt()
        {
            currentColumnIndex = 0;
            currentRowBoxViews.Clear();
            currentAttempt++; // Increment the attempt
            InitialiseGrid();
        }

        // Method to handle the game over condition
        private async void HandleGameOver()
        {
            if (gameLogic.IsWordGuessedCorrectly(guessedWord))
            {
                await DisplayAlert("Congratulations", "You guessed the word!", "OK");
            }
            else
            {
                await DisplayAlert("Game Over", $"The correct word was {gameLogic.TargetWord}.", "OK");
            }
            // Optionally, offer to restart the game
            gameLogic.StartNewGame(); // Resets the game
        }
    }
}
