using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._19
{
    public static class Challenge19
    {
        public static void Run()
        {
            // Load input
            List<Rule> rules = LoadRules(Path.Combine("19", "input")).ToList();
            string molecule = File.ReadLines(Path.Combine("19", "input")).Last();

            // Calculate all molecules
            int distinctCount = FindAllMolecules(molecule, rules).Distinct().Count();

            Console.WriteLine(distinctCount);
        }

        private static IEnumerable<Rule> LoadRules(string path)
        {
            return File.ReadLines(path).Where(x => x.Contains("=>")).Select(x =>
            {
                string[] parts = x.Split(' ');

                return new Rule()
                {
                    From = parts[0],
                    To = parts[2]
                };
            });
        }

        private static IEnumerable<string> FindAllMolecules(string molecule, List<Rule> rules)
        {
            foreach (Rule rule in rules)
            {
                int index = molecule.IndexOf(rule.From);
                while (index != -1)
                {
                    yield return molecule.Substring(0, index) + rule.To + molecule.Substring(index + rule.From.Length);

                    int from = index + 1;
                    index = molecule.Substring(from).IndexOf(rule.From);
                    if (index > -1)
                        index += from;
                }
            }
        }
    }
}
