using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeCalculator
{
    public class SieveOfEratosthenesCalculator
    {
        public List<int> MakeSieve(int max, IProgress<double> progress = null, CancellationToken ct = new CancellationToken())
        {
            ct.ThrowIfCancellationRequested();

            progress?.Report(0);
            // Make an array indicating whether numbers are prime.
            List<int> primes = new List<int>(max / 10);
            bool[] isPrime = new bool[max + 1];
            for (int i = 2; i <= max; i++)
            {
                isPrime[i] = true;
            }
            ct.ThrowIfCancellationRequested();

            // Cross out multiples.
            for (int i = 2; i <= max; i++)
            {
                // See if i is prime.
                if (isPrime[i])
                {
                    // Knock out multiples of i.
                    for (int j = i * 2; j <= max && !ct.IsCancellationRequested; j += i)
                        isPrime[j] = false;
                    primes.Add(i);
                }
                progress?.Report((2.0 * max - i + 1.0) * i / (max * (max + 1.0)));
                ct.ThrowIfCancellationRequested();
            }
            return primes;
            //return isPrime.Select((v, i) => new {v, i}).Where(x => x.v).Select(x => x.i);
        }


        public async Task<List<int>> MakeSieveAsync(int max, IProgress<double> progress = null, CancellationToken ct = new CancellationToken())
        {
            var task = Task.Run(() => MakeSieve(max, progress, ct), ct);
            return await task;
        }
        
    }
}
