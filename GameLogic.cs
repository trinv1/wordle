using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WORDLE;

namespace WORDLE;

public class GameLogic
{
    private WordList wordList;
    private string targetWord;
    private int currentAttempt;
    private const int MaxAttempts = 6; // Adjust as needed
    private bool gameRunning = false;
    public string TargetWord => targetWord;

    public GameLogic()
    {
        wordList = new WordList();
        InitializeGame();
    }

    private async void InitializeGame()
    {
        await wordList.EnsureWordListIsAvailable();
        StartNewGame();
    }

    public void StartNewGame()
    {
        targetWord = wordList.GetRandomWord(); // Assuming GetRandomWord() gets a random word
        currentAttempt = 0;
        gameRunning = true;
    }

    public string CheckGuess(string guessedWord)
    {
        if (!gameRunning || string.IsNullOrWhiteSpace(guessedWord))
            return string.Empty;

        if (guessedWord.Length != targetWord.Length)
            return "Invalid word length"; // Or handle as appropriate

        currentAttempt++;

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

        if (currentAttempt >= MaxAttempts || guessedWord.Equals(targetWord, StringComparison.OrdinalIgnoreCase))
        {
            gameRunning = false; // Game over
        }

        return result.ToString();
    }

    public bool IsGameOver()
    {
        return !gameRunning;
    }
    public bool IsWordGuessedCorrectly(string guessedWord)
    {
        return guessedWord.Equals(targetWord, StringComparison.OrdinalIgnoreCase);
    }
}



