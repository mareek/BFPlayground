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
        private BrainFuckInterpreter _interpreter;
        public MainWindow()
        {
            InitializeComponent();
            this.CodeTextBox.Text = "[-]>[-]<>+++++++[<+++++++>-]<+++.--.";
        }

        private void InitInterpreter()
        {
            if (_interpreter == null 
                || _interpreter.EndOfProgram
                || _interpreter.Program != this.CodeTextBox.Text)
            {
                _interpreter = new BrainFuckInterpreter(this.CodeTextBox.Text);
                this.OutputTextBox.Clear();
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
            catch(Exception ex)
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
            this.OutputTextBox.Text = _interpreter.Output;
            
            this.DataListBox.ItemsSource = _interpreter.Data;
            this.DataListBox.SelectedIndex = _interpreter.DataPointer;

            if (!_interpreter.EndOfProgram)
            {
                this.CodeTextBox.SelectionStart = _interpreter.ProgramPointer;
                this.CodeTextBox.SelectionLength = 1;
                this.CodeTextBox.Focus();
            }
        }
    }
}
