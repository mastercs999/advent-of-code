using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._03
{
    public static class Challenge03
    {
        public static void Run()
        {
            string input = File.ReadAllText(Path.Combine("03", "input"));

            // Task 1
            int housesVisited1 = MakeVisits(input).Count;

            Console.WriteLine(housesVisited1);

            // Task 2
            int housesVisited2 = MakeVisits(input.Select((x, i) => (x: x, i: i)).Where(x => x.i % 2 == 0).Select(x => x.x)).Concat(
                                 MakeVisits(input.Select((x, i) => (x: x, i: i)).Where(x => x.i % 2 == 1).Select(x => x.x))).ToHashSet().Count;

            Console.WriteLine(housesVisited2);
        }

        public static HashSet<Point> MakeVisits(IEnumerable<char> path)
        {
            // We start at 0,0
            Point currentPosition = new Point(0, 0);

            // There are visited houses
            HashSet<Point> houses = new HashSet<Point>() { currentPosition };

            // Translation to coords
            Dictionary<char, Point> translation = new Dictionary<char, Point>()
            {
                { '^', new Point(0, 1) },
                { '>', new Point(1, 0) },
                { 'v', new Point(0, -1) },
                { '<', new Point(-1, 0) },
            };

            // Do the visits
            houses.UnionWith(path.Select(x => currentPosition = Sum(currentPosition, translation[x])));

            return houses;
        }

        public static Point Sum(Point p1, Point p2) => new Point(p1.X + p2.X, p1.Y + p2.Y);
    }
}
