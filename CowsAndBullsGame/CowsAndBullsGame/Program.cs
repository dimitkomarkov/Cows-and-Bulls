using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CowsAndBullsGame
{
    internal class Program
    {
        static int[][] everyDigit = new int[][]
        {
            new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0},
            new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0},
            new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0},
            new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0}
        };
        static List<Guess> botGuesses = new List<Guess>();
        static bool the4Algorithm = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Cows and Bulls game!");
            Console.WriteLine("The bot will try to guess your 4-digit number.");
            Console.WriteLine("You will try to guess the bot's 4-digit number.");
            Console.WriteLine();

            string userSecretNumber = GetUserSecretNumber();
            string botSecretNumber = GenerateSecretNumber();
            int userAttempts = 0;
            int botAttempts = 0;

            // For testing
            Console.WriteLine(botSecretNumber);

            // User guessing bot’s number
            while (true)
            {
                Console.Write("Enter your guess: ");
                string userGuess = Console.ReadLine();

                if (!IsValidNumber(userGuess))
                {
                    Console.WriteLine("Please enter a valid 4-digit number with no repeating digits.");
                    continue;
                }

                userAttempts++;
                (int cows, int bulls) = CalculateCowsAndBulls(botSecretNumber, userGuess);
                Console.WriteLine($"Cows: {cows}, Bulls: {bulls}");
                if (bulls == 4)
                {
                    Console.WriteLine($"Congratulations! You've guessed the bot's number in {userAttempts} attempts.");
                    break;
                }
            }

            // Bot guessing user’s number
            while (true)
            {
                string botGuess = GenerateBotGuess(botAttempts);
                (int cows, int bulls) = CalculateCowsAndBulls(userSecretNumber, botGuess);

                botGuesses.Add(new Guess(botGuess, cows, bulls));
                Console.Write($" Bot's guess: {botGuess}");
                //foreach (var a in allDigits)
                //{
                //    Console.Write(a);
                //}
                /*foreach (var subArray in everyDigit)
                {
                    Console.WriteLine(string.Join(" ", subArray));
                }*/

                botAttempts++;

                Console.WriteLine($"Cows: {cows}, Bulls: {bulls}");

                if (bulls == 4)
                {
                    Console.WriteLine($"Bot guessed your number in {botAttempts} attempts.");
                    break;
                }

                UpdatePossibleDigits(botGuess, cows, bulls);
            }

            if (botAttempts == userAttempts)
            {
                Console.WriteLine($"It`s a draw! You both guessed in: {botAttempts}");
            }
            else if (botAttempts < userAttempts)
            {
                Console.WriteLine($"The Bot won! By: {userAttempts - botAttempts}");
            }
            else if (botAttempts > userAttempts)
            {
                Console.WriteLine($"YOU WOOON YEEEY! By: {botAttempts - userAttempts}");
            }
        }
        static string GetUserSecretNumber()
        {
            while (true)
            {
                Console.Write("Enter a 4-digit number with no repeating digits: ");
                string input = Console.ReadLine();
                //string input = "1746";

                if (IsValidNumber(input))
                {
                    return input;
                }

                Console.WriteLine("Invalid number. Please enter a 4-digit number with no repeating digits.");
            }
        }

        static bool IsValidNumber(string number)
        {
            if (number.Length != 4 || !int.TryParse(number, out _))
            {
                return false;
            }

            return number.Distinct().Count() == 4;
        }

        static string GenerateSecretNumber()
        {
            Random random = new Random();
            char[] digits = new char[4];
            int index = 0;

            while (index < 4)
            {
                char digit = (char)('0' + random.Next(0, 10));

                if (Array.IndexOf(digits, digit, 0, index) == -1)
                {
                    digits[index] = digit;
                    index++;
                }
            }

            return new string(digits);
        }

        static (int cows, int bulls) CalculateCowsAndBulls(string secret, string guess)
        {
            int cows = 0;
            int bulls = 0;
            bool[] secretMatched = new bool[4];
            bool[] guessMatched = new bool[4];

            // Calculate bulls
            for (int i = 0; i < 4; i++)
            {
                if (guess[i] == secret[i])
                {
                    bulls++;
                    secretMatched[i] = true;
                    guessMatched[i] = true;
                }
            }

            // Calculate cows
            for (int i = 0; i < 4; i++)
            {
                if (!guessMatched[i])
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (!secretMatched[j] && guess[i] == secret[j])
                        {
                            cows++;
                            secretMatched[j] = true;
                            break;
                        }
                    }
                }
            }

            return (cows, bulls);
        }

        static void UpdatePossibleDigits(string guess, int cows, int bulls)
        {
            if (bulls == 0 && cows == 0)
            {
                everyDigit = RemoveAll(everyDigit, guess);
            }
            else if (bulls + cows == 4)
            {
                everyDigit = ReadyArray(everyDigit, guess);
            }
            if (bulls == 0 && cows > 0)
            {
                everyDigit = RemovePositions(everyDigit, guess);
            }
        }

        static int[][] ReadyArray(int[][] jaggedArray, string numbersToKeep)
        {
            int[] keepNumbers = numbersToKeep.Select(c => int.Parse(c.ToString())).ToArray();

            int[][] result = jaggedArray
                .Select(subArray => subArray.Where(num => keepNumbers.Contains(num)).ToArray())
                .ToArray();

            return result;
        }

        static int[][] RemovePositions(int[][] jaggedArray, string numbersToRemove)
        {
            List<int> removeNumbers = numbersToRemove.Select(c => int.Parse(c.ToString())).ToList();

            int[][] result = new int[jaggedArray.Length][];
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                List<int> subArray = jaggedArray[i].ToList();

                if (subArray.Contains(removeNumbers[i]))
                {
                    subArray.Remove(removeNumbers[i]);
                }

                result[i] = subArray.ToArray();
            }

            return result;
        }

        static int[][] RemoveAll(int[][] jaggedArray, string numbersToRemove)
        {
            List<int> removeNumbers = numbersToRemove.Select(c => int.Parse(c.ToString())).ToList();

            int[][] result = new int[jaggedArray.Length][];
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                List<int> subArray = jaggedArray[i].ToList();

                foreach (int num in removeNumbers)
                {
                    subArray.Remove(num);
                }

                result[i] = subArray.ToArray();
            }

            return result;
        }

        static string GenerateBotGuess(int botAttempts)
        {
            if (botAttempts == 0)
            {
                return "1234";
            }
            else if (botAttempts == 1)
            {
                return "4567";
            }
            else if (botAttempts == 2)
            {
                return "8409";
            }
            else if (botAttempts == 4)
            {
                if (botGuesses[0].Cows + botGuesses[0].Bulls +
                    botGuesses[1].Cows + botGuesses[1].Bulls +
                    botGuesses[2].Cows + botGuesses[2].Bulls == 4)
                {
                    //doesnt contains 4
                    the4Algorithm = false;

                    everyDigit[0] = everyDigit[0].Where(val => val != 4).ToArray();
                    everyDigit[1] = everyDigit[1].Where(val => val != 4).ToArray();
                    everyDigit[2] = everyDigit[2].Where(val => val != 4).ToArray();
                    everyDigit[3] = everyDigit[3].Where(val => val != 4).ToArray();
                }
                else
                {
                    //contains 4
                    the4Algorithm = true;
                    if (botGuesses[0].Bulls + botGuesses[1].Bulls + botGuesses[2].Bulls == 0)
                    {
                        everyDigit[2] = everyDigit[2].Where(val => val == 4).ToArray();
                    }
                    if (botGuesses[0].Cows + botGuesses[0].Bulls == 1)
                    {
                        everyDigit = RemoveAll(everyDigit, botGuesses[0].GuessString.Replace("4", ""));
                    }
                    if (botGuesses[1].Cows + botGuesses[1].Bulls == 1)
                    {
                        everyDigit = RemoveAll(everyDigit, botGuesses[1].GuessString.Replace("4", ""));
                    }
                    if (botGuesses[2].Cows + botGuesses[2].Bulls == 1)
                    {
                        everyDigit = RemoveAll(everyDigit, botGuesses[2].GuessString.Replace("4", ""));
                    }
                }
            }
            if (the4Algorithm)
            {

            }
            else
            {

            }
            Random random = new Random();
            HashSet<string> allPossibleGuesses = new HashSet<string>();

            // Generate all possible 4-digit numbers with unique digits
            for (int i = 0; i < everyDigit[0].Length; i++)
            {
                for (int j = 0; j < everyDigit[1].Length; j++)
                {
                    for (int k = 0; k < everyDigit[2].Length; k++)
                    {
                        for (int g = 0; g < everyDigit[3].Length; g++)
                        {
                            string nums = everyDigit[0][i].ToString() + everyDigit[1][j].ToString() + everyDigit[2][k] + everyDigit[3][g];
                            if (nums.Distinct().Count() == 4)
                            {
                                allPossibleGuesses.Add(nums);
                            }
                        }
                        
                    }
                }
            }

            // Remove previous guesses
            foreach (var feedback in botGuesses)
            {
                allPossibleGuesses.Remove(feedback.GuessString);
            }

            // Randomly select a guess from the remaining possibilities
            return allPossibleGuesses.ElementAt(random.Next(allPossibleGuesses.Count));
        }
    }
}
