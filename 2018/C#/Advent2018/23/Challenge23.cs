using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2018._23
{
    public static class Challenge23
    {
        public static void Run()
        {
            // Load nanobots
            List<Nanobot> nanobots = LoadNanobots(Path.Combine("23", "input")).ToList();

            // Task 1
            Nanobot strongest = nanobots.OrderByDescending(x => x.Radius).First();
            int inRange = nanobots.Count(x => Distance(strongest.Point, x.Point) <= strongest.Radius);

            Console.WriteLine(inRange);

            // Task 2
            HashSet<Point> corners = ExtractCorners(nanobots).ToHashSet();

            // Find corners with max intersection counts
            Dictionary<Point, int> counts = corners.ToDictionary(x => x, x => IntersectionCount(nanobots, x));
            int maxIntersectionCount = corners.Select(x => IntersectionCount(nanobots, x)).Max();
            HashSet<Point> maxPoints = counts.Where(x => x.Value == maxIntersectionCount).Select(x => x.Key).ToHashSet();

            // Move after cube edges to find possibly better points
            HashSet<Point> candidates = new HashSet<Point>();
            foreach (Point maxPoint in maxPoints)
            {
                int dxShift = maxPoint.X > 0 ? -1 : 1;
                int dyShift = maxPoint.Y > 0 ? -1 : 1;
                int dzShift = maxPoint.Z > 0 ? -1 : 1;
                foreach ((int dx, int dy, int dz) in new (int, int, int)[] { (dxShift, dyShift, 0), (dxShift, 0, dzShift), (0, dyShift, dzShift) })
                {
                    Point local = new Point(maxPoint);
                    int localMax = maxIntersectionCount;

                    while (true)
                    {
                        // Make shift
                        Point temp = new Point(local);
                        temp.X += dx;
                        temp.Y += dy;
                        temp.Z += dz;

                        // Greedy search
                        int m = IntersectionCount(nanobots, temp);
                        if (m < localMax)
                            break;
                        if (m > localMax)
                            localMax = m;

                        local = temp;
                    }

                    // Add candidate
                    candidates.Add(local);
                }
            }

            // Find the point with the highest intersection count and closest to zero
            maxIntersectionCount = candidates.Select(x => IntersectionCount(nanobots, x)).Max();
            Point nearest = candidates.Where(x => IntersectionCount(nanobots, x) == maxIntersectionCount).OrderBy(x => Distance(x, new Point(0, 0, 0))).First();

            Console.WriteLine(Distance(nearest, new Point(0, 0, 0)));
        }

        private static IEnumerable<Nanobot> LoadNanobots(string path)
        {
            Regex regex = new Regex(@"pos=<(-?\d+),(-?\d+),(-?\d+)>, r=(\d+)");

            return File.ReadLines(path).Select(line =>
            {
                int[] values = regex.Match(line).Groups.Cast<Group>().Skip(1).Select(x => int.Parse(x.Value)).ToArray();

                return new Nanobot(new Point(values[0], values[1], values[2]), values[3]);
            });
        }

        private static int Distance(Point point1, Point point2) => Distance(point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z);
        private static int Distance(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2) + Math.Abs(z1 - z2);
        }

        private static int IntersectionCount(List<Nanobot> nanobots, Point point) => nanobots.Count(n => Distance(point, n.Point) <= n.Radius);

        private static IEnumerable<Point> ExtractCorners(IEnumerable<Nanobot> nanobots)
        {

            foreach (Nanobot range in nanobots)
                foreach (int dx in new int[] { -1, 0, 1 })
                    foreach (int dy in new int[] { -1, 0, 1 })
                        foreach (int dz in new int[] { -1, 0, 1 })
                        {
                            int x = range.Point.X + dx * range.Radius;
                            int y = range.Point.Y + dy * range.Radius;
                            int z = range.Point.Z + dz * range.Radius;

                            yield return new Point(x, y, z);
                        }
        }
    }
}
