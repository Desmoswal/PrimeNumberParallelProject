using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using PrimeNumberParallelProject;

namespace PrimeNumberGUI
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


        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            P1.Visibility = Visibility.Hidden;
            P2.Visibility = Visibility.Hidden;
            P3.Visibility = Visibility.Hidden;

            long first = 0;
            long last = 0;

            try
            {
                first = long.Parse(startValue.Text);
                last = long.Parse(endValue.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Try using numbers..");
            }

            Calculations(first, last);

            
        }

        public void Calculations(long first, long last)
        {
            List<long> sequentials = new List<long>();
            List<long> parallels = new List<long>();

            P1.Visibility = Visibility.Visible;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            sequentials = Generator.GetPrimesSequential(first, last);
            watch.Stop();
            sequentialListBox.ItemsSource = sequentials;
            sequentialTime.Content = watch.ElapsedMilliseconds;

            P2.Visibility = Visibility.Visible;

            watch = System.Diagnostics.Stopwatch.StartNew();
            parallels = Generator.GetPrimesParallel(first, last);
            watch.Stop();
            parallelListBox.ItemsSource = parallels;
            parallelTime.Content = watch.ElapsedMilliseconds;

            P3.Visibility = Visibility.Visible;

            sequentialItems.Content = sequentials.Count;
            parallelItems.Content = parallels.Count;
        }
    }
}
