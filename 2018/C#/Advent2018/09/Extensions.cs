using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._09
{
    public static class Extensions
    {
        public static LinkedListNode<T> Next<T>(this LinkedListNode<T> node, int steps)
        {
            for (int i = 0; i < steps; ++i)
                node = node.Next ?? node.List.First;
            return node;
        }

        public static LinkedListNode<T> Previous<T>(this LinkedListNode<T> node, int steps)
        {
            for (int i = 0; i < steps; ++i)
                node = node.Previous ?? node.List.Last;

            return node;
        }
    }
}
