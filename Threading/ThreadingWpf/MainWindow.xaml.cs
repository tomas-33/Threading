using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThreadingLibrary;

namespace ThreadingWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

            this.physicalCoreCountTextBox.IsEnabled = false;
            this.physicalCoreCountTextBox.Text = Utility.PhysicalCoreCount.ToString();

            this.LogicalCoreCountTextBox.IsEnabled = false;
            this.LogicalCoreCountTextBox.Text = Utility.LogicalCoreCount.ToString();
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
