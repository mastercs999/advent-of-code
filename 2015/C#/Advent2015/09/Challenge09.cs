using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2015._09
{
    public static class Challenge09
    {
        public static void Run()
        {
            // Load routes
            List<Route> routes = LoadRoutes(Path.Combine("09", "input")).ToList();

            // Extract all cities and distances
            HashSet<string> cities = routes.Select(x => x.City1).Concat(routes.Select(x => x.City2)).ToHashSet();
            Dictionary<(string, string), int> routeToDistance = routes.Select(x => (x.City1, x.City2, x.Distance)).Concat(routes.Select(x => (x.City2, x.City1, x.Distance))).ToDictionary(x => (x.Item1, x.Item2), x => x.Distance);

            // Find all distance paths
            List<int> distances = cities
                .Permutations()
                .Select(x => x.Zip(x.Skip(1), (t1, t2) => (t1, t2)))
                .Where(x => x.All(y => routeToDistance.ContainsKey(y)))
                .Select(x => x.Sum(y => routeToDistance[y]))
                .ToList();

            // Task 1
            // Find the shortes path
            int shortestLength = distances.Min();

            Console.WriteLine(shortestLength);

            // Task 2
            // Find the longest path
            int longestLength = distances.Max();

            Console.WriteLine(longestLength);
        }

        public static IEnumerable<Route> LoadRoutes(string path)
        {
            Regex regex = new Regex(@"^(\w+) to (\w+) = (\d+)$");

            return File.ReadLines(path).Select(x =>
            {
                Match match = regex.Match(x);

                return new Route()
                {
                    City1 = match.Groups[1].Value,
                    City2 = match.Groups[2].Value,
                    Distance = int.Parse(match.Groups[3].Value),
                };
            });
        }
    }
}
