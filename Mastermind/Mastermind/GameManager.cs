using System;

namespace App
{
    class GameManager
    {
        private static int MAX_ATTEMPTS = 10;
        private static int MIN_DIGIT = 1;
        private static int MAX_DIGIT = 6;
        private static int NUM_DIGITS = 4;
        private static char CORRECT_INDICATOR = '+';

        private static DigitCounter[] answerDigits = new DigitCounter[NUM_DIGITS];
        private static DigitCounter[] guessDigits = new DigitCounter[NUM_DIGITS];

        public void runGame()
        {
            printInstructions();

            bool playing = true;
            while (playing)
            {
                generateAnswerDigits();

                Console.WriteLine();

                // uncomment for debug
                //Console.WriteLine("{0} {1} {2} {3}", answerDigits[0].Value, answerDigits[1].Value, answerDigits[2].Value, answerDigits[3].Value);

                Console.WriteLine("Enter {0} digits, each with a value between {1} and {2}:", NUM_DIGITS, MIN_DIGIT, MAX_DIGIT);

                int numAttempts = 0;
                while (numAttempts < MAX_ATTEMPTS)
                {
                    getNextGuess();

                    string result = checkResult();
                    Console.WriteLine("  {0}", result);

                    // check if all correct
                    if (isAllCorrect(result))
                    {
                        Console.WriteLine();
                        Console.Write("Congratulations, you won!");
                        break;
                    }

                    // did not win, try again
                    numAttempts++;
                }

                // ran out of attempts
                if (numAttempts >= MAX_ATTEMPTS)
                {
                    Console.WriteLine();
                    Console.Write("Sorry, you lost.");
                }

                Console.Write(" The answer was {0}{1}{2}{3}.", answerDigits[0].Value, answerDigits[1].Value, answerDigits[2].Value, answerDigits[3].Value);

                // check if they want to play again, then keep going or exit
                Console.Write(" Play again (Y/N)? ");
                ConsoleKeyInfo playChar = Console.ReadKey(true);
                playing = char.ToUpper(playChar.KeyChar) == 'Y';
            }
        }

        private void printInstructions()
        {
            Console.WriteLine("*** Welcome to the Game! ***");
            Console.WriteLine("The object of the game is to guess a {0} digit number, with each digit between the numbers {1} and {2}.", NUM_DIGITS, MIN_DIGIT, MAX_DIGIT);
            Console.WriteLine("After each guess, a minus (-) sign will display for every digit that is correct but in the wrong position,");
            Console.WriteLine("and a plus (+) sign will display for every digit that is both correct and in the correct position.");
            Console.WriteLine("Nothing will display for incorrect digits. You will have {0} attempts to guess the number correctly. Good luck!", MAX_ATTEMPTS);
        }

        private void generateAnswerDigits()
        {
            // generate a specific number of random digits, each with a value between the min and max
            Random random = new Random();
            for (int i = 0; i < answerDigits.Length; i++)
            {
                answerDigits[i] = new DigitCounter(random.Next(MIN_DIGIT, MAX_DIGIT + 1));
            }
        }

        private void getNextGuess()
        {
            int numChars = 0;
            while (numChars < NUM_DIGITS)
            {
                // get the next character, but only accept certain values
                ConsoleKeyInfo inputChar = Console.ReadKey(true);
                if (inputChar.KeyChar >= '1' && inputChar.KeyChar <= '6')
                {
                    guessDigits[numChars] = new DigitCounter(int.Parse(inputChar.KeyChar.ToString()));
                    Console.Write(inputChar.KeyChar);
                    numChars++;
                }
                // also allow Backspace to delete the last character
                else if (inputChar.Key == ConsoleKey.Backspace && numChars > 0)
                {
                    Console.Write("\b \b");
                    numChars--;
                }
            }
        }

        private string checkResult()
        {
            string result = "";

            // check the guessed digits, compare against the actual answer
            for (int i = 0; i < guessDigits.Length && i < answerDigits.Length; i++)
            {
                answerDigits[i].Counted = false;  // reset

                // first check for exact matches, correct number and in correct position
                if (guessDigits[i].Value == answerDigits[i].Value)
                {
                    result += CORRECT_INDICATOR;  // display a plus (+) sign to indicate that a digit is both correct and in the correct position
                    guessDigits[i].Counted = true;
                    answerDigits[i].Counted = true;
                }
            }

            for (int i = 0; i < guessDigits.Length; i++)
            {
                if (guessDigits[i].Counted)
                    continue;

                // then check for correct number but in wrong position
                for (int j = 0; j < answerDigits.Length; j++)
                {
                    // make sure that didn't already count this digit/position, so don't count twice
                    if (guessDigits[i].Counted || answerDigits[j].Counted)
                        continue;

                    if (guessDigits[i].Value == answerDigits[j].Value)
                    {
                        result += "-";  // display a minus (-) sign to indicate that a digit is correct but in the wrong position
                        guessDigits[i].Counted = true;
                        answerDigits[j].Counted = true;
                    }
                }

                // (display nothing for incorrect digits)
            }

            return result;
        }

        private bool isAllCorrect(string val)
        {
            if (val.Length != NUM_DIGITS)
                return false;

            foreach (char c in val)
            {
                if (c != CORRECT_INDICATOR)
                    return false;
            }
            
            return true;
        }
    }
}
