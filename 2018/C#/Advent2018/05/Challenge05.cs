using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._05
{
    public static class Challenge05
    {
        public static void Run()
        {
            // Load input
            List<char> polymer = File.ReadAllText(Path.Combine("05", "input")).Trim().ToList();

            // Task 1
            List<char> reducedPolymer = ReducePolymer(polymer);

            Console.WriteLine(reducedPolymer.Count);

            // Task 2
            Dictionary<char, int> unitToReducedLength = 
                Enumerable.Range('a', 'z' - 'a' + 1)
                .ToDictionary(
                    x => (char)x, 
                    x => ReducePolymer(
                        polymer.Where(y => char.ToLower(y) != x).ToList()
                    ).Count
                );

            KeyValuePair<char, int> troublemaker = unitToReducedLength.OrderBy(x => x.Value).First();

            Console.WriteLine(troublemaker.Value);
        }

        public static List<Char> ReducePolymer(List<char> polymer)
        {
            List<char> reducedPolymer = new List<char>();

            foreach (char unit in polymer)
                if (reducedPolymer.Any() && Reacts(reducedPolymer.Last(), unit))
                    reducedPolymer.RemoveLast();
                else
                    reducedPolymer.Add(unit);

            return reducedPolymer;
        }

        public static bool Reacts(char unit1, char unit2) => char.ToLowerInvariant(unit1) == char.ToLowerInvariant(unit2) && unit1 != unit2;
    }
}
