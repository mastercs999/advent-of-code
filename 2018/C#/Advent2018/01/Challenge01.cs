using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._01
{
    public static class Challenge01
    {
        public static void Run()
        {
            // Load input
            List<int> numbers = Load(Path.Combine("01", "input")).ToList();

            // Task 1
            int result = numbers.Sum();

            Console.WriteLine(result);

            // Task 2
            HashSet<int> seen = new HashSet<int>();
            int first = InfiniteSum(numbers).First(x => !seen.Add(x));

            Console.WriteLine(first);
        }

        public static IEnumerable<int> Load(string path)
        {
            return File.ReadLines(path).Select(int.Parse);
        }

        public static IEnumerable<int> InfiniteSum(List<int> sequence)
        {
            int i = 0;
            int sum = 0;

            while (true)
            {
                yield return sum = sum + sequence[i++];

                i %= sequence.Count;
            }
        }
    }
}
