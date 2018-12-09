using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._13
{
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Permutations<T>(this ICollection<T> source)
        {
            if (source.Count == 1)
                yield return new T[] { source.Single() };
            else
                foreach (T item in source)
                    foreach (IEnumerable<T> subPermutation in source.Except(new T[] { item }).ToHashSet().Permutations())
                        yield return new T[] { item }.Concat(subPermutation);
        }
    }
}
