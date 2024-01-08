using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wordle
{
    // This class represents a historical game record, storing information about a player's game attempt.
    public class GameHistory
    {
        // The timestamp when the game attempt was recorded.
        public DateTime Timestamp { get; set; }

        // The correct word that the player was attempting to guess.
        public string CorrectWord { get; set; }

        // The number of guesses made by the player in this game attempt.
        public int Guesses { get; set; }

        // A string representation of an emoji grid, possibly showing the result of the game attempt.
        public string EmojiGrid { get; set; }
    }
}
