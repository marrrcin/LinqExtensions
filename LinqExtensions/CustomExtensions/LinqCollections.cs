using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LinqExtensions.LambdaWrappers;

namespace LinqExtensions.CustomExtensions
{
    public static class LinqCollections
    {
        /// <summary>
        /// Flattens multi-dimensional collections. This method is recursive.
        /// To perform non-recursive depth first flattening set last parameter to true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nested"></param>
        /// <param name="useRecursive">is set to true, non-recursive version of algorithm is used</param>
        /// <returns></returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<dynamic> nested,bool useRecursive=true)
        {
            if (!useRecursive)
            {
                foreach (var unnested in FlattenNonRecursive<T>(nested))
                {
                    yield return unnested;
                }
                yield break;
            }

            foreach (var item in nested)
            {
                if (item is IEnumerable && !(item is T))
                {
                    foreach (var unnested in Flatten<T>(item))
                    {
                        yield return unnested;
                    }
                }
                else
                {
                    yield return (T)item;
                }
            }
        }

        private static IEnumerable<T> FlattenNonRecursive<T>(IEnumerable<dynamic> collection)
        {
            var toFlatten = new Queue<dynamic>();
            toFlatten.Enqueue(collection);
            while (toFlatten.Any())
            {
                dynamic item = toFlatten.Dequeue();
                if (item is IEnumerable && !(item is T))
                {
                    foreach (var i in item)
                    {
                        toFlatten.Enqueue(i);
                    }
                }
                else
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> Merge<T>(Func<T,T,int> compare,params IList<T>[] toMerge)
        {
            toMerge.Aggregate(0, (sum, arrayToMerge) => sum + arrayToMerge.Count);
            var indexes = new int[toMerge.Length];
            var queue = new SortedDictionary<T, int>(new LinqDynamicComparer<T>(compare));

            int completed = 0;
            int i = 0;

            for (i = 0; i < toMerge.Length; i++)
            {
                queue.Add(toMerge[i][0], i);
            }

            KeyValuePair<T, int> top;
            while (completed != toMerge.Length)
            {
                top = queue.First();
                queue.Remove(top.Key);

                ++indexes[top.Value];
                if (indexes[top.Value] != toMerge[top.Value].Count)
                {
                    queue.Add(toMerge[top.Value][indexes[top.Value]],top.Value);
                }
                else
                {
                    ++completed;
                }

                yield return top.Key;
            }
        }

        public static Tuple<int, T> MinWithIndexWithSkipping<T>(this IList<T> collection, Func<T, T, bool> compare,
            int[] indexesToSkip)
        {
            var forbidden = new HashSet<int>(indexesToSkip);
            int firstIndex = 0;
            if (indexesToSkip != null && indexesToSkip.Length > 0)
            {
                Array.Sort(indexesToSkip);
                firstIndex = Enumerable.Range(0, collection.Count).FirstOrDefault(idx => !forbidden.Contains(idx));
            }
            
            T min = collection[firstIndex];
            int index = firstIndex;
            
            for (int idx = firstIndex+1; idx < collection.Count; idx++)
            {
                if(!forbidden.Contains(idx) && compare(min, collection[idx]))
                {
                    min = collection[idx];
                    index = idx;
                }
            }
            return new Tuple<int, T>(index, min);
        }

        public static Tuple<int, T> MinWithIndex<T>(this IList<T> collection,Func<T,T,bool> compare)
        {
            return MinWithIndexWithSkipping(collection, compare, null);
        }
        
    }
}
