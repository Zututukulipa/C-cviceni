using System;
using System.Data;
using System.Linq;

namespace Delegate
{
    public class Controller
    {
        static Students students = new Students();
        public static void AddStudent()
        {
            Console.Write("Please enter students name:\n");
            var name = Console.ReadLine();
            Console.Write("Please set students ID:\n");
            if (!int.TryParse(Console.ReadLine(), out var id))
                throw new InvalidCastException("ID should be a number [0-9]!");

            Console.Write("Please choose students faculty:\n" +
                          "1) FES\n" +
                          "2) FF\n" +
                          "3) FEI\n" +
                          "4) FCHT\n");

            if (!int.TryParse(Console.ReadLine(), out var option))
                throw new InvalidCastException("Option should be a number [0-9]!");

            setStudentsFaculty(option, out var faculty);
            var student = new Student(name, id, faculty);
            students.appendStudent(student);
        }

        private static void setStudentsFaculty(int option, out Faculty faculty)
        {
            switch (option)
            {
                case 1:
                    faculty = Faculty.FES;
                    break;
                case 2:
                    faculty = Faculty.FF;
                    break;
                case 3:
                    faculty = Faculty.FEI;
                    break;
                case 4:
                    faculty = Faculty.FCHT;
                    break;
                default:
                    throw new InvalidExpressionException("Assign a valid faculty!");
            }
        }

        public static void PrintStudentsToConsole()
        {
            foreach (var student in students.StudentList)
            {
                Console.Write($"\n--------------------------------------\n" +
                              $"{student.ToString()}" +
                              $"\n--------------------------------------\n");
            }
        }

        public static void SortStudentsById()
        {
            students.StudentList.Sort((student, student1) => student.id.CompareTo(student1.id));
            PrintStudentsToConsole();
        }

        public static void SortStudentsByName()
        {
            students.StudentList.Sort((stud1, stud2) => string.Compare(stud1.name, stud2.name));
            PrintStudentsToConsole();
        }

        public static void SortStudentsByFaculty()
        {
            students.StudentList.Sort(((student, student1) => (int)student.faculty.CompareTo(student1.faculty)));
            PrintStudentsToConsole();

        }

        public static void GenStuds()
        {
            Random r = new Random();
            for (int i = 0; i < 31; i++)
            {
                students.StudentList.Add(new Student(string.Concat("STUDENT", i.ToString()), r.Next(1000), EnumCaller.getRandomFaculty()));
            }
        }
    }
}