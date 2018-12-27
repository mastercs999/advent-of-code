using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._25
{
    public static class Challenge25
    {
        public static void Run()
        {
            // Load points
            List<Point> points = LoadPoints(Path.Combine("25", "input")).ToList();

            // Task 1
            int constellations = ReduceConstellations(points).Count();

            Console.WriteLine(constellations);
        }

        private static IEnumerable<Point> LoadPoints(string path)
        {
            return File.ReadLines(path).Select(x => new Point(x.Split(',').Select(int.Parse).ToArray()));
        }

        private static IEnumerable<Constellation> ReduceConstellations(List<Point> points)
        {
            // At the beginning, every points is a constellation
            Queue<Constellation> queue = new Queue<Constellation>();
            foreach (Point p in points)
                queue.Enqueue(new Constellation()
                {
                    Points = new List<Point>() { p }
                });

            // Do search
            while (queue.Any())
            {
                // Get the first
                Constellation constellation = queue.Dequeue();

                // Find if belongs anywhere
                Constellation target = queue.FirstOrDefault(x => x.CanJoin(constellation));
                if (target != null)
                    target.Join(constellation);
                else
                    yield return constellation;
            }
        }
    }
}
