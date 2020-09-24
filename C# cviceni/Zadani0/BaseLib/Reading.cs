using System;

namespace Fei.BaseLib
{
    public class Reading
    {
        /// <summary>
        /// Method prints message then read user input.
        /// If parsing fails, then double.NaN is returned.
        /// </summary>
        /// <param name="message">Message to be printed</param>
        /// <returns>User input as double</returns>
        public static double ReadDouble(string message)
        {
            Console.Write($"{message}: ");
            if (double.TryParse(Console.ReadLine(), out double result))
                return result;

            throw new InvalidCastException("Not a double");
        }

        /// <summary>
        /// Method prints message then read user input.
        /// If parsing fails, then int.NaN is returned.
        /// </summary>
        /// <param name="message">Message to be printed</param>
        /// <returns>User input as int</returns>
        public static int ReadInt(string message)
        {
            Console.Write($"{message}: ");
            if (int.TryParse(Console.ReadLine(), out int result))
                return result;

            throw new InvalidCastException("Not an integer");
        }

        /// <summary>
        /// Method prints message then read user input.
        /// If parsing fails, then exception is returned.
        /// </summary>
        /// <param name="message">Message to be printed</param>
        /// <returns>User input as char</returns>
        public static char ReadChar(string message)
        {
            Console.Write($"{message}: ");
            var input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
                return input[0];

            throw new InvalidCastException("Not a char");
        }

        /// <summary>
        /// Method prints message then read user input.
        /// </summary>
        /// <param name="message">Message to be printed</param>
        /// <returns>User inputted text</returns>
        public static string ReadString(string message)
        {
            Console.Write($"{message}: ");
            return Console.ReadLine();
        }
    }
}
