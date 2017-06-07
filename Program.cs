using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample03
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }
        public void Run()
        {
            Console.WriteLine("Start Run");

            double result01 = 0;
            for (int i = 1; i < 10000; i++)
            {
                result01 += DoSomeWork(i);
            }
            Console.WriteLine("Result: {0}", result01);

            double result02 = 0;
            object locker = new Object();
            int inits = 0;
            int aggregates = 0;
            Parallel.For<double>(1, 10000,
            () =>
            {
                Interlocked.Increment(ref inits);
                double tls = 0.0;
                return tls;
            },
            (i, pls, tls) =>
            {
                tls += DoSomeWork(i);
                return tls;
            },
            (tls) =>
            {
                Interlocked.Increment(ref aggregates);
                lock (locker)
                {
                    result02 += tls;
                }
            });
            Console.WriteLine("Result: {0}", result02);
            Console.WriteLine("inits: {0}   aggregates: {1}", inits, aggregates);

            Console.WriteLine("End Run");
            Console.ReadLine();
        }
        private double DoSomeWork(int index)
        {
            return Math.Sin(index) + Math.Sqrt(index) * Math.Pow(index, 0.14);
        }
    }
}
