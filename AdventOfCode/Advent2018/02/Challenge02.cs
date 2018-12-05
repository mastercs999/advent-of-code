using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._02
{
    public static class Challenge02
    {
        public static void Run()
        {
            // Load words
            string[] words = File.ReadAllLines(Path.Combine("02", "input"));

            // Task 1
            int twice = words.Count(x => x.GroupBy(y => y).Any(y => y.Count() == 2));
            int threeTimes = words.Count(x => x.GroupBy(y => y).Any(y => y.Count() == 3));

            Console.WriteLine(twice * threeTimes);

            // Task 2
            (string, string) id = words.CartesianJoin(words).Where(x => String.Compare(x.Item1, x.Item2) > 0).Single(x => x.Item1.Zip(x.Item2, (c1, c2) => c1 - c2).Count(y => y != 0) == 1);
            string common = new string(id.Item1.Zip(id.Item2, (c1, c2) => (c1: c1, c2: c2)).Where(x => x.c1 == x.c2).Select(x => x.c1).ToArray());

            Console.WriteLine(common);
        }
    }
}
