using System;
using System.Collections.Generic;
using System.Windows;

namespace cpustress
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ThreadPool_WorkerFunction> _tasks = new List<ThreadPool_WorkerFunction>();

        public MainWindow()
        {
            InitializeComponent();
            sliderCPU.Maximum = SystemInfo.GetNumProcessors();
            this.Title = string.Format("{0} \t (Machine: {1})", this.Title, SystemInfo.GetComputerName());
        }

        private void sliderCPU_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int newVal = (int)e.NewValue;
            int oldVal = (int)e.OldValue;

            int diff = newVal - oldVal;

            if (diff > 0)
            {
                for (int idx = 0; idx < diff; ++idx)
                {
                    ThreadPool_WorkerFunction wkFn = new ThreadPool_WorkerFunction();
                    _tasks.Add(wkFn);
                    wkFn.Start();
                }
            }
            else if (diff < 0)
            {
                int absDiff = -diff;
                for (int idx = 0; idx < absDiff; ++idx)
                {
                    _tasks[idx].Stop();
                    _tasks.RemoveAt(idx);
                }
            }
        }
    }
}
