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
            var interpreter = new BrainFuckInterpreter(this.CodeTextBox.Text);
            try
            {
                MessageBox.Show(this, interpreter.Run(), "Output");
            }
            catch
            {
                MessageBox.Show(this, "ERROR", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
