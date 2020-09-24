using SemestralniPrace_Zdenek_Zalesky.Models;
using SemestralniPrace_Zdenek_Zalesky.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SemestralniPrace_Zdenek_Zalesky.Components
{
    /// <summary>
    /// Made to store ONE Goal only
    /// </summary>
    public partial class GoalBlock : UserControl
    {
        public Goal Goal { get; set; }
        public GoalListBlock ParentBlock { get; set; }

        public GoalBlock(string listName, GoalListBlock parent)
        {
            InitializeComponent();
            ParentBlock = parent;
            Goal = new Goal()
            {
                Name = "New Goal",
                Description = "Default Description",
                DueTime = DateTimeOffset.Now,
                Labels = new List<string>(),
                AttachmentFilePaths = new List<string>(),
                CheckListLabels = new List<(string, bool?)>(),
                ListName = listName,
                DueTimeSet = false
            };
            SetFields(Goal);
        }

        public GoalBlock(Goal goal, GoalListBlock parent)
        {
            ParentBlock = parent;
            InitializeComponent();
            Goal = goal;
            SetFields(goal);
        }

        private void SetFields(Goal goal)
        {

            if (goal.DueTime != null && !goal.DueTimeSet)
                DueTimeText.Text = goal.DueTime.ToString("dd/MM hh:mm");
            if (DateTimeOffset.Now > goal.DueTime && goal.DueTime != DateTimeOffset.MinValue && goal.DueTimeSet)
                Background = Brushes.DarkRed;
            else
                Background = Brushes.White;

            if (goal.AttachmentFilePaths != null)
                AttachmentText.Text = ChainAttachments(goal.AttachmentFilePaths);
            if (goal.Labels != null)
                LabelsText.Text = ChainLabels(goal.Labels);
            GoalNameText.Text = goal.Name;
        }

        private string ChainAttachments(List<string> attachments)
        {
            return $"Attachments: {attachments.Count}";
        }

        private string ChainLabels(List<string> labels)
        {
            if (labels.Count > 0)
            {
                StringBuilder bldr = new StringBuilder(labels[0]);
                for (int i = 1; i < labels.Count; i++)
                {
                    bldr.Append($" {labels[i]}");
                }
                return bldr.ToString();
            }
            return "";
        }

        private void GoalPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            EditGoalWindow dialog = new EditGoalWindow(Goal);
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                Goal = dialog.EditedGoal;
                if (Goal != null)
                    SetFields(Goal);
            }
            else
            {
                Goal = dialog.EditedGoal;
                if (Goal != null)
                    ParentBlock.RemoveGoal(this);
            }
            
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject();
                data.SetData("Object", this);

                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
        }
    }
}
