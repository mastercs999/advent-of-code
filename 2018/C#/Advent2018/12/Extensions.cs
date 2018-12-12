using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._12
{
    public static class Extensions
    {
        public static (T, T, T, T, T) Next<T>(this (T, T, T, T, T) items, T next)
        {
            items.Item1 = items.Item2;
            items.Item2 = items.Item3;
            items.Item3 = items.Item4;
            items.Item4 = items.Item5;
            items.Item5 = next;

            return items;
        }
    }
}
