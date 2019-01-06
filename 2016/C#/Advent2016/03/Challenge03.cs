using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2016._03
{
    public static class Challenge03
    {
        public static void Run()
        {
            // Load triangles
            List<int[]> triangles = LoadTriangles(Path.Combine("03", "input")).ToList();

            // Task 1
            int valid = triangles.Count(IsValid);

            Console.WriteLine(valid);

            // Task 2
            valid = Enumerable.Range(0, triangles.First().Length).SelectMany(x => triangles.Select(y => y[x])).Select((x, i) => (val: x, index: i)).GroupBy(x => x.index / 3, x => x.val).Count(x => IsValid(x.ToArray()));

            Console.WriteLine(valid);
        }

        private static IEnumerable<int[]> LoadTriangles(string path)
        {
            return File.ReadLines(path).Select(x => x.Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
        }

        private static bool IsValid(int[] sides)
        {
            if (sides.Length != 3)
                throw new InvalidOperationException();

            return
                sides[0] + sides[1] > sides[2] &&
                sides[0] + sides[2] > sides[1] &&
                sides[1] + sides[2] > sides[0];
        }
    }
}
