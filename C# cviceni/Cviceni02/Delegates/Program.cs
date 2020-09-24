using Delegate;
using System;

namespace Delegates
{
    public delegate void GameLoop();
    class Program
    {
        private static GameLoop loop { get; set; }
        static void Main(string[] args)
        {
            fei.BaseLib.ExtraMath.solveQuadratic(2, 11, 14,out double x1, out double x2);
            Console.WriteLine(fei.BaseLib.MathConvertor.romanToArabic("MMCLI"));
            Console.WriteLine(fei.BaseLib.MathConvertor.arabicToRoman(2151));
            
            Console.WriteLine($"Result:\nx1: {x1}; x2: {x2}");
            loop = PrintMenu;
            loop += ExecuteAction;
            loop();
        }

        private static void PrintMenu()
        {
            Console.Write("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n" +
               "1) Add Student\n" +
               "2) Print out Students\n" +
               "3) Sort by ID\n" +
               "4) Sort by Name\n" +
               "5) Sort by Faculty\n" +
               "0) End\n" +
               "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
        }

        static void ExecuteAction()
        {
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Controller.AddStudent();
                    loop();
                    break;
                case "2":
                    Controller.PrintStudentsToConsole();
                    loop();
                    break;
                case "3":
                    Controller.SortStudentsById();
                    loop();
                    break;
                case "4":
                    Controller.SortStudentsByName();
                    loop();
                    break;
                case "5":
                    Controller.SortStudentsByFaculty();
                    loop();
                    break;
                case "0":
                    break;
                default:
                    Controller.GenStuds();
                    loop();
                    break;
            }
        }
    }
}
