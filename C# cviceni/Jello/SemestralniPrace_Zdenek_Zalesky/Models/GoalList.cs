using System;
using System.Collections.Generic;

namespace SemestralniPrace_Zdenek_Zalesky.Models
{
    [Serializable]
    public class GoalList
    {
        public string Name { get; set; }
        public List<Goal> Tasks;
        public event Action<int> TasksCountEditedEvent;
        public GoalList(string name)
        {
            Name = name;
            Tasks = new List<Goal>();
        }

        public GoalList()
        {
            Name = "New List";
            Tasks = new List<Goal>();
        }

        public void AddTask(Goal task)
        {
            Tasks.Add(task);
            TasksCountEditedEvent?.Invoke(Tasks.Count);
        }

        public void RemoveTask(Goal task)
        {
            Tasks.Remove(task);
            TasksCountEditedEvent?.Invoke(Tasks.Count);
        }


    }
}
