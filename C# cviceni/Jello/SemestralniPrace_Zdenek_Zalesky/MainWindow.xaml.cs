using JelloTests;
using Microsoft.Win32;
using SemestralniPrace_Zdenek_Zalesky.Components;
using SemestralniPrace_Zdenek_Zalesky.Models;
using System.Collections.Generic;
using System.Windows;

namespace SemestralniPrace_Zdenek_Zalesky
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddListButton_Click(object sender, RoutedEventArgs e)
        {
            var element = new GoalListBlock();
            ListStackPanel.Children.Add(element);
        }

        private void SaveListButton_Click(object sender, RoutedEventArgs e)
        {
            List<GoalList> goals = new List<GoalList>();
            foreach (GoalListBlock child in ListStackPanel.Children)
            {
                goals.Add(child.Goals);
            }
            IOManager mngr = new IOManager();
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Serialized Goals (*.gdat)|*.gdat";
            if (dialog.ShowDialog() == true)
            {
                mngr.SaveTaskList(dialog.FileName, goals);
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            ListStackPanel.Children.Clear();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Serialized Goals (*.gdat)|*.gdat";
            IOManager mngr = new IOManager();
            if (dialog.ShowDialog() == true)
            {
                var lists = mngr.LoadTaskList(dialog.FileName);
                foreach (var element in lists)
                {
                    ListStackPanel.Children.Add(new GoalListBlock(element));
                }
            }
        }
    }
}
