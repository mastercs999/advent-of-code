using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._03
{
    public static class Challenge03
    {
        public static void Run()
        {
            // Load input
            string[] lines = File.ReadAllLines(Path.Combine("03", "input.txt"));

            // Task 1
            int trees = CountTrees(lines, 3, 1);
            Console.WriteLine(trees);

            // Task 2
            ulong minTrees = new (int dx, int dy)[] {
                (1, 1),
                (3, 1),
                (5, 1),
                (7, 1),
                (1, 2),
            }
            .Select(x => (ulong)CountTrees(lines, x.dx, x.dy))
            .Aggregate(1ul, (x, y) => x * y);
            Console.WriteLine(minTrees);
        }

        public static int CountTrees(string[] lines, int dx, int dy)
        {
            int column = 0;
            int trees = 0;
            for  (int i = 0; i < lines.Length; i += dy)
            {
                if (lines[i][column] == '#')
                    ++trees;

                column = (column + dx) % lines[i].Length;
            }

            return trees;
        }
    }
}
