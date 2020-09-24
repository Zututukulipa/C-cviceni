using System;
using System.Windows;

namespace SemestralniPrace_Zdenek_Zalesky.Windows
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        public string TextF { get; private set; }
        private string _placeholder;

        public Dialog(string placeholder)
        {
            InitializeComponent();
            _placeholder = placeholder;
            ResponseTextBox.GotFocus += new RoutedEventHandler(RemoveText);
            ResponseTextBox.LostFocus += new RoutedEventHandler(AddText);
            ResponseTextBox.Text = placeholder;
        }

        private void RemoveText(object sender, EventArgs e)
        {
            if (ResponseTextBox.Text == _placeholder)
            {
                ResponseTextBox.Text = "";
            }
        }

        private void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ResponseTextBox.Text))
                ResponseTextBox.Text = _placeholder;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            TextF = ResponseTextBox.Text;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
