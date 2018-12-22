using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._22
{
    public static class Extensions
    {
        public static IEnumerable<TResult> ToQueryable<TSource, TResult>(this TSource[,] source, Func<int, int, TSource, TResult> conversionFunc)
        {
            for (int y = 0; y < source.GetLength(0); ++y)
                for (int x = 0; x < source.GetLength(1); ++x)
                    yield return conversionFunc(x, y, source[y, x]);
        }
    }
}
