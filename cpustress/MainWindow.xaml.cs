using System;
using System.Collections.Generic;
using System.Windows;
using MahApps.Metro.Controls;
using System.Timers;
using System.ComponentModel;

namespace cpustress
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private List<ThreadPool_WorkerFunction> _tasks = new List<ThreadPool_WorkerFunction>();
        private CPUUsage CPU = new CPUUsage();
        private Timer _timer = new Timer();

        public MainWindow()
        {
            InitializeComponent();
            sliderCPU.Maximum = SystemInfo.GetNumProcessors();
            this.Title = string.Format("{0} ({1})", this.Title, SystemInfo.GetComputerName());

            _timer.Enabled = true;
            _timer.Interval = 250;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _timer.Stop();
            _timer.Dispose();
        }

        internal void StopTimerOnShutdown()
        {
            _timer.Enabled = false;
            _timer.Stop();
        }


        private static double Clamp(double val, double min, double max)
        {
            if (val < min)
                return min;
            else if (val > max)
                return max;
            return val;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this)
            {
                CPU.Update();
                this.Dispatcher.Invoke(() =>
                {
                    var cpuVal = Clamp(CPU.CPU, 0.0, 100.0);
                    _cpuDial.Value = cpuVal;

                    _cpuLabel.Content = string.Format("CPU {0} %", cpuVal.ToString("N2"));
                });
            }
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

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutBox dialog = new AboutBox();
            dialog.Owner = this;
            dialog.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            dialog.ShowDialog();
        }
    }
}
