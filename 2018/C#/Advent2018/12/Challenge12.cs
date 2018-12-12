using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._12
{
    public static class Challenge12
    {
        public static void Run()
        {
            // Load input
            LinkedList<Pot> pots = LoadPots(Path.Combine("12", "input"));
            Dictionary<(bool, bool, bool, bool, bool), bool> rules = LoadRules(Path.Combine("12", "input"));

            // Task 1
            // Do iterations
            for (int i = 0; i < 20; ++i)
                NextStep(pots, rules);

            // Calculate ons
            long ons = SumOns(pots);

            Console.WriteLine(ons);

            // Task 2
            // Find cycle
            Queue<long> diffs = new Queue<long>();
            long cyclesMade = 20;
            do
            {
                NextStep(pots, rules);
                ++cyclesMade;

                long ons2 = SumOns(pots);
                diffs.Enqueue(ons2 - ons);
                ons = ons2;

                if (diffs.Count > 5)
                    diffs.Dequeue();
            } while (diffs.Count < 5 || diffs.Distinct().Count() != 1);

            // Calculate result
            ons = SumOns(pots) + diffs.Distinct().Single() * (50000000000 - cyclesMade);

            Console.WriteLine(ons);
        }

        private static LinkedList<Pot> LoadPots(string path)
        {
            LinkedList<Pot> pots = new LinkedList<Pot>();

            foreach ((char c, int index) in File.ReadLines(path).First().Substring("initial state: ".Length).Select((x, i) => (x, i)))
                pots.AddLast(new Pot()
                {
                    Id = index,
                    HasPlant = HasPlant(c)
                });

            return pots;
        }
        private static Dictionary<(bool, bool, bool, bool, bool), bool> LoadRules(string path)
        {
            return File.ReadLines(path).Skip(2).ToDictionary(
                x =>
                {
                    bool[] ruleElements = x.Substring(0, 5).Select(y => HasPlant(y)).ToArray();

                    return (ruleElements[0], ruleElements[1], ruleElements[2], ruleElements[3], ruleElements[4]);
                },
                x => HasPlant(x[9]));
        }
        private static bool HasPlant(char c) => c == '#';

        private static void NextStep(LinkedList<Pot> pots, Dictionary<(bool, bool, bool, bool, bool), bool> rules)
        {
            // Add before or after
            if (pots.First.Value.HasPlant || pots.First.Next.Value.HasPlant)
                for (int i = 0; i < 2; ++i)
                    pots.AddFirst(new Pot()
                    {
                        Id = pots.First.Value.Id - 1,
                        HasPlant = false
                    });
            if (pots.Last.Value.HasPlant || pots.Last.Previous.Value.HasPlant)
                for (int i = 0; i < 2; ++i)
                    pots.AddLast(new Pot()
                    {
                        Id = pots.Last.Value.Id + 1,
                        HasPlant = false
                    });

            // Init queue with rule
            (bool, bool, bool, bool, bool) rule = (false, false, false, pots.First.Value.HasPlant, pots.First.Next.Value.HasPlant);

            // Apply rules
            LinkedListNode<Pot> current = pots.First;
            while (current != null)
            {
                // Append new value to queue
                LinkedListNode<Pot> next = current.Next?.Next;
                bool nextValue = next == null ? false : next.Value.HasPlant;
                rule = rule.Next(nextValue);

                // Find the rule
                bool newValue = rules[rule];

                current.Value.HasPlant = newValue;

                current = current.Next;
            }
        }

        private static long SumOns(LinkedList<Pot> pots) => pots.Where(x => x.HasPlant).Sum(x => (long)x.Id);
    }
}
