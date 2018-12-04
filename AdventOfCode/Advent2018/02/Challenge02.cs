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
            words.CartesianJoin(words).Where(x => x.Item1 != x.Item2).Single(x => )
        }

        public static char? Diff(string str1, string str2)
        {
            bool haveId = str1.Zip(str2, (c1, c2) => c1 - c2).Count(x => x != 0) == 1;
            if (!haveId)
                return null;
        }
    }
}
