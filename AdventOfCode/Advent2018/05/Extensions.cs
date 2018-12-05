using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._05
{
    public static class Extensions
    {
        public static void RemoveLast<T>(this List<T> source)
        {
            source.RemoveAt(source.Count - 1);
        }
    }
}
