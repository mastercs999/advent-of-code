using Priority_Queue;
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
            Console.WriteLine(Reduce(rules, molecule));
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

        private static int Reduce(List<Rule> rules, string molecule)
        {
            // Dictionary with number of steps to molecule
            Dictionary<string, int> moleculeToSteps = new Dictionary<string, int>()
            {
                { molecule, 0 }
            };

            // Remember processed molecules
            HashSet<string> seen = new HashSet<string>();

            // Queue with molecules to process
            SimplePriorityQueue<string> toProcess = new SimplePriorityQueue<string>();
            toProcess.Enqueue(molecule, molecule.Length);

            // Randomizator, because otherwise we get stuck
            Random rng = new Random();

            while (toProcess.Any())
            {
                // Get another molecule to process
                string oldMolecule = toProcess.Dequeue();

                //Console.WriteLine(oldMolecule);
                if (oldMolecule == "e")
                    break;

                // Do all possible reductions
                foreach (Rule rule in rules.OrderBy(x => rng.Next()))
                    foreach (int index in oldMolecule.IndiciesOf(rule.To))
                    {
                        // Do reduction
                        string newMolecule = oldMolecule.Substring(0, index) + rule.From + oldMolecule.Substring(index + rule.To.Length);

                        // Calculate steps
                        int steps = moleculeToSteps[oldMolecule] + 1;

                        // Save progress
                        if (!moleculeToSteps.TryGetValue(newMolecule, out int oldSteps) || oldSteps > steps)
                            moleculeToSteps[newMolecule] = steps;

                        // Do more reductions
                        if (!seen.Contains(newMolecule))
                        {
                            toProcess.Enqueue(newMolecule, newMolecule.Length);
                            seen.Add(newMolecule);
                        }
                    }
            }

            return moleculeToSteps["e"];
        }
    }
}
