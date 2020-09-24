using SemestralniPrace_Zdenek_Zalesky.Models;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace JelloTests
{
    public class IOManager
    {

        public void SaveTaskList(string outputFilePath, List<GoalList> tasks)
        {
            IFormatter formatter = new BinaryFormatter();
            using Stream stream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, tasks);
        }

        public List<GoalList> LoadTaskList(string inputFilePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using Stream readStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var tasks = (List<GoalList>)formatter.Deserialize(readStream);
            return tasks;
        }
    }
}
