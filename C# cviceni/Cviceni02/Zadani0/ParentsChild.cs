using System;
using System.Collections.Generic;
using System.Text;

namespace Zadani0
{
    class ParentsChild : Parent
    {
        public override void Print()
        {
            Console.WriteLine($"{nameof(ParentsChild)} name: {Name}");
        }
    }
}
