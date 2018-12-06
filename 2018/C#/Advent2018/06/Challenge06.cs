using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._06
{
    public static class Challenge06
    {
        public static void Run()
        {
            // Load input
            List<Point> points = LoadPoints(Path.Combine("06", "input")).ToList();

            // Task 1

            // Find closest neighbours
            Point?[,] area = FillArea(points, (x, y) => FindClosest(x, y, points));

            // Find points which have infinite areas
            HashSet<Point> infinitePoints = FindInfinites(area).ToHashSet();

            // Find max area occupied by one point
            int maxSize = area.Cast<Point?>().Where(x => x.HasValue).Select(x => x.Value).Where(x => !infinitePoints.Contains(x)).GroupBy(x => x).Select(x => x.Count()).Max();

            Console.WriteLine(maxSize);

            // Task 2

            // Calculate and sum distances to every point
            int[,] sums = FillArea(points, (x, y) => points.Sum(p => Distance(x, y, p.X, p.Y)));

            // Find number of points with distance less than 10000
            int maxSumSize = sums.Cast<int>().Where(x => x < 10000).Count();

            Console.WriteLine(maxSumSize);
        }

        private static IEnumerable<Point> LoadPoints(string path)
        {
            return File.ReadAllLines(path).Select(x =>
            {
                int[] parts = x.Split(',').Select(int.Parse).ToArray();

                return new Point(parts[0], parts[1]);
            });
        }

        private static T[,] FillArea<T>(List<Point> points, Func<int, int, T> func)
        {
            int xMax = points.Max(x => x.X);
            int yMax = points.Max(x => x.Y);

            T[,] area = new T[yMax, xMax];

            for (int x = 0; x < xMax; ++x)
                for (int y = 0; y < yMax; ++y)
                    area[y, x] = func(x, y);

            return area;
        }

        private static Point? FindClosest(int x, int y, List<Point> points)
        {
            List<(Point, int)> distances = points.Select(p => (point: p, distance: Distance(x, y, p.X, p.Y))).OrderBy(p => p.distance).Take(2).ToList();

            return distances[0].Item2 == distances[1].Item2 ? null : (Point?)distances[0].Item1;
        }

        private static IEnumerable<Point> FindInfinites(Point?[,] area)
        {
            foreach (int x in new int[] { 0, area.GetLength(1) - 1 })
                foreach (int y in new int[] { 0, area.GetLength(0) - 1 })
                    if (area[y, x].HasValue)
                        yield return area[y, x].Value;
        }

        private static int Distance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
    }
}
