using SemestralniPrace_Zdenek_Zalesky.Models;
using SemestralniPrace_Zdenek_Zalesky.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace SemestralniPrace_Zdenek_Zalesky.Components
{
    /// <summary>
    /// Interaction logic for GoalListBlock.xaml
    /// </summary>
    [Serializable]
    public partial class GoalListBlock : UserControl
    {
        public GoalList Goals { get; set; }
        private byte _backgroundColour = 255;
        
        public GoalListBlock()
        {
            InitializeComponent();
            Goals = new GoalList();
            ListName.Text = Goals.Name;
            Goals.TasksCountEditedEvent += SetGoalCountText;
        }

        public GoalListBlock(GoalList goals)
        {
            InitializeComponent();

            foreach (var goal in Goals.Tasks)
            {
                ListStackPanel.Children.Add(new GoalBlock(goal, this));
            }
            ListName.Text = Goals.Name;
        }

        private void AddToGoalBtn_Click(object sender, RoutedEventArgs e)
        {
            GoalBlock block = new GoalBlock(Goals.Name, this);
            Goals.AddTask(block.Goal);
            ListStackPanel.Children.Add(block);
        }

        private void SetGoalCountText(int count)
        {
            GoalCount.Text = $"Goals: {count}";
        }

        public void RemoveGoal(GoalBlock block)
        {
            Goals.RemoveTask(block.Goal);
            ListStackPanel.Children.Remove(block);
        }

        private void ListName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Dialog dialog = new Dialog(Goals.Name);
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
                ListName.Text = dialog.TextF;
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Object"))
            {
                GoalBlock goal = e.Data.GetData("Object") as GoalBlock;
                goal.ParentBlock.RemoveGoal(goal);
                goal.ParentBlock = this;
                goal.Goal.ListName = Goals.Name;
                Goals.AddTask(goal.Goal);
                ListStackPanel.Children.Add(goal);
            }
        }

        private void ListStackPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("Object") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            var parent = Parent as StackPanel;
            parent.Children.Remove(this);
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
                DimBackGround();
            else
                BrightenBackGround();

            controlBase.Background = GetShade();
        }

        private void BrightenBackGround()
        {
            if (_backgroundColour >= 255)
            {
                _backgroundColour = 0;
            }

            _backgroundColour += 2;
        }

        private void DimBackGround()
        {
            if (_backgroundColour <= 0)
            {
                _backgroundColour = 255;
            }

            _backgroundColour -= 2;
        }

        private SolidColorBrush GetShade()
        {
            return new SolidColorBrush(Color.FromRgb(_backgroundColour, _backgroundColour, _backgroundColour));
        }
    }
}
