using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqExtensions.LambdaWrappers
{
    public static class LinqOrderByLambda
    {
        public static IOrderedEnumerable<T> OrderBy<T,TKey>(this IEnumerable<T> enumerable,Func<T,TKey> keySelector,Func<TKey,TKey,int> comparer)
        {
            var comparerInstance = new LinqDynamicComparer<TKey>(comparer);
            return enumerable.OrderBy(keySelector, comparerInstance);
        }

        public static IOrderedEnumerable<T> OrderByDescending<T, TKey>(this IEnumerable<T> enumerable,
            Func<T, TKey> keySelector, Func<TKey, TKey, int> comparer)
        {
            var comparerInstance = new LinqDynamicComparer<TKey>(comparer);
            return enumerable.OrderByDescending(keySelector, comparerInstance);
        }
    }

    internal class LinqDynamicComparer<T> : IComparer<T>
    {
        private Func<T, T, int> Comparer;
        public LinqDynamicComparer(Func<T,T,int> comparer)
        {
            Comparer = comparer;
        }
        public int Compare(T x, T y)
        {
            return Comparer(x, y);
        }
    }
}
