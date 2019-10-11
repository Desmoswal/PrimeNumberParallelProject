using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeNumberParallelProject
{
    public class Generator
    {
        public static List<long> GetPrimesSequential(long first, long last)
        {
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
                    continue;
                }
            }

            Console.WriteLine(primes.Count);
            return primes;


        }

        public static List<long> GetPrimesParallel(long first, long last)
        {
            List<long> primes = new List<long>();

            long current = first;
            int[] calculatePrimes = { 2, 3, 5, 7 };

            CancellationTokenSource cts = new CancellationTokenSource();
            ParallelOptions po = new ParallelOptions()
            {
                CancellationToken = cts.Token,
                MaxDegreeOfParallelism = System.Environment.ProcessorCount
            };

            try
            {
                Parallel.For(first, last + 1, po, (current) =>
                 {
                     if (current <= 1)
                     {
                         //return current for iteration function, force to start a new one in parallel. (acts as continue)
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

            Console.WriteLine(primes.Count);
            return primes;
        }
    }
}