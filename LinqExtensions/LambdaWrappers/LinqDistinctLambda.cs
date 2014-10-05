using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqExtensions.LambdaWrappers
{
    public static class LinqDistinctLambda
    {
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> enumerable,Func<T, T, bool> comparer, Func<T, int> hasher = null)
        {
            var comparerInstance = new LinqDynamicEqualityComparer<T>(comparer, hasher);
            return enumerable.Distinct(comparerInstance);
        }
    }

    internal class LinqDynamicEqualityComparer<T> : IEqualityComparer<T>
    {
        private Func<T, T, bool> Comparer;
        private Func<T, int> Hasher; 

        public LinqDynamicEqualityComparer(Func<T,T,bool> comparer,Func<T,int> hasher = null)
        {
            Comparer = comparer;
            if (hasher != null)
            {
                Hasher = hasher;
            }
            else
            {
                Hasher = arg => 1;
            }
        }
        public bool Equals(T x, T y)
        {
            return Comparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            if (Hasher == null)
            {
                return obj.GetHashCode();
            }
            return Hasher(obj);
        }
    }
}
