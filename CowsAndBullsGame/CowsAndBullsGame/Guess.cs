using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CowsAndBullsGame
{
    public class Guess
    {
        public string GuessString { get; set; }
        public int Cows { get; set; }
        public int Bulls { get; set; }

        public Guess(string guess, int cows, int bulls)
        {
            GuessString = guess;
            Cows = cows;
            Bulls = bulls;
        }
    }
}
