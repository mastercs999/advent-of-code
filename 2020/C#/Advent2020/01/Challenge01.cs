using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._01
{
    public static class Challenge01
    {
        public static void Run()
        {
            // Load input file
            List<int> input = File.ReadLines(Path.Combine("01", "input.txt")).Select(int.Parse).ToList();

            // Task 1
            (int num1, int num2) = input.CartesianJoin(input).First(x => x.Item1 + x.Item2 == 2020);
            Console.WriteLine(num1 * num2);

            // Task 2
            ((int num3, int num4), int num5) = input.CartesianJoin(input).CartesianJoin(input).First(x => x.Item1.Item1 + x.Item1.Item2 + x.Item2 == 2020);
            Console.WriteLine(num3 * num4 * num5);
        }

        private static (int, int) FindSumNumbers(IList<int> numbers, int sum)
        {
            for (int i = 0; i < numbers.Count - 1; ++i)
                for (int j = i + 1; j < numbers.Count; ++j)
                    if (numbers[i] + numbers[j] == sum)
                        return (numbers[i], numbers[j]);

            throw new InvalidOperationException();
        }

        private static IEnumerable<Tuple<T, U>> CartesianJoin<T, U>(this IEnumerable<T> first, IEnumerable<U> second)
        {
            return first.SelectMany(x => second, (x, y) => Tuple.Create(x, y));
        }
    }
}
