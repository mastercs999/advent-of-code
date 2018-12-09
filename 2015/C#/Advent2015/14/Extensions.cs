using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._14
{
    public static class Extensions
    {
        public static IEnumerable<int>CumulativeSum(this IEnumerable<int> source)
        {
            int sum = 0;
            foreach (int n in source)
                yield return sum += n;
        }
    }
}
