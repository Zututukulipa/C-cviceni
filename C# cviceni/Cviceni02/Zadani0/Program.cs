using System;
using System.Text;

namespace Zadani0
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            StringBuilder builder = new StringBuilder("ABCD",50);

            builder.Append("EFGHIJKLMNOPQRSTUVWXYZ");
            builder.Append(new char[] { 'A', 'B', 'C' });

            Console.WriteLine($"Length: {builder.Length}, String: {builder.ToString()}");

            Parent parent = new Parent { Name = "Johnny Nozka" };
            parent.Print();

            ParentsChild child = new ParentsChild { Name = "Johanka Nozkova" };
            child.Print();

        }
    }
}
