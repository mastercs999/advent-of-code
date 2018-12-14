using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._14
{
    public static class Extensions
    {
        public static IEnumerable<int> ToDigits(this int number)
        {
            IEnumerable<int> toDigitsReversed(int num)
            {
                do
                {
                    int rest = num % 10;
                    yield return rest;

                    num /= 10;
                } while (num > 0);
            }

            return toDigitsReversed(number).Reverse();
        }

        public static bool EndsWith<T>(this IList<T> source, IList<T> end)
        {
            for (int i = 0; i < end.Count; ++i)
                if (!source[source.Count - 1 - end.Count + i].Equals(end[i]))
                    return false;

            return true;
        }

        public static int IndexOf<T>(this IList<T> source, IList<T> subsequnce)
        {
            for (int i = 0; i < source.Count - subsequnce.Count; ++i)
            {
                if (!source[i].Equals(subsequnce[0]))
                    continue;

                for (int j = 1; j < subsequnce.Count; ++j)
                    if (!source[i + j].Equals(subsequnce[j]))
                        goto loopEnd;

                return i;

                loopEnd:;
            }

            return -1;
        }
    }
}
