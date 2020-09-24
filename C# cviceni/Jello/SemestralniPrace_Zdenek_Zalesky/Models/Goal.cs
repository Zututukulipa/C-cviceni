using System;
using System.Collections.Generic;

namespace SemestralniPrace_Zdenek_Zalesky.Models
{
    [Serializable]
    public class Goal
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string ListName { get; set; }
        public List<string> Labels { get; set; }
        public List<string> AttachmentFilePaths { get; set; }
        public List<(string, bool?)> CheckListLabels { get; set; }
        public DateTimeOffset DueTime { get; set; }
        public bool DueTimeSet { get; set; }

    }
}
