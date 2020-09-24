using Microsoft.Win32;
using SemestralniPrace_Zdenek_Zalesky.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SemestralniPrace_Zdenek_Zalesky.Windows
{
    /// <summary>
    /// Interaction logic for EditGoalWindow.xaml
    /// </summary>
    public partial class EditGoalWindow : Window
    {
        public Goal EditedGoal { get; set; }
        private event Action GoalEdited;

        public EditGoalWindow(Goal goal)
        {
            InitializeComponent();
            EditedGoal = goal;
            ReminderPicker.SelectedDateChanged += ReminderPicker_SelectedDateChanged;
            GoalEdited += RedrawTextFields;
            GoalEdited += FillLabelsAttachmentsCheckboxes;
            GoalEdited?.Invoke();
        }

        private void ReminderPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditedGoal.DueTime < DateTimeOffset.Now)
                EditedGoal.DueTimeSet = false;
            else
                EditedGoal.DueTimeSet = true;
        }

        private void FillLabelsAttachmentsCheckboxes()
        {
            int labelCount = EditedGoal.Labels.Count;
            int attachmentCount = EditedGoal.AttachmentFilePaths.Count;
            int checkListCount = EditedGoal.CheckListLabels.Count;
            ClearMinimizedLists();
            for (int i = 0; i < labelCount; i++)
            {
                LabelsStack.Children.Add(ConstructLabel(EditedGoal.Labels[i]));
            }
            for (int i = 0; i < attachmentCount; i++)
            {
                AttachmentsPanel.Children.Add(ConstructAttachment(EditedGoal.AttachmentFilePaths[i]));
            }
            for (int i = 0; i < checkListCount; i++)
            {
                ConstructCheckBoxDuo(EditedGoal.CheckListLabels[i].Item1, EditedGoal.CheckListLabels[i].Item2);
            }
        }

        private void ClearMinimizedLists()
        {
            LabelsStack.Children.Clear();
            AttachmentsPanel.Children.Clear();
            CheckBoxStack.Children.Clear();
        }

        private void RedrawTextFields()
        {
            NameText.Text = EditedGoal.Name;
            DescriptionText.Text = EditedGoal.Description;
            ReminderPicker.SelectedDate = EditedGoal.DueTime.DateTime;
        }

        private TextBlock ConstructLabel(string label)
        {
            var block = new TextBlock();
            block.Text = label;
            block.Margin = new Thickness(0, 0, 5, 0);
            block.MouseDown += LabelClicked;
            block.IsMouseDirectlyOverChanged += Block_IsMouseDirectlyOverChanged;
            return block;
        }

        private void Block_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var text = sender as TextBlock;
            if (text.Foreground != Brushes.Red)
                text.Foreground = Brushes.Red;
            else
                text.Foreground = Brushes.Black;
        }

        private TextBlock ConstructAttachment(string path)
        {
            var block = new TextBlock();
            block.Text = path;
            block.MouseLeftButtonDown += Attachment_Clicked;
            block.MouseRightButtonUp += RemoveAttachment;
            block.IsMouseDirectlyOverChanged += Block_IsMouseDirectlyOverChanged;
            return block;
        }

        private void Attachment_Clicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var attachmentPath = sender as TextBlock;
            if (attachmentPath.Text != null)
            {
                if (File.Exists(attachmentPath.Text))
                {
                    Process.Start("explorer.exe", attachmentPath.Text);
                }
            }
        }

        private CheckBox ConstructCheckBox(string prompt, bool? isChecked)
        {
            CheckBox box = new CheckBox();
            box.Tag = prompt;
            box.IsChecked = isChecked;
            box.VerticalContentAlignment = VerticalAlignment.Center;
            return box;
        }

        private void ConstructCheckBoxDuo(string name, bool? checkedBox)
        {
            StackPanel duo = new StackPanel();
            duo.Orientation = Orientation.Horizontal;
            var box = ConstructCheckBox(name, checkedBox);
            Label lbl = ConstructCheckBoxLabel((string)box.Tag);
            EditedGoal.CheckListLabels.Add((name, checkedBox));
            duo.Children.Add(box);
            duo.Children.Add(lbl);
            CheckBoxStack.Children.Add(duo);
        }

        private static Label ConstructCheckBoxLabel(string text)
        {
            var lbl = new Label();
            lbl.Content = text;
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            return lbl;
        }

        private void AddLabel_Click(object sender, RoutedEventArgs e)
        {
            Dialog dialog = new Dialog("Label Name");
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                EditedGoal.Labels.Add(dialog.TextF);
                GoalEdited?.Invoke();
            }
        }

        private void LabelClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var label = sender as TextBlock;
            if (label != null)
            {
                var clickedLabel = EditedGoal.Labels.FirstOrDefault(x => x.Equals(label.Text));
                EditedGoal.Labels.Remove(clickedLabel);
                GoalEdited?.Invoke();
            }
        }

        private void RemoveAttachment(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var label = sender as TextBlock;
            if (label != null)
            {
                var clickedLabel = EditedGoal.AttachmentFilePaths.FirstOrDefault(x => x.Equals(label.Text));
                EditedGoal.AttachmentFilePaths.Remove(clickedLabel);
                GoalEdited?.Invoke();
            }
        }

        private void AddAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                EditedGoal.AttachmentFilePaths.Add(dialog.FileName);
                GoalEdited?.Invoke();
            }
        }

        private void AddCheckBoxButton_Click(object sender, RoutedEventArgs e)
        {
            Dialog dialog = new Dialog("Goal");
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                ConstructCheckBoxDuo(dialog.TextF, false);
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            EditedGoal.Name = NameText.Text;
            EditedGoal.Description = DescriptionText.Text;
            EditedGoal.DueTime = DateTime.SpecifyKind(ReminderPicker.SelectedDate.Value, DateTimeKind.Utc);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = null;
            this.Close();
        }
    }
}
