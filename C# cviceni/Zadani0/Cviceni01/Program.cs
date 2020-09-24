using System;

namespace Cviceni01
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numberArray1 = new int[512];
            int[] numberArray2 = new int[] { 1,2,3};
            int[] numberArray3 = { 1, 2, 3, 4};

            string[] stringArray = new string[10];
            string str = "jfdalkf";
            
            foreach(var item in numberArray1)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();
            for (int i = 0; i < numberArray2.Length; ++i)
            {
                Console.Write($"{numberArray2[i]} ");
            }

            int[,] multiArray = new int[3, 3];
            multiArray[0, 0] = 420;

            int[,] multiArray1 = new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };

            int[,] multiArray2 =  { { 1, 2, 3 }, { 4, 5, 6 }};

            foreach (var number in multiArray)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
            Console.WriteLine();
            for (int j = 0; j < multiArray.GetLength(0); j++)
            {
                for (int i = 0; i < multiArray.GetLength(1); ++i)
                {
                    Console.Write($"{multiArray[j,i]} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n----------");

            int[][] jaggedArray1 = new int[3][];

            jaggedArray1[0] = new int[5] { 1, 2, 3, 4, 5 };
            jaggedArray1[1] = new int[2] { 9, 1 };
            jaggedArray1[2] = new int[3] { 0, 1, 2 };

            for (int j = 0; j < jaggedArray1.Length; j++)
            {
                for (int i = 0; i < jaggedArray1[j].Length; ++i)
                {
                    Console.Write($"{jaggedArray1[j][i]} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n----------");
        }
    }
}
