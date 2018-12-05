using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._02
{
    public static class Extensions
    {
        public static IEnumerable<(T, U)> CartesianJoin<T, U>(this IEnumerable<T> first, IEnumerable<U> second)
        {
            return first.SelectMany(x => second, (x, y) => (x, y));
        }
    }
}
