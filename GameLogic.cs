using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wordle; 
using WORDLE; 

namespace WORDLE 
{
    public class GameLogic
    {
        private WordList wordList;
        private string targetWord;
        private int currentRow;
        private const int MaxRows = 6;
        private bool gameRunning = false;

        // Property to get the target word
        public string TargetWord => targetWord;

        public GameLogic()
        {
            wordList = new WordList();
            InitializeGame();
        }

        // Asynchronous method to initialize the game and ensure the word list is available
        private async void InitializeGame()
        {
            await wordList.EnsureWordListIsAvailable();
            StartNewGame();
        }

        // Method to start a new game
        public void StartNewGame()
        {
            targetWord = wordList.GetRandomWord(); // Assuming GetRandomWord() gets a random word
            currentRow = 0;
            gameRunning = true;
        }

        // Method to check a guessed word and provide feedback
        public string CheckGuess(string guessedWord)
        {
            if (!gameRunning || string.IsNullOrWhiteSpace(guessedWord))
                return string.Empty;

            if (guessedWord.Length != targetWord.Length)
                return "Invalid word length"; // Error message for incorrect word length

            currentRow++; // Move to the next row

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < guessedWord.Length; i++)
            {
                if (guessedWord[i] == targetWord[i])
                    result.Append('G'); // Letter is correct and in the right position
                else if (targetWord.Contains(guessedWord[i]))
                    result.Append('Y'); // Letter is correct but in the wrong position
                else
                    result.Append('X'); // Letter is not in the word
            }

            // Check if the game is over
            if (currentRow >= MaxRows || guessedWord.Equals(targetWord, StringComparison.OrdinalIgnoreCase))
            {
                gameRunning = false; // Game over
            }

            return result.ToString(); // Return feedback for the guessed word
        }

        // Method to check if the game is over
        public bool IsGameOver()
        {
            return !gameRunning;
        }

        // Method to check if the guessed word is correct
        public bool IsWordGuessedCorrectly(string guessedWord)
        {
            return guessedWord.Equals(targetWord, StringComparison.OrdinalIgnoreCase);
        }

        // Property to store game history
        public List<GameHistory> GameHistory { get; private set; } = new List<GameHistory>();

        // Method to add a game history entry
        public void AddGameHistoryEntry(string guessedWord)
        {
            if (IsGameOver() && IsWordGuessedCorrectly(guessedWord))
            {
                GameHistory.Add(new GameHistory
                {
                    Timestamp = DateTime.Now,
                    CorrectWord = targetWord,
                    Guesses = currentRow,

                });
            }
        }

    }
}
