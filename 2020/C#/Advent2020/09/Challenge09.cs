using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._09
{
    public static class Challenge09
    {
        public static void Run()
        {
            // Load numbers
            decimal[] numbers = File.ReadLines(Path.Combine("09", "input.txt")).Select(decimal.Parse).ToArray();

            // Task 1
            decimal invalidNumber = Enumerable.Range(0, int.MaxValue).Where(x => !HasSum(numbers[x..(x + 25)], numbers[x + 25])).Select(x => numbers[x + 25]).First();
            Console.WriteLine(invalidNumber);

            // Task 2
            decimal[] set = Enumerable
                .Range(0, int.MaxValue)
                .Select(x => 
                { 
                    decimal sum = 0;
                    return numbers[x..].TakeWhile(y => (sum += y) <= invalidNumber).ToList(); 
                })
                .First(x => x.Sum() == invalidNumber)
                .ToArray();
            decimal sum = set.Min() + set.Max();
            Console.WriteLine(sum);
        }

        private static bool HasSum(decimal[] numbers, decimal sum)
        {
            return numbers.SelectMany(x => numbers, (x, y) => (x, y)).Any(x => x.x != x.y && x.x + x.y == sum);
        }
    }
}
