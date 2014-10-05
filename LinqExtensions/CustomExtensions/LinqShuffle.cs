using System;
using System.Collections.Generic;
using LinqExtensions.LambdaWrappers;

namespace LinqExtensions.CustomExtensions
{
    public static class LinqShuffle
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            var rand = new Random();
            return enumerable.OrderBy(i => i, (a,b) => rand.Next(-1, 2));
        }
    }
}
