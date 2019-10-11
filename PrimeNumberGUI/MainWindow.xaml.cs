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
            ResetUI();

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

        private void CleanUo()
        {
            GC.Collect(0, GCCollectionMode.Forced);
            GC.Collect(1, GCCollectionMode.Forced);
            GC.Collect(2, GCCollectionMode.Forced);
        }

        private void ResetUI()
        {
            P1.Content = "Calculating Sequential list...";
            P2.Content = "Calculating Parallel list...";
            P3.Content = "Sorting parallel array...";
            P1.Visibility = Visibility.Hidden;
            P2.Visibility = Visibility.Hidden;
            P3.Visibility = Visibility.Hidden;
            sequentialListBox.ItemsSource = null;
            parallelListBox.ItemsSource = null;
            sequentialTime.Content = "";
            sequentialItems.Content = "";
            parallelTime.Content = "";
            parallelItems.Content = "";
        }

        public void Calculations(long first, long last)
        {
            List<long> sequentials = new List<long>();
            List<long> parallels = new List<long>();
            long seqTime = 0;
            long parTime = 0;

            P1.Visibility = Visibility.Visible; //sequential start

            Task.Factory.StartNew(() =>
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                sequentials = Generator.GetPrimesSequential(first, last);
                watch.Stop();
                seqTime = watch.ElapsedMilliseconds;

            }).GetAwaiter().OnCompleted(() =>
            {
                P1.Content = (string)P1.Content + "done";
                sequentialListBox.ItemsSource = sequentials;
                sequentialTime.Content = seqTime + " ms";
                sequentialItems.Content = sequentials.Count + " items";
                CleanUo();
            });

            P2.Visibility = Visibility.Visible; //parallel start

            Task.Factory.StartNew(() =>
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                parallels = Generator.GetPrimesParallel(first, last);
                watch.Stop();
                parTime = watch.ElapsedMilliseconds;
            }).GetAwaiter().OnCompleted(() =>
            {
                P2.Content = (string)P2.Content + "done";
                P3.Visibility = Visibility.Visible; //sorting start
                Task.Factory.StartNew(() => parallels.Sort()).GetAwaiter().OnCompleted(() =>
                {
                    P3.Content = (string) P3.Content + "done";
                    parallelListBox.ItemsSource = parallels; //only show parallel array after sorting
                    CleanUo();
                });
                
                parallelTime.Content = parTime + " ms";
                parallelItems.Content = parallels.Count + " items";
                
            });
        }
    }
}
