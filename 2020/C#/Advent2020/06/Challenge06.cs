using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._06
{
    public static class Challenge06
    {
        public static void Run()
        {
            // Load input
            List<string[]> answers = LoadAnswers(Path.Combine("06", "input.txt"));

            // Task 1
            int sum1 = answers.Sum(x => x.SelectMany(y => y).Distinct().Count());
            Console.WriteLine(sum1);

            // Task 2
            int sum2 = answers.Sum(x => x.Select(y => y.Cast<char>()).Aggregate((y, z) => y.Intersect(z)).Count());
            Console.WriteLine(sum2);
        }

        private static List<string[]> LoadAnswers(string filePath)
        {
            return File
                .ReadAllText(filePath)
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x
                    .Split((char[])null, StringSplitOptions.RemoveEmptyEntries))
                .ToList();
        }
    }
}
