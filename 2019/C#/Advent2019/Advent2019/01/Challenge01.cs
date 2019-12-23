using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent2019._01
{
    public static class Challenge01
    {
        public static void Run()
        {
            // Load modules
            List<int> modules = File.ReadLines(Path.Combine("01", "input")).Select(int.Parse).ToList();

            // Task 1
            int fuel = modules.Sum(CalculateFuel);

            Console.WriteLine(fuel);

            // Task 2
            int fuelR = modules.SelectMany(x => CalculateFuelR(x).TakeWhile(y => y > 0)).Sum();

            Console.WriteLine(fuelR);
        }

        private static int CalculateFuel(int mass)
        {
            return (int)Math.Floor(mass / 3.0) - 2;
        }
        private static IEnumerable<int> CalculateFuelR(int mass)
        {
            while (true)
            {
                mass = CalculateFuel(mass);

                yield return mass;
            }
        }
    }
}
