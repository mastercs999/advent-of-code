using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2015._14
{
    public static class Challenge14
    {
        public static void Run()
        {
            // Load reindeers
            List<Reindeer> reindeers = LoadReindeers(Path.Combine("14", "input")).ToList();

            // Task 1
            int maxDistance = reindeers.Max(x => x.Distances().Take(2503).Sum());

            Console.WriteLine(maxDistance);

            // Task 2
            int points = FindMaxPoints(reindeers, 2503);

            Console.WriteLine(points);
        }

        public static IEnumerable<Reindeer> LoadReindeers(string path)
        {
            // Dancer can fly 27 km/s for 5 seconds, but then must rest for 132 seconds.
            Regex regex = new Regex(@"(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds.");

            return File.ReadLines(path).Select(x =>
            {
                Match match = regex.Match(x);

                return new Reindeer()
                {
                    Name = match.Groups[1].Value,
                    Speed = int.Parse(match.Groups[2].Value),
                    FlyDuration = int.Parse(match.Groups[3].Value),
                    RestDuration = int.Parse(match.Groups[4].Value),
                };
            });
        }

        public static int FindMaxPoints(List<Reindeer> reindeers, int seconds)
        {
            // Find distances in every seconds
            Dictionary<Reindeer, int[]> reindeerToDistances = reindeers.ToDictionary(x => x, x => x.Distances().Take(seconds).CumulativeSum().ToArray());

            // Give points
            Dictionary<Reindeer, int> reindeerToPoints = reindeers.ToDictionary(x => x, _ => 0);
            for (int i = 0; i < seconds; ++i)
            {
                int max = reindeerToDistances.Max(x => x.Value[i]);

                foreach (Reindeer reindeer in reindeerToDistances.Where(x => x.Value[i] == max).Select(x => x.Key))
                    ++reindeerToPoints[reindeer];
            }

            return reindeerToPoints.Max(x => x.Value);
        }
    }
}
