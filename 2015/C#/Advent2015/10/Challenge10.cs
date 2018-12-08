using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._10
{
    public static class Challenge10
    {
        public static void Run()
        {
            // The input
            string inputText = "1321131112";
            int[] inputNumbers = inputText.Select(x => x - '0').ToArray();

            // Task 1
            // Do 40 iterations
            for (int i = 0; i < 40; ++i)
                inputNumbers = GenerateNext(inputNumbers).ToArray();

            // Length of the result
            int length = inputNumbers.Length;

            Console.WriteLine(length);

            // Task 2
            // Do next 10 iterations
            for (int i = 0; i < 10; ++i)
                inputNumbers = GenerateNext(inputNumbers).ToArray();

            // Length of the result
            length = inputNumbers.Length;

            Console.WriteLine(length);
        }

        public static IEnumerable<int> GenerateNext(int[] input)
        {
            int index = 0;
            while (index < input.Length)
            {
                int end;
                for (end = index + 1; end < input.Length; ++end)
                    if (input[index] != input[end])
                        break;
                int count = end - index;

                yield return count;
                yield return input[index];

                index += count;
            }
        }
    }
}
