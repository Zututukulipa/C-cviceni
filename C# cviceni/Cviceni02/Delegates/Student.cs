namespace Delegate
{
    public class Student
    {
        public string name { get; }
        public int id { get; }
        public Faculty faculty { get; set; }

        public Student(string name, int id, Faculty faculty)
        {
            this.name = name;
            this.id = id;
            this.faculty = faculty;
        }

        public override string ToString()
        {
            return $"ID: {id}\nName: {name}\nFaculty: {faculty.ToString()}";
        }
    }
}