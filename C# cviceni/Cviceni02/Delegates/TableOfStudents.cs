using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Delegate
{
    public class TableOfStudents : Students
    {
        private Dictionary<int, Student> StudentTable { get; set; }
    }

}