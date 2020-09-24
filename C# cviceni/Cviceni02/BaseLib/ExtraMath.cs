using System;

namespace fei
{
    namespace BaseLib
    {
        public class ExtraMath
        {
            /// <summary>
            /// Method tries to solve a Quadratic Rational Equation. If the Equation is solvable method returns True.
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <param name="c"></param>
            /// <param name="x1"></param>
            /// <param name="x2"></param>
            /// <returns></returns>
            public static bool solveQuadratic(double a, double b, double c, out double x1, out double x2)
            {
                double d = Math.Pow(b, 2) - (4 * a * c);
                if (d == 0)
                {
                    x1 = ((-b) / 2 * a);
                    x2 = ((-b) / 2 * a);
                    return true;
                }

                if (d > 0)
                {
                    x1 = (((-b) + Math.Sqrt(d)) / (2 * a));
                    x2 = (((-b) - Math.Sqrt(d)) / (2 * a));
                    return true;
                }

                x1 = 0;
                x2 = 0;
                return false;
            }

            /// <summary>
            /// Method returns a random double Value in a given [min,max] range.
            /// </summary>
            /// <param name="randomInstance"></param>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public static double getRandomDoubleValue(Random randomInstance, double min, double max)
            {
                return randomInstance.NextDouble() * (max - min) + min;
            }
        }
    }
}