using System.Collections.Generic;

namespace Delegate
{
    public class Students
    {
        public List<Student> StudentList { get; set; }

        public void appendStudent(Student studToAppend)
        {
            StudentList.Add(studToAppend);
        }

        public Students()
        {
            StudentList = new List<Student>();
        }

        public Students(List<Student> studentList)
        {
            StudentList = studentList;
        }
    }
}