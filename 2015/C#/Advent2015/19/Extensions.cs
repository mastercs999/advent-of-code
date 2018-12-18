using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._19
{
    public static class Extensions
    {
        public static IEnumerable<int> IndiciesOf(this string str, string substr)
        {
            int index = str.IndexOf(substr);

            while (index >= 0)
            {
                yield return index;

                index = str.IndexOf(substr, index + 1);
            }
        }
    }
}
