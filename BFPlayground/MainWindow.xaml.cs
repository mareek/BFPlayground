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
            this.CodeTextBox.Text = "[-]>[-]<>+++++++[<+++++++>-]<+++.--.";
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show(this, ExecuteBF(this.CodeTextBox.Text), "Output");
            }
            catch
            {
                MessageBox.Show(this, "ERROR", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string ExecuteBF(string program)
        {
            var instructions = "<>+-[].,";
            var code = program.Where(c => instructions.Contains(c)).ToArray();
            var codePointer = 0;
            var lastOpeningBracket = -1;

            var data = new List<byte> { 0 };
            int dataPointer = 0;

            var output = new List<byte>();

            Action MoveCursorRight = () =>
            {
                if (dataPointer == data.Count - 1)
                    data.Add(0);
                dataPointer++;
            };

            Action MoveCursorLeft = () =>
            {
                if (dataPointer == 0)
                    throw new ApplicationException("Moved pointer before the beggining of the data");
                else
                    dataPointer--;
            };

            Action Increment = () => data[dataPointer] += 1;

            Action Decrement = () => data[dataPointer] -= 1;

            Action OpeningBracket = () =>
            {
                lastOpeningBracket = codePointer;
                if (data[dataPointer] == 0)
                {
                    while (codePointer < code.Length && code[codePointer] != ']')
                        codePointer++;
                    if (codePointer == code.Length)
                    {
                        throw new ApplicationException("No matching closing bracket");
                    }
                }
            };

            Action ClosingBracket = () =>
            {
                if (data[dataPointer] == 0)
                { /*Do nothing*/ }
                else if (lastOpeningBracket >= 0)
                    codePointer = lastOpeningBracket;
                else
                    throw new ApplicationException("No matching opening bracket");
            };

            Action Input = () => { };
            Action Output = () => output.Add(data[dataPointer]);

            while (codePointer < code.Length)
            {
                switch (code[codePointer])
                {
                    case '>':
                        MoveCursorRight();
                        break;
                    case '<':
                        MoveCursorLeft();
                        break;
                    case '+':
                        Increment();
                        break;
                    case '-':
                        Decrement();
                        break;
                    case '[':
                        OpeningBracket();
                        break;
                    case ']':
                        ClosingBracket();
                        break;
                    case ',':
                        Input();
                        break;
                    case '.':
                        Output();
                        break;
                }
                codePointer++;
            }

            return new string(UTF8Encoding.UTF8.GetChars(output.ToArray()));
        }
    }
}
