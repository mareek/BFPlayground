using System;
using System.Linq;
using System.Windows;

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
            CodeTextBox.Text = new Fuzzier().GenerateProgramWithOuput();
        }
    }
}
