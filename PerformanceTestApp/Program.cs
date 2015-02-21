using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqExtensions.CustomExtensions;

namespace PerformanceTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const int spread = 1009;
            for (int i = spread; i <= 1551010; i+=50000)
            {
                MergeTest(i, spread);
            }
            
            Console.ReadKey();
        }

        private static void MergeTest(int count, int spread)
        {
            var arrays = new List<int>[spread];
            for (int i = 0; i < spread; i++)
            {
                arrays[i] = new List<int>(count/spread);
            }

            for (int i = 0; i < count; i++)
            {
                arrays[i%spread].Add(i);
            }

            for (int i = 0; i < spread; i++)
            {
                arrays[i].Sort();
            }

            var clock = new Stopwatch();
            clock.Reset();
            clock.Start();
            var result = LinqCollections.Merge((a, b) => a - b, false,arrays).ToList();
            clock.Stop();
            Console.WriteLine("{0}\t{1}",count,clock.ElapsedMilliseconds);
        }
    }
}
