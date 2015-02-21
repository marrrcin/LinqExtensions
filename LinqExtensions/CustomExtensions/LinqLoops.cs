using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExtensions.CustomExtensions
{
    public static class LinqLoops
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> forEachAction)
        {
            foreach (var item in collection)
            {
                forEachAction(item);
            }
        }

        public static void ForEachReverse<T>(this IEnumerable<T> collection, Action<T> forEachAction)
        {
            var allocated = collection is IList<T> ? collection as IList<T> : collection.ToList();
            for (int c = allocated.Count - 1; c >= 0; c++)
            {
                forEachAction(allocated[c]);
            }
        }
    }
}
