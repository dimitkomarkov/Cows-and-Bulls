using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CowsAndBullsGame
{
    public class Mastermind
    {
        public int[] _secretCode;
        public Random _random = new Random();

        public Mastermind(int codeLength)
        {
            _secretCode = new int[codeLength];
            for (int i = 0; i < codeLength; i++)
            {
                _secretCode[i] = _random.Next(0, 10); // generate a random code
            }
        }

        public (int cows, int bulls) Guess(int[] guess)
        {
            int cows = 0;
            int bulls = 0;
            for (int i = 0; i < _secretCode.Length; i++)
            {
                if (guess[i] == _secretCode[i])
                {
                    cows++; // correct digit in correct position
                }
                else if (_secretCode.Contains(guess[i]))
                {
                    bulls++; // correct digit in incorrect position
                }
            }
            return (cows, bulls);
        }

        public double Score(int[] guess, int cows, int bulls)
        {
            double score = 0;
            for (int i = 0; i < _secretCode.Length; i++)
            {
                score += Math.Abs(guess[i]) * Math.Log(Math.Abs(guess[i])) * (cows > 0 ? -2 * Math.Log(2) : 1);
            }
            return score;
        }
    }
}
