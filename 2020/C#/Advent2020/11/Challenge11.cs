using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._11
{
    public static class Challenge11
    {
        public static void Run()
        {
            // Load map
            char[][] map = LoadMap(Path.Combine("11", "input.txt"));

            // Task 1
            int occupied1 = Simulate(map, false, 4).Sum(x => x.Count(y => y == '#'));
            Console.WriteLine(occupied1);

            // Task 2
            int occupied2 = Simulate(map, true, 5).Sum(x => x.Count(y => y == '#'));
            Console.WriteLine(occupied2);
        }

        private static char[][] Simulate(char[][] current, bool infinite, int occupiedTreshold)
        {
            char[][] endMap = current;
            bool changed;
            do
            {
                (endMap, changed) = SimulateStep(endMap, infinite, occupiedTreshold);
            } while (changed);

            return endMap;
        }

        private static (char[][] newMap, bool changed) SimulateStep(char[][] current, bool infinite, int occupiedTreshold)
        {
            char[][] newOne = Copy(current);
            bool changed = false;
            
            for (int i = 0; i < current.Length; ++i)
                for (int j = 0; j < current[0].Length; ++j)
                {
                    int occupied = CountOccupied(current, j, i, infinite);
                    if (current[i][j] == 'L' && occupied == 0)
                    {
                        newOne[i][j] = '#';
                        changed = true;
                    }
                    else if (current[i][j] == '#' && occupied >= occupiedTreshold)
                    {
                        newOne[i][j] = 'L';
                        changed = true;
                    }
                }

            return (newOne, changed);
        }

        private static int CountOccupied(char[][] map, int posX, int posY, bool infinite)
        {
            (int dx, int dy)[] diffs = new (int dx, int dy)[]
            {
                (1, 0),
                (1, 1),
                (0, 1),
                (-1, 1),
                (-1, 0),
                (-1, -1),
                (0, -1),
                (1, -1)
            };
            static bool IsInside(char[][] map, int x, int y) => x >= 0 && x < map[0].Length && y >= 0 && y < map.Length;
            static char FindClosest(char[][] map, int x, int y, int dx, int dy, bool infinite)
            {
                do
                {
                    x += dx;
                    y += dy;

                    if (!IsInside(map, x, y))
                        return '.';
                    else if (map[y][x] != '.')
                        return map[y][x];
                } while (infinite);

                return map[y][x];
            }

            return diffs.Count(x => FindClosest(map, posX, posY, x.dx, x.dy, infinite) == '#');
        }

        private static char[][] Copy(char[][] map)
        {
            return map.Select(x => x.ToArray()).ToArray();
        }

        private static char[][] LoadMap(string filePath)
        {
            return File.ReadAllLines(filePath).Select(x => x.ToArray()).ToArray();
        }
    }
}
