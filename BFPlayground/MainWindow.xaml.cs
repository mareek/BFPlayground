using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BFPlayground
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

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private string ExecuteBF(string program)
        {
            var data = new List<byte> { 0 };
            int cursor = 0;

            Action MoveCursorRight = () =>
            {
                if (cursor == data.Count - 1)
                    data.Add(0);
                cursor++;
            };

            Action MoveCursorLeft = () =>
            {
                if (cursor > 0)
                    cursor++;
            };

            Action Increment = () => data[cursor] += 1;
            Action Decrement = () => data[cursor] -= 1;

            var output = new StringBuilder();

            return output.ToString();
        }
    }
}
