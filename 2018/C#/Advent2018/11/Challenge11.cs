using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._11
{
    public static class Challenge11
    {
        public static void Run()
        {
            int input = 8444;

            // Task 1
            // Calculate whole grid
            int[,] grid = CalculateGrid(300, input);

            // Find the highest 3x3 grid
            List<Square> squares = CalculateSquares(grid).ToList();
            Square highest = squares.Where(x => x.Size == 3).OrderByDescending(x => x.PowerLevel).First();

            Console.WriteLine(highest.X + "," + highest.Y);

            // Task 2
            highest = squares.OrderByDescending(x => x.PowerLevel).First();

            Console.WriteLine(highest.X + "," + highest.Y + "," + highest.Size);
        }

        public static int CalculatePowerLevel(int x, int y, int serialNumber)
        {
            int rackId = x + 10;
            int powerLevel = rackId * y;
            powerLevel += serialNumber;
            powerLevel *= rackId;
            powerLevel = powerLevel / 100 % 10;
            powerLevel -= 5;

            return powerLevel;
        }

        public static int[,] CalculateGrid(int dimension, int serialNumber)
        {
            int[,] grid = new int[dimension, dimension];

            for (int x = 0; x < grid.GetLength(0); ++x)
                for (int y = 0; y < grid.GetLength(1); ++y)
                    grid[y, x] = CalculatePowerLevel(x + 1, y + 1, serialNumber);

            return grid;
        }

        public static IEnumerable<Square> CalculateSquares(int[,] grid)
        {
            for (int x = 0; x < grid.GetLength(0); ++x)
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    int maxSize = Math.Min(grid.GetLength(0) - x, grid.GetLength(1) - y);
                    int sum = 0;

                    for (int size = 1; size <= maxSize; ++size)
                    {
                        for (int i = x; i < x + size - 1; ++i)
                            sum += grid[y + size - 1, i];
                        for (int i = y; i < y + size - 1; ++i)
                            sum += grid[i, x + size - 1];
                        sum += grid[y + size - 1, x + size - 1];

                        yield return new Square()
                        {
                            X = x + 1,
                            Y = y + 1,
                            Size = size,
                            PowerLevel = sum
                        };
                    }
                }
        }
    }
}
