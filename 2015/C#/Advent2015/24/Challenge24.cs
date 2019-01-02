using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._24
{
    public static class Challenge24
    {
        public static void Run()
        {
            // Load input
            int[] packages = File.ReadLines(Path.Combine("24", "input")).Select(int.Parse).ToArray();

            // Task 1
            ulong[] compartmentPackages = Split(packages, 3);

            // Calculate quantum entanglement
            ulong eq = compartmentPackages.Aggregate((x, y) => x * y);

            Console.WriteLine(eq);

            // Task 2
            compartmentPackages = Split(packages, 4);

            // Calculate quantum entanglement
            eq = compartmentPackages.Aggregate((x, y) => x * y);

            Console.WriteLine(eq);
        }

        private static ulong[] Split(int[] packages, int groups)
        {
            // Calculate weight of one part
            int weight = packages.Sum() / groups;

            int? minLength = null;
            return Enumerable.Range(1, packages.Length).SelectMany(x => new Combinations<int>(packages, x).Where(y => y.Sum() == weight)).Select(y => y.Select(z => (ulong)z).ToList()).TakeWhile(x =>
            {
                // There is a sequence which is longer then the first one, we don't need the longer one
                if (minLength.HasValue && x.Count != minLength)
                    return false;

                // Store length of the first sequence
                minLength = x.Count;

                return true;
            }).First().ToArray();
        }
    }
}
