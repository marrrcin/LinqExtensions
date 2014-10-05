using System;
using System.Collections.Generic;
using System.Linq;
using LinqExtensions.LambdaWrappers;

namespace LinqExtensions.CustomExtensions
{
    public static class LinqPermutations
    {
        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> enumerable) where T : IComparable<T>
        {
            var used = new HashSet<T>();
            return GeneratePermutations(enumerable, used);
        }

        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> enumerable,IEqualityComparer<T> comparer)
        {
            var used = new HashSet<T>(comparer);
            return GeneratePermutations(enumerable, used);
        }

        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> enumerable,
            Func<T, T, bool> comparer)
        {
            var comparerInstance = new LinqDynamicEqualityComparer<T>(comparer);
            var used = new HashSet<T>(comparerInstance);
            return GeneratePermutations(enumerable, used);
        }

        private static IEnumerable<IEnumerable<T>> GeneratePermutations<T>(IEnumerable<T> enumerable, HashSet<T> used)
        {
            var permutations = new List<T>();
            var enumerableList = enumerable as IList<T> ?? enumerable.ToList();
            return Permutations(enumerableList, permutations, used);
        }

        private static IEnumerable<IEnumerable<T>> Permutations<T>(IList<T> enumerable, List<T> permutations,HashSet<T> used)
        {
            if (permutations.Count == enumerable.Count)
            {
                yield return permutations;
            }
            else
            {
                foreach (var i in enumerable)
                {
                    if (!used.Contains(i))
                    {
                        used.Add(i);
                        permutations.Add(i);
                        foreach (var perm in Permutations(enumerable, permutations, used))
                        {
                            yield return perm;
                        }
                        permutations.Remove(i);
                        used.Remove(i);
                    }
                }
            }
        }
    }
}
