using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace BFPlayground
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Interpreter _interpreter;
        public MainWindow()
        {
            InitializeComponent();
            CodeTextBox.Text = "[-]>[-]<>+++++++[<+++++++>-]<+++.--.";
        }

        private void InitInterpreter()
        {
            if (_interpreter == null
                || _interpreter.EndOfProgram
                || _interpreter.Program != CodeTextBox.Text)
            {
                _interpreter = new Interpreter(CodeTextBox.Text);
                OutputTextBox.Clear();
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            InitInterpreter();
            try
            {
                _interpreter.Run();
                ShowResults();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Step_Click(object sender, RoutedEventArgs e)
        {
            InitInterpreter();
            try
            {
                _interpreter.Step();
                ShowResults();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowResults()
        {
            const int maxDataDisplayed = 500;
            OutputTextBox.Text = _interpreter.Output;
            DataListBox.ItemsSource = _interpreter.Data.Take(maxDataDisplayed);
            DataListBox.SelectedIndex = _interpreter.DataPointer > maxDataDisplayed ? -1 : _interpreter.DataPointer;

            if (!_interpreter.EndOfProgram)
            {
                CodeTextBox.SelectionStart = _interpreter.ProgramPointer;
                CodeTextBox.SelectionLength = 1;
                CodeTextBox.Focus();
            }
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            CodeTextBox.Text = new Fuzzer().GenerateProgramWithOuput();
        }

        private void GenerateCorpus_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new SaveFileDialog
            {
                Title = "Save program corpus",
                FileName = "program corpus.txt",
                Filter = "text file|*.txt"
            };

            if (fileDialog.ShowDialog(this) ?? false)
            {
                var corpus = GenerateProgramCorpus(100);

                using (var fileStream = fileDialog.OpenFile())
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(corpus);
                }
            }
        }

        private string GenerateProgramCorpus(int nbPrograms)
        {
            var fuzzier = new Fuzzer();
            var resultBuilder = new StringBuilder();
            for (int i = 1; i <= nbPrograms; i++)
            {
                var program = fuzzier.GenerateProgramWithOuput();
                var output = GetProgramOutput(program);
                var title = $"Program n°{i:000} - length : {program.Length} - output : [{string.Join(", ", output)}]";
                resultBuilder.AppendLine(title);
                resultBuilder.AppendLine(program);
                resultBuilder.AppendLine(Convert.ToBase64String(output));
            }

            return resultBuilder.ToString();
        }

        private static byte[] GetProgramOutput(string program)
        {
            var interpreter = new Interpreter(program);
            interpreter.Run();
            return interpreter.BinaryOutput;
        }
    }
}
