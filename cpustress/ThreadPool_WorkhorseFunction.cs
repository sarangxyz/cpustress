using System;
using System.Threading;

namespace cpustress
{

    class ThreadPool_WorkerFunction
    {
        private CancellationTokenSource CancellationTokenSource { get; set; }
        private CancellationToken CancellationToken { get; set; }

        public ThreadPool_WorkerFunction()
        {
            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken = CancellationTokenSource.Token;
        }

        public void Start()
        {
            ThreadPool.QueueUserWorkItem(this.ThreadPoolCallback);
        }

        public void Stop()
        {
            CancellationTokenSource.Cancel();
        }

        public void ThreadPoolCallback(Object threadContext)
        {
            WorkHorseFunction();
        }

        private void WorkHorseFunction()
        {
            RandomNumberGeneration_Function();
        }

        private void RandomNumberGeneration_Function()
        {
            byte[] randomBytes = new byte[100];
            Random random = new Random();
            while (!CancellationToken.IsCancellationRequested)
            {
                for(int idx = 0; idx < 50; ++idx)
                    random.NextBytes(randomBytes);
            }
        }
        

        private void Fibonacci_Function()
        {
            Func<int, int> fibRecusive = null;
            fibRecusive = num => (num < 2) ? 1 : fibRecusive(num - 1) + fibRecusive(num - 2);

            while (!CancellationToken.IsCancellationRequested)
            {
                int fib35 = fibRecusive(35);
            }
        }

        //  source:
        //  https://stackoverflow.com/a/39404
        //
        //  PI = 2 * (1 + 1/3 * (1 + 2/5 * (1 + 3/7 * (...))))
        //  --> pi = 2 * f(1)
        //  f(i) --> return 1 + i / (2.0 * i + 1) * f(i + 1);
        //
        private void PiCalculation_Function()
        {
            Func<int, double> piCalc = null;
            piCalc = depth => depth > 40 ? 1.0 : 1.0 + depth / (2.0 * depth + 1) * piCalc(depth + 1);

            while(!CancellationToken.IsCancellationRequested)
            {
                double pi = piCalc(1);
            }
        }
    }
}

