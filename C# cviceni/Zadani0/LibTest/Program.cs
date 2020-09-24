using System;

namespace LibTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var arr = new int[1];
            while (true)
            {
                try
                {
                    arr = loop(arr);
                }
                catch
                {
                    break;
                }
            }
        }

        private static int[] loop(int[] arr)
        {
            Console.Write("Hello World!\nLets try to use BaseLib!\n1. Add elements into the field\n2. print out the field\n3. sort field" +
                "\n4. Find minimum \n5. Find first appearance of number\n6. Find last appearance of number\n7. Exit\n>");
            if (int.TryParse(Console.ReadLine(), out int choice))
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter array length:");
                        if (int.TryParse(Console.ReadLine(), out int length))
                        {
                            arr = new int[length];
                            for (int i = 0; i < length; ++i)
                            {
                                arr[i] = Fei.BaseLib.Reading.ReadInt($"Insert [{i}] element:");
                            }
                            Console.WriteLine();
                        }
                        return arr;
                    case 2:
                        foreach (var number in arr)
                        {
                            Console.Write($"{number} ");
                        }
                        return arr;
                    case 3:
                        Array.Sort(arr);
                        return arr;
                    case 4:
                        int min = int.MaxValue;
                        foreach (var number in arr)
                        {
                            if (number < min)
                                min = number;
                        }
                        Console.WriteLine($"Minimum is: {min}");
                        return arr;
                    case 5:
                        var searched = Fei.BaseLib.Reading.ReadInt("Insert searched word:");
                        for (var i = 0; i < arr.Length; ++i)
                        {
                            if (arr[i] == searched)
                            {
                                Console.WriteLine($"{arr[i]} found at index [{i}]");
                                break;
                            }

                        }
                        return arr;

                    case 6:
                        var search = Fei.BaseLib.Reading.ReadInt("Insert searched word:");
                        var lastIndex = -1;
                        for (var i = 0; i < arr.Length; ++i)
                        {
                            if (arr[i] == search)
                            {
                                lastIndex = i;
                            }

                        }
                        if(lastIndex >= 0)
                            Console.WriteLine($"{arr[lastIndex]} found at index [{lastIndex}]");
                        else
                            Console.WriteLine("Not found :(");

                        return arr;
                    default:
                        throw new Exception("break");
                }
            return null;
        }
    }
}
