using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        private int _numberOfThreads;
        private Primes _primes = new Primes();

        public MainWindow()
        {
            InitializeComponent();

            this.physicalCoreCountTextBox.IsEnabled = false;
            this.physicalCoreCountTextBox.Text = Utility.PhysicalCoreCount.ToString();

            this.LogicalCoreCountTextBox.IsEnabled = false;
            this.LogicalCoreCountTextBox.Text = Utility.LogicalCoreCount.ToString();

            this.NumberOfThreadsTextBox.TextChanged += this.NumberOfThreadsTextBox_TextChanged;

            this.TimeTo10MilTextBox.IsEnabled = false;
            this.TimeTo100MilTextBox.IsEnabled = false;
            this._primes.TenMilEvent += this._primes_TenMilEvent;
            this._primes.HundredMilEvent += this._primes_HundredMilEvent;
        }

        private void NumberOfThreadsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.NumberOfThreadsTextBox.Text) && !int.TryParse(this.NumberOfThreadsTextBox.Text.Trim(), out this._numberOfThreads))
            {
                this.NumberOfThreadsTextBox.Clear();
                MessageBox.Show("Enter only integer.", "Info", MessageBoxButton.OK);
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            this._primes.ThreadCount = this._numberOfThreads;

            Thread thread = new Thread(this._primes.RunParallel);
            thread.Start();

            BackgroundWorker progressWorker = new BackgroundWorker();
            progressWorker.DoWork += this.Progress;
            progressWorker.RunWorkerAsync();
        }

        private void _primes_TenMilEvent(TimeSpan time, EventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() => this.TimeTo10MilTextBox.Text = time.ToString()));
        }

        private void _primes_HundredMilEvent(TimeSpan time, EventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() => this.TimeTo100MilTextBox.Text = time.ToString()));
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._primes != null)
            {
                this._primes.Stop = true;
            }
        }

        private void Progress(object sender, DoWorkEventArgs e)
        {
            while (!this._primes.Stop)
            {
                this.Dispatcher.Invoke((Action)(() => this.PrimesCalculationProgressBar.Value = ((double)this._primes.ActualNumber / (this._primes.Limit != 0 ? this._primes.Limit : ulong.MaxValue)) * 100));
                Thread.Sleep(250);
            }
        }
    }
}
