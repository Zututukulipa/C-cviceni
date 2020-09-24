using System;
using System.Collections.Generic;
using System.Text;

namespace fei
{
    namespace BaseLib {
        public class MathConvertor
        {

            /// <summary>
            /// Method returns a string of converted integer into a decimal form.
            /// </summary>
            /// <param name="decimalValue"></param>
            /// <returns></returns>
            public static string decimalToBinary(int decimalValue)
            {
                var result = string.Empty;
                while (decimalValue > 0)
                {
                    var remainder = decimalValue % 2;
                    decimalValue /= 2;
                    result = remainder + result;
                }

                return result;
            }

            /// <summary>
            /// Reverse method to decimalToBinary.
            /// Method converts binary string into integer value.
            /// </summary>
            /// <param name="binaryValue"></param>
            /// <returns></returns>
            public static int binaryToDecimal(string binaryValue)
            {
                return Convert.ToInt32(binaryValue, 2);
            }

            /// <summary>
            /// Method converts Roman string into Arabic decimal Value.
            /// </summary>
            /// <param name="romanValue"></param>
            /// <returns></returns>
            public static int romanToArabic(string romanValue)
            {
                var romanNumberValues = new Dictionary<char, int>
            {
                { 'I', 1 },
                { 'V', 5 },
                { 'X', 10 },
                { 'L', 50 },
                { 'C', 100 },
                { 'D', 500 },
                { 'M', 1000 },
            };

                var total = 0;
                var previousRoman = '\0';

                foreach (var number in romanValue)
                {
                    var currentRoman = number;

                    int previous;

                    if (previousRoman != '\0')
                        previous = romanNumberValues[previousRoman];
                    else
                        previous = '\0';

                    var current = romanNumberValues[currentRoman];

                    if (previous != 0 && current > previous)
                    {
                        total = total - 2 * previous + current;
                    }
                    else
                    {
                        total += current;
                    }

                    previousRoman = currentRoman;
                }
                return total;
            }

            /// <summary>
            /// Method converts Integer value into String of Roman Literals representing its Value.
            /// </summary>
            /// <param name="arabicDecimalValue"></param>
            /// <returns></returns>
            public static string arabicToRoman(int arabicDecimalValue)
            {
                var DecimalRomanValues = new Dictionary<int, string>
            {
                { 1000, "M" },
                { 900, "CM" },
                { 500, "D" },
                { 400, "CD" },
                { 100, "C" },
                { 50, "L" },
                { 40, "XL" },
                { 10, "X" },
                { 9, "IX" },
                { 5, "V" },
                { 4, "IV" },
                { 1, "I" },
            };
                var roman = new StringBuilder();

                foreach (var item in DecimalRomanValues)
                {
                    while (arabicDecimalValue >= item.Key)
                    {
                        roman.Append(item.Value);
                        arabicDecimalValue -= item.Key;
                    }
                }

                return roman.ToString();
            }
        }
    }
}