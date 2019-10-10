using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeNumberParallelProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please insert a starting number: ");
            string inputFirst = Console.ReadLine();
            Console.WriteLine("Please insert an ending number: ");
            string inputLast = Console.ReadLine();

            long first = 0;
            long last = 0;

            try
            {
                first = long.Parse(inputFirst);
                last = long.Parse(inputLast);
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }

            GetPrimesParallel(first, last);
            GetPrimesSequential(first, last);

        }

        static List<long> GetPrimesSequential(long first, long last)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<long> primes = new List<long>();

            long current = first;
            int[] calculatePrimes = { 2, 3, 5, 7 };

            for (current = first; current <= last; current++)
            {
                if (current <= 1)
                {
                    continue;
                }

                else if (current % calculatePrimes[0] != 0 && current % calculatePrimes[1] != 0 && current % calculatePrimes[2] != 0 && current % calculatePrimes[3] != 0)
                {
                    primes.Add(current);
                    //Console.WriteLine(current);
                    continue;
                }
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("GetPrimesSequential Finished in: " + elapsedMs);
            Console.WriteLine(primes.Count);
            return primes;


        }

        static List<long> GetPrimesParallel(long first, long last)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<long> primes = new List<long>();

            long current = first;
            int[] calculatePrimes = { 2, 3, 5, 7 };
            CancellationTokenSource cts = new CancellationTokenSource();
            ParallelOptions po = new ParallelOptions()
            {
                CancellationToken = cts.Token,
                MaxDegreeOfParallelism = System.Environment.ProcessorCount
            };

            Task.Factory.StartNew(() =>
            {
                if (Console.ReadKey().KeyChar == 'c')
                    cts.Cancel();
            });

            try
            {
                Parallel.For(first, last + 1, po, (current) =>
                 {
                     if (current <= 1)
                     {
                         return;
                     }

                     else if (current % calculatePrimes[0] != 0 && current % calculatePrimes[1] != 0 &&
                              current % calculatePrimes[2] != 0 && current % calculatePrimes[3] != 0)
                     {
                         lock (primes)
                         {
                             primes.Add(current);
                         }
                         return;
                     }

                 });
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                cts.Dispose();
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("GetPrimesParallel Finished in: " + elapsedMs);
            Console.WriteLine(primes.Count);
            return primes;
        }
    }
}