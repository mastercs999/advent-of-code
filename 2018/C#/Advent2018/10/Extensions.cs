using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._10
{
    public static class Extensions
    {
        public static void Move(this IEnumerable<Star> stars)
        {
            foreach (Star star in stars)
                star.Move();
        }

        public static IEnumerable<(T, T)> CartesianJoinUnique<T>(this IList<T> source)
        {
            for (int i = 0; i < source.Count; ++i)
                for (int j = i + 1; j < source.Count; ++j)
                    yield return (source[i], source[j]);
        }
    }
}
