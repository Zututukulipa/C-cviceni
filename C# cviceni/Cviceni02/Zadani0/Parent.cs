using System;
using System.Collections.Generic;
using System.Text;

namespace Zadani0
{
    class Parent
    {
        public string Name { get; set; }

        public virtual void Print()
        {

            Console.WriteLine($"{nameof(Parent)} name: {Name}");
        }
    }
}
