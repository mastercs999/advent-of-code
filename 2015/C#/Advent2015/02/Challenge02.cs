using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._02
{
    public static class Challenge02
    {
        public static void Run()
        {
            List<Present> presents = LoadPresents(Path.Combine("02", "input")).ToList();

            // Task 1
            int totalArea = presents.Sum(x => x.Area + x.SmallestArea);

            Console.WriteLine(totalArea);

            // Task 2
            int ribbonLength = presents.Sum(x => x.RibbonWrap + x.RibbonBow);

            Console.WriteLine(ribbonLength);
        }

        public static IEnumerable<Present> LoadPresents(string path)
        {
            return File.ReadLines(path).Select(x =>
            {
                int[] parts = x.Split('x').Select(int.Parse).ToArray();

                return new Present(parts[0], parts[1], parts[2]);
            });
        }
    }
}
