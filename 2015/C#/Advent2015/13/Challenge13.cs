using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2015._13
{
    public static class Challenge13
    {
        public static void Run()
        {
            // Load rules from input
            List<Rule> rules = LoadRules(Path.Combine("13", "input")).ToList();

            // Prepare needed structures
            Dictionary<(string, string), int> neighboursToHappiness = rules.ToDictionary(x => (x.Name, x.Neighbour), x => x.Points);
            HashSet<string> names = rules.Select(x => x.Name).Concat(rules.Select(x => x.Neighbour)).ToHashSet();

            // Task 1
            IEnumerable<string> bestArrangement = FindBest(names, neighboursToHappiness);
            int best = CalculateHappiness(bestArrangement, neighboursToHappiness);

            Console.WriteLine(best);

            // Task 2
            bestArrangement = FindBestWithMe(bestArrangement, names, neighboursToHappiness);
            best = CalculateHappiness(bestArrangement, neighboursToHappiness);

            Console.WriteLine(best);
        }

        public static IEnumerable<Rule> LoadRules(string path)
        {
            //Alice would lose 2 happiness units by sitting next to Bob.
            Regex regex = new Regex(@"(\w+) would (lose|gain) (\d+) happiness units by sitting next to (\w+).");

            return File.ReadLines(path).Select(x =>
            {
                Match match = regex.Match(x);

                return new Rule()
                {
                    Name = match.Groups[1].Value,
                    Neighbour = match.Groups[4].Value,
                    Points = int.Parse(match.Groups[3].Value) * (match.Groups[2].Value == "lose" ? -1 : 1)
                };
            });
        }

        public static IEnumerable<string> FindBest(HashSet<string> names, Dictionary<(string, string), int> neighboursToHappiness)
        {
            // Because table is round, first name can be hardly set
            string[] first = new string[] { names.First() };
            HashSet<string> otherNames = names.Except(first).ToHashSet();

            // Find best arrangement
            return
                otherNames
                .Permutations()
                .Select(x => first.Concat(x))
                .OrderByDescending(x => CalculateHappiness(x, neighboursToHappiness))
                .First();
        }

        public static IEnumerable<string> FindBestWithMe(IEnumerable<string> arrangement, HashSet<string> names, Dictionary<(string, string), int> neighboursToHappiness)
        {
            string me = "me";

            // Add me to dictionary
            foreach (string name in names)
            {
                neighboursToHappiness.Add((me, name), 0);
                neighboursToHappiness.Add((name, me), 0);
            }

            // Try all possible arrangements
            return
                Enumerable.Range(0, names.Count - 1)
                .Select(x => arrangement.Take(x).Concat(new string[] { me }).Concat(arrangement.Skip(x)))
                .OrderByDescending(x => CalculateHappiness(x, neighboursToHappiness))
                .First();
        }

        public static int CalculateHappiness(IEnumerable<string> arrangement, Dictionary<(string, string), int> neighboursToHappiness)
        {
            return arrangement.Zip(arrangement.Skip(1).Concat(arrangement.Take(1)), (n1, n2) => neighboursToHappiness[(n1, n2)] + neighboursToHappiness[(n2, n1)]).Sum();
        }
    }
}
