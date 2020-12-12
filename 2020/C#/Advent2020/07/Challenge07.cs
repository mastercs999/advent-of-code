using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2020._07
{
    public static class Challenge07
    {
        public static void Run()
        {
            // Load bags
            Bag[] bags = LoadBags(Path.Combine("07", "input.txt"));

            // Task 1
            HashSet<string> inside = FindBag(bags, "shiny gold");
            Console.WriteLine(inside.Count);

            // Task 2
            int containing = FindContaining(bags.ToDictionary(x => x.Name), "shiny gold");
            Console.WriteLine(containing);
        }

        private static HashSet<string> FindBag(Bag[] bags, string name)
        {
            ILookup<string, string> insideBags = bags.SelectMany(x => x.BagsInside.Select(y => (y.Key, x.Name))).ToLookup(x => x.Key, x => x.Name);

            HashSet<string> inside = new HashSet<string>();

            Stack<string> toSearch = new Stack<string>(insideBags[name]);
            while (toSearch.TryPop(out string next))
            {
                inside.Add(next);

                foreach (string x in insideBags[next])
                    toSearch.Push(x);
            }

            return inside;
        }
        private static int FindContaining(Dictionary<string, Bag> nameToBag, string name)
        {
            return !nameToBag[name].BagsInside.Any() ? 0 : nameToBag[name].BagsInside.Sum(x => x.Value + x.Value * FindContaining(nameToBag, x.Key));
        }

        private static Bag[] LoadBags(string filePath)
        {
            return File
                .ReadLines(filePath)
                .Select(x => new Bag()
                {
                    Name = Regex.Match(x, @"(\w+\s+\w+)").Groups[1].Value,
                    BagsInside = Regex.Matches(x, @"(\d+)\s+(\w+\s+\w+)").ToDictionary(x => x.Groups[2].Value, x => int.Parse(x.Groups[1].Value))
                })
                .ToArray();
        }
    }
}
