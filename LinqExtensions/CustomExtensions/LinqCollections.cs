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

        public static IEnumerable<T> Merge<T>(Func<T, T, int> compare, bool removeDuplicates, params IList<T>[] toMerge)
        {
            var indexes = new int[toMerge.Length];
            var comparer = new LinqDynamicComparer<T>(compare);
            var queue = new SortedDictionary<T, LinkedList<Tuple<int, T>>>(comparer);
            var unique = new SortedSet<T>(comparer);

            int completed = 0;
            int i = 0;
            T current;
            for (i = 0; i < toMerge.Length; i++)
            {
                current = toMerge[i][0];
                AddToQueue(removeDuplicates, unique, current, queue, i);
            }

            KeyValuePair<T, LinkedList<Tuple<int, T>>> top;
            int idx;
            while (completed != toMerge.Length && queue.Count != 0)
            {
                top = queue.First();
                queue.Remove(top.Key);    

                foreach (var pair in top.Value)
                {
                    idx = pair.Item1;
                    current = pair.Item2;
                    yield return current;

                    ++indexes[idx];
                    if (indexes[idx] != toMerge[idx].Count)
                    {
                        AddToQueue(removeDuplicates,unique,current,queue,idx);
                    }
                    else
                    {
                        ++completed;
                    }
                }
            }
        }

        private static void AddToQueue<T>(bool removeDuplicates, SortedSet<T> unique, T current, SortedDictionary<T, LinkedList<Tuple<int, T>>> queue, int i)
        {
            if (!removeDuplicates || !unique.Contains(current))
            {
                AddToQueue(queue, current, i);
                if (removeDuplicates)
                {
                    unique.Add(current);
                }
            }
        }

        public static IEnumerable<T> Merge<T>(Func<T,T,int> compare,params IList<T>[] toMerge)
        {
            return Merge(compare, false, toMerge);
        }

        private static void AddToQueue<T>(SortedDictionary<T, LinkedList<Tuple<int, T>>> queue, T current, int i)
        {
            if (!queue.ContainsKey(current))
            {
                queue.Add(current, new LinkedList<Tuple<int, T>>());
            }
            queue[current].AddLast(new Tuple<int, T>(i, current));
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
