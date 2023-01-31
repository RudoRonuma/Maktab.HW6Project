using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW6Project.Utility
{
    public static class InputHelper
    {
        /// <summary>
        /// This method does the same job as <see cref="GetConsoleInput(string, int)"/>,
        /// but will mask the input with asterisk ('*') character.
        /// </summary>
        /// <param name="prompt">
        /// The prompt message to be shown to the user when trying to get their input.
        /// </param>
        /// <param name="minLen">
        /// The minimum allowed lenght for the input value to be returned from this
        /// method. As said before, if the user-input's minimum length is less than this
        /// value, this method will keep asking the user to provide a valid input.
        /// </param>
        /// <returns>
        /// The user input which is >= <paramref name="minLen"/>.
        /// </returns>
        public static string GetSecureConsoleInput(string prompt, int minLen = 8)
        {
            string secureInput;
            while (true)
            {
                Console.Write(prompt);
                GetSecureString();
                if (secureInput.Length >= minLen)
                    break;

                Console.WriteLine(
                    $"Sorry, invalid input provided, min len is {minLen}.\n" +
                    "please try again."
                );
            }

            return secureInput;

            void GetSecureString()
            {
                ConsoleKey key;
                secureInput = string.Empty;
                do
                {
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if (key == ConsoleKey.Backspace && secureInput.Length > 0)
                    {
                        Console.Write("\b \b");
                        secureInput = secureInput[0..^1];
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Console.Write("*");
                        secureInput += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);

                Console.Write("\n");
            }
        }
        /// <summary>
        /// Does kinda the same job as <see cref="GetConsoleInput(string, int)"/>,
        /// but this time, this method will invoke the lambda <paramref name="selector"/>
        /// on the user input (to parse the value to type <typeparamref name="T"/>).
        /// </summary>
        /// <typeparam name="T">
        /// The specified type that we will have to return from this method.
        /// </typeparam>
        /// <param name="prompt">
        /// The prompt message to be shown to the user.
        /// </param>
        /// <param name="selector">
        /// The lambda to be called on the user-input to convert it to the specified
        /// type of <typeparamref name="T"/>.
        /// </param>
        /// <returns>
        /// An instance of type <typeparamref name="T"/> which is parsed from the
        /// user's input in console.
        /// </returns>
        public static T GetConsoleInput<T>(string prompt, Func<string, T> selector)
        {
            Console.Write(prompt);
            return selector.Invoke(Console.ReadLine()!);
        }
        /// <summary>
        /// Shows the <paramref name="prompt"/> value to the user and gets
        /// an string input (if the input's length didn't satisfy the minimum length
        /// requirement, this method will continue getting input from user until it does :).
        /// </summary>
        /// <param name="prompt">
        /// The prompt message to be shown to the user when trying to get their input.
        /// </param>
        /// <param name="minLen">
        /// The minimum allowed lenght for the input value to be returned from this
        /// method. As said before, if the user-input's minimum length is less than this
        /// value, this method will keep asking the user to provide a valid input.
        /// </param>
        /// <returns>
        /// The user input which is >= <paramref name="minLen"/>.
        /// </returns>
        public static string GetConsoleInput(string prompt, int minLen = 1)
        {
            string userInput;
            while (true)
            {
                Console.Write(prompt);
                userInput = Console.ReadLine()!;
                if (userInput.Length >= minLen)
                {
                    break;
                }

                Console.WriteLine($"Sorry, invalid input provided, min length is {minLen}," +
                    "please try again.");
            }

            return userInput;
        }

    }
}
