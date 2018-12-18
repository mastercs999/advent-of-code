using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2018._17
{
    public static class Challenge17
    {
        public static void Run()
        {
            // Load ground
            TileType[,] ground = LoadGround(Path.Combine("17", "input"));

            // Find spring
            Point spring = FindSpring(ground);

            // Start water spread
            GoDown(ground, spring.X, spring.Y);

            // Task 1
            int waterTiles = ground.Cast<TileType>().Count(x => x == TileType.Water) - 1;

            Console.WriteLine(waterTiles);

            // Task 2
            int standingWater = 0;
            for (int y = 0; y < ground.GetLength(0); ++y)
                for (int x = 0; x < ground.GetLength(1); ++x)
                    if (ground[y, x] == TileType.Water && DetectWall(ground, x, y, -1) && DetectWall(ground, x, y, 1))
                        ++standingWater;

            Console.WriteLine(standingWater);
        }

        private static TileType[,] LoadGround(string path)
        {
            List<Point> clays = new List<Point>();

            // Load all clays from file
            Regex regex = new Regex(@"([x|y])=(\d+), ([x|y])=(\d+)\.\.(\d+)");
            foreach (string line in File.ReadLines(path))
            {
                Match match = regex.Match(line);

                bool xStart = match.Groups[1].Value == "x";
                int coord = int.Parse(match.Groups[2].Value);
                int from = int.Parse(match.Groups[4].Value);
                int to = int.Parse(match.Groups[5].Value);

                foreach (int x in Enumerable.Range(xStart ? coord : from, xStart ? 1 : to - from + 1))
                    foreach (int y in Enumerable.Range(!xStart ? coord : from, !xStart ? 1 : to - from + 1))
                        clays.Add(new Point(x, y));
            }

            // Determine ground size
            int xMin = clays.Min(x => x.X) - 1;
            int xMax = clays.Max(x => x.X) + 1;
            int yMin = clays.Min(x => x.Y) - 1;
            int yMax = clays.Max(x => x.Y);

            TileType[,] ground = new TileType[yMax - yMin + 1, xMax - xMin + 1];

            // Set loaded clays into ground
            foreach (Point clay in clays)
                ground[clay.Y - yMin, clay.X - xMin] = TileType.Clay;

            // Set spring
            ground[0, 500 - xMin] = TileType.Water;

            return ground;
        }
        private static Point FindSpring(TileType[,] ground)
        {
            for (int x = 0; x < ground.GetLength(1); ++x)
                if (ground[0, x] == TileType.Water)
                    return new Point(x, 0);

            throw new InvalidOperationException("Spring not found");
        }
        private static void PrintGround(string path, TileType[,] ground)
        {
            using (StreamWriter sw = new StreamWriter(path))
                for (int y = 0; y < ground.GetLength(0); ++y)
                {
                    for (int x = 0; x < ground.GetLength(1); ++x)
                    {
                        char c = ground[y, x] == TileType.Clay ? '#' : ground[y, x] == TileType.Sand ? '.' : '|';
                        if (ground[y, x] == TileType.Water && DetectWall(ground, x, y, -1) && DetectWall(ground, x, y, 1))
                            c = '~';

                        sw.Write(c);
                    }
                    sw.WriteLine();
                }
        }

        private static void GoDown(TileType[,] ground, int x, int y)
        {
            int targetY = y + 1;

            // We reached rock bottom
            if (targetY == ground.GetLength(0))
                return;

            // Usual flow
            if (ground[targetY, x] == TileType.Sand)
            {
                ground[targetY, x] = TileType.Water;

                // Continue in flow
                GoDown(ground, x, targetY);
                return;
            }
            else if (ground[targetY, x] == TileType.Water)   // There is water
            {
                if (DetectWall(ground, x, targetY, 1) && DetectWall(ground, x, targetY, -1))
                    GoUp(ground, x, targetY);
                return;
            }
            else    // There is clay, start filling up
                GoUp(ground, x, y);
        }
        private static void MoveHorizontal(TileType[,] ground, int x, int y, int dx)
        {
            int targetX = x + dx;

            // Dont fill anything already filled
            if (ground[y, targetX] == TileType.Water)
                return;

            // There is sand under target location - overflow
            if (ground[y + 1, targetX] == TileType.Sand)
            {
                ground[y, targetX] = TileType.Water;
                GoDown(ground, targetX, y);

            }
            else if (ground[y, targetX] != TileType.Clay)    // There is clay or another water under target location and wall not hit - continue
            {
                ground[y, targetX] = TileType.Water;

                MoveHorizontal(ground, targetX, y, dx);
            }
        }
        private static void GoUp(TileType[,] ground, int x, int y)
        {
            // Go left and right
            bool wallHitLeft, wallHitRight;
            int currentY = y;

            // Continue up if we hit the wall on both sides
            do
            {
                ground[currentY, x] = TileType.Water;

                MoveHorizontal(ground, x, currentY, -1);
                MoveHorizontal(ground, x, currentY, 1);

                wallHitLeft = DetectWall(ground, x, currentY, -1);
                wallHitRight = DetectWall(ground, x, currentY, 1);

                --currentY;
            } while (wallHitLeft && wallHitRight);
        }
        private static bool DetectWall(TileType[,] ground, int x, int y, int dx)
        {
            int targetX = x + dx;

            if (targetX < 0 || targetX >= ground.GetLength(1) || ground[y, targetX] == TileType.Sand)
                return false;
            else if (ground[y, targetX] == TileType.Clay)
                return true;
            else
                return DetectWall(ground, targetX, y, dx);
        }
    }
}
