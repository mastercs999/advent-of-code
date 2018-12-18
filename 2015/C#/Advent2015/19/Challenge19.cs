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

            // Task 1
            // Calculate all molecules
            int distinctCount = FindAllMolecules(molecule, rules).Distinct().Count();

            Console.WriteLine(distinctCount);

            // Task 2
            Console.WriteLine(Reduce2(rules, "CRnFYFYFArCaF"));
            Console.WriteLine(Reduce2(rules, molecule));
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

        private static int? Reduce(List<Rule> rules, string molecule)
        {
            // Dictionary with number of steps to molecule
            Dictionary<string, int> moleculeToSteps = new Dictionary<string, int>()
            {
                { molecule, 0 }
            };

            // Remember processed molecules
            HashSet<string> processed = new HashSet<string>();

            // Queue with molecules to process
            HashSet<string> toProcess = new HashSet<string>() { molecule };

            while (toProcess.Any())
            {
                // Get another molecule to process
                string oldMolecule = toProcess.First();
                toProcess.Remove(oldMolecule);

                // Add to processed
                processed.Add(oldMolecule);

                // Do all possible reductions
                foreach (Rule rule in rules)
                    foreach (int index in oldMolecule.IndiciesOf(rule.To))
                    {
                        // Do reduction
                        string newMolecule = oldMolecule.Substring(0, index) + rule.From + oldMolecule.Substring(index + rule.To.Length);

                        // Calculate steps
                        int steps = moleculeToSteps[oldMolecule] + 1;

                        // Save progress
                        if (!moleculeToSteps.ContainsKey(newMolecule))
                            moleculeToSteps.Add(newMolecule, steps);
                        else if (moleculeToSteps.TryGetValue(newMolecule, out int oldSteps) && oldSteps > steps)
                            moleculeToSteps[newMolecule] = steps;

                        // Do more reductions
                        if (!toProcess.Contains(newMolecule) && !processed.Contains(newMolecule))
                            toProcess.Add(newMolecule);
                    }
            }

            return moleculeToSteps["e"];
        }


        private static int? Reduce2(List<Rule> rules, string molecule)
        {
            // Sort list by lengths
            rules = rules.OrderByDescending(x => x.To.Length - x.From.Length).ToList();

            HashSet<string> examined = new HashSet<string>();

            int? _reduce(string oldMolecule)
            {
                examined.Add(oldMolecule);

                foreach (Rule rule in rules.Where(x => oldMolecule.Contains(x.To)))
                {
                    // Do reduction
                    int index = oldMolecule.IndexOf(rule.To);
                    string newMolecule = oldMolecule.Substring(0, index) + rule.From + oldMolecule.Substring(index + rule.To.Length);
                    if (examined.Contains(newMolecule))
                        continue;

                    // End
                    if (newMolecule == "e")
                        return 1;

                    // Do more reductions
                    int? steps = _reduce(newMolecule);
                    if (steps.HasValue)
                        return steps.Value + 1;
                }

                return null;
            }

            return _reduce(molecule);
        }
    }
}
