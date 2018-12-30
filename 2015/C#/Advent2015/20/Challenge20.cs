using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._20
{
    public static class Challenge20
    {
        public static void Run()
        {
            int input = 36000000;

            // Task 1
            int maxHouse1 = Task1(input);

            Console.WriteLine(maxHouse1);

            // Task 2
            int maxHouse2 = Task2(input);

            Console.WriteLine(maxHouse2);
        }

        private static int Task1(int input)
        {
            for (int maxHouse = 1; maxHouse < int.MaxValue; ++maxHouse)
            {
                int presents = Divisors(maxHouse).Sum(x => x * 10);

                if (presents > input)
                    return maxHouse;
            }

            throw new InvalidOperationException();
        }

        private static int Task2(int input)
        {
            for (int maxHouse = 1; maxHouse < int.MaxValue; ++maxHouse)
            {
                int presents = Divisors(maxHouse).Where(x => x * 50 >= maxHouse).Sum(x => x * 11);

                if (presents > input)
                    return maxHouse;
            }

            throw new InvalidOperationException();
        }

        private static IEnumerable<int> Divisors(int number)
        {
            double sqrt = Math.Sqrt(number) - 0.00000001;

            for (int div = 1; div < sqrt; ++div)
                if (number % div == 0)
                {
                    yield return div;
                    yield return number / div;
                }

            int roundSqrt = (int)Math.Sqrt(number);
            if ((int)sqrt < roundSqrt && number % roundSqrt == 0)
                yield return roundSqrt;
        }
    }
}
