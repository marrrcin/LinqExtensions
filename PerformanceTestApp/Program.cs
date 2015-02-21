using System;
using System.Collections.Generic;
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
            MergeTest();
            Console.ReadKey();
        }

        private static void MergeTest()
        {
            const int spread = 1009;
            const int count = 1000000;
            var arrays = new List<int>[spread];
            for (int i = 0; i < spread; i++)
            {
                arrays[i] = new List<int>(count/spread);
            }

            for (int i = 0; i < count; i++)
            {
                arrays[i%spread].Add(i);
            }

            var result = LinqCollections.Merge((a, b) => a > b, arrays).ToList();
            Console.WriteLine(result[0]);
        }
    }
}
