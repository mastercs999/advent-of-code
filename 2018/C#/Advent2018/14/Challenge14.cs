using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._14
{
    public static class Challenge14
    {
        public static void Run()
        {
            int input = 323081;
            int[] inputDigits = input.ToDigits().ToArray();

            // Generate recipes until we have all results we need
            List<int> recipes = new List<int>(input + 20) { 3, 7 };
            int pos1 = 0;
            int pos2 = 1;
            while (recipes.Count < input + 10 || !recipes.EndsWith(inputDigits))
            {
                NextStep(recipes, pos1, pos2);

                pos1 = (pos1 + 1 + recipes[pos1]) % recipes.Count;
                pos2 = (pos2 + 1 + recipes[pos2]) % recipes.Count;
            }

            // Task 1
            // Extract the ten digits
            int[] resultDigits = recipes.Skip(input).Take(10).ToArray();

            Console.WriteLine(String.Join("", resultDigits));

            // Task 2
            int beforeCount = recipes.IndexOf(inputDigits);

            Console.WriteLine(beforeCount);
        }

        private static void NextStep(List<int> recipes, int pos1, int pos2)
        {
            int sum = recipes[pos1] + recipes[pos2];

            int digit1 = sum / 10;
            int digit2 = sum % 10;

            if (digit1 != 0)
                recipes.Add(digit1);
            recipes.Add(digit2);
        }
    }
}
