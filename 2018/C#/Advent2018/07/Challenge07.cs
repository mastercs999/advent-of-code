using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2018._07
{
    public static class Challenge07
    {
        public static void Run()
        {
            // Load input
            List<(char, char)> relations = LoadRelations(Path.Combine("07", "input")).ToList();

            // Task 1 ABLCFNSXZPRHVEGUYKDIMQTWJO
            string path = new string(FindPath(relations.ToHashSet()).ToArray());

            Console.WriteLine(path);

            // Task 2
            int totalTime = TimeInParallel(relations.ToHashSet(), 5);

            Console.WriteLine(totalTime);
        }

        private static IEnumerable<char> FindPath(HashSet<(char, char)> relations)
        {
            HashSet<char> ends = relations.Where(x => !relations.Select(y => y.Item1).Contains(x.Item2)).Select(x => x.Item2).ToHashSet();

            while (relations.Any() || ends.Any())
            {
                // Find available step
                char step = GetNextStep(relations, ends, new HashSet<char>()).Value;

                // Return this step
                yield return step;

                // Remove useless transitions
                relations.RemoveWhere(x => x.Item1 == step);
                ends.RemoveWhere(x => x == step);
            }
        }

        private static int TimeInParallel(HashSet<(char, char)> relations, int elphesCount)
        {
            HashSet<char> ends = relations.Where(x => !relations.Select(y => y.Item1).Contains(x.Item2)).Select(x => x.Item2).ToHashSet();

            // Init elphes
            Elf[] elphes = Enumerable.Range(0, elphesCount).Select(x => new Elf()).ToArray();

            int totalTime = 0;
            while (relations.Any() || ends.Any() || elphes.Any(x => x.IsWorking))
            {
                // Continue in elphes work or end it
                foreach (Elf elf in elphes.Where(x => x.IsWorking))
                {
                    --elf.SecondsLeft;
                    if (elf.SecondsLeft == 0)
                    {
                        // Remove useless transitions
                        relations.RemoveWhere(x => x.Item1 == elf.Step.Value);
                        ends.RemoveWhere(x => x == elf.Step.Value);

                        elf.Step = null;
                    }
                }

                // Start work for elphes not working now
                foreach (Elf elf in elphes.Where(x => !x.IsWorking))
                {
                    // Find available step (if any) for the elf
                    elf.Step = GetNextStep(relations, ends, elphes.Where(x => x.IsWorking).Select(x => x.Step.Value).ToHashSet());
                    if (!elf.Step.HasValue)
                        continue;

                    // Assign time to work
                    elf.SecondsLeft = elf.Step.Value - 'A' + 61;
                }

                ++totalTime;
            }

            return totalTime - 1;
        }

        private static char? GetNextStep(HashSet<(char, char)> relations, HashSet<char> ends, HashSet<char> inProgress)
        {
            HashSet<char> firsts = relations.Select(x => x.Item1).Where(x => !inProgress.Contains(x)).ToHashSet();
            HashSet<char> seconds = relations.Select(x => x.Item2).Where(x => !inProgress.Contains(x)).ToHashSet();

            // Find start char
            return firsts.Where(x => !seconds.Contains(x)).Concat(ends.Where(x => !seconds.Contains(x))).OrderBy(x => x).Select(x => (char?)x).FirstOrDefault();
        }

        private static IEnumerable<(char, char)> LoadRelations(string path)
        {
            Regex regex = new Regex("Step ([A-Z]) must be finished before step ([A-Z]) can begin");

            return File.ReadAllLines(path).Select(x =>
            {
                Match match = regex.Match(x);

                return (match.Groups[1].Value[0], match.Groups[2].Value[0]);
            });
        }
    }
}
