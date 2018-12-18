using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._18
{
    public static class Extensions
    {
        public static IEnumerable<T> Neighbours<T>(this T[,] source, int x, int y)
        {
            foreach (int dx in new int[] { -1, 0, 1 })
                foreach (int dy in new int[] { -1, 0, 1 })
                    if (dx != 0 || dy != 0)
                    {
                        int newX = x + dx;
                        int newY = y + dy;

                        if (newX >= 0 && newX < source.GetLength(1) && newY >= 0 && newY < source.GetLength(0))
                            yield return source[newY, newX];
                    }
        }
    }
}
