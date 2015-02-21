using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExtensions.CustomExtensions
{
    public static class LinqParallelPermutations
    {
        public static dynamic ParallelPermutations<T>(this IEnumerable<T> enumerable,int threads)
        {
            int perThread = enumerable.Count()/threads;
            List<List<T>> seeds = new List<List<T>>(threads);

            int t = 0;
            for (t = 0; t < threads; t++)
            {
                seeds[t] = new List<T>(perThread);
            }

            t = 0;
            foreach (var item in enumerable)
            {
                seeds[t].Add(item);
                t++;
                if (t > perThread)
                {
                    t = 0;
                }
            }


            return new object();
        }
    }
}
