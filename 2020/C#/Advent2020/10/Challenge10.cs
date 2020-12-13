using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._10
{
    public static class Challenge10
    {
        public static void Run()
        {
            // Load numbers
            int[] numbers = File.ReadLines(Path.Combine("10", "input.txt")).Select(int.Parse).OrderBy(x => x).ToArray();


            // Task 1
            int[] allnumbers = new int[] { 0 }.Concat(numbers).Concat(new int[] { numbers.Last() + 3 }).ToArray();
            int result = allnumbers.Zip(allnumbers[1..], (x, y) => y - x).GroupBy(x => x).Where(x => x.Key is 1 or 3).Select(x => x.Count()).Aggregate(1, (x, y) => x * y);
            Console.WriteLine(result);

            // Task 2
            ulong combinations = FindCombinations(allnumbers);
            Console.WriteLine(combinations);
        }

        private static ulong FindCombinations(int[] numbers)
        {
            ulong combinations = 1;
            ulong sum = 0;

            for (int i = 0; i < numbers.Length; ++i)
            {
                ulong current = 1;
                for (int j = i + 2; j <= i + 3 && j < numbers.Length; ++j)
                    if (numbers[j] - numbers[i] <= 3)
                        ++current;

                if (current == 1 && sum == 2)
                {
                    combinations *= 2;
                    sum = 0;
                }
                else if (current > 1)
                    sum += current;
                else if (current == 1 && sum > 2)
                {
                    --sum;
                    combinations *= sum;
                    sum = 0;
                }
            }

            return combinations;
        }
    }
}
