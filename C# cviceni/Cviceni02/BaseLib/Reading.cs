using System;

namespace fei
{
    namespace BaseLib
    {
        public class Reading
        {
            public static string initPrompt(string promptMessage)
            {
                Console.WriteLine(promptMessage + ": ");
                var inputString = Console.ReadLine();
                return inputString;
            }

            public static char parseToChar(string promptMessage)
            {
                var inputString = initPrompt(promptMessage);
                if (char.TryParse(inputString, out var returnValue))
                    return returnValue;
                throw new Exception("This string cannot be converted..");
            }

            public static int parseToInteger(string promptMessage)
            {
                var inputString = initPrompt(promptMessage);
                if (int.TryParse(inputString, out var returnValue))
                    return returnValue;
                throw new Exception("This string cannot be converted..");
            }

            public static double parseToDouble(string promptMessage)
            {
                var inputString = initPrompt(promptMessage);
                if (double.TryParse(inputString, out var returnValue))
                    return returnValue;
                throw new Exception("This string cannot be converted..");
            }

            public static string parseToString(string promptMessage)
            {
                var inputString = initPrompt(promptMessage);
                return inputString;
            }
        }
    }
}