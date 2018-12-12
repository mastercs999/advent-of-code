using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._17
{
    public static class Challenge17
    {
        public static void Run()
        {
            // Containers
            int[] containers = File.ReadLines(Path.Combine("17", "input")).Select(int.Parse).ToArray();

            // Task 1
            List<List<int>> possibilities = Find(containers).Select(x => x.ToList()).ToList();
            int possibilitiesCount = possibilities.Count;

            Console.WriteLine(possibilitiesCount);

            // Task 2
            int minCount = possibilities.Min(x => x.Count);
            int minPossibilities = possibilities.Where(x => x.Count == minCount).Count();

            Console.WriteLine(minPossibilities);
        }

        public static IEnumerable<IEnumerable<int>> Find(int[] containers)
        {
            bool[] indexes = new bool[containers.Length];

            do
            {
                int currentIndex = indexes.Length - 1;
                bool overflow = false;

                do
                {
                    indexes[currentIndex] = !indexes[currentIndex];
                    if (!indexes[currentIndex])
                    {
                        overflow = true;
                        --currentIndex;
                        if (currentIndex < 0)
                            break;
                    }
                    else
                        overflow = false;

                } while (overflow);

                IEnumerable<int> used = containers.Zip(indexes, (c, i) => (c: c, i: i)).Where(x => x.i).Select(x => x.c);
                if (used.Sum() == 150)
                    yield return used;

            } while (indexes.Any(x => !x));
        }
    }
}
