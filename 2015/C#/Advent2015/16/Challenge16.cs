using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._16
{
    public static class Challenge16
    {
        public static void Run()
        {
            // Load information about aunts
            List<Aunt> aunts = LoadAunts(Path.Combine("16", "input")).ToList();

            // Task 1
            Aunt sample = new Aunt()
            {
                Children = 3,
                Cats = 7,
                Samoyeds = 2,
                Pomeranians = 3,
                Akitas = 0,
                Vizslas = 0,
                Goldfish = 5,
                Trees = 3,
                Cars = 2,
                Perfumes = 1
            };

            Aunt theAunt = FindAunt(aunts, sample);

            Console.WriteLine(theAunt.Id);

            // Task 2
            theAunt = FindAunt2(aunts, sample);

            Console.WriteLine(theAunt.Id);
        }

        public static IEnumerable<Aunt> LoadAunts(string path)
        {
            Dictionary<string, PropertyInfo> nameToProperty = typeof(Aunt).GetProperties().ToDictionary(x => x.Name.ToLower());

            return File.ReadLines(path).Select(x =>
            {
                Aunt aunt = new Aunt();

                // Get id Sue 36: cats: 3, children: 9, samoyeds: 3
                aunt.Id = int.Parse(x.Split(' ', ':')[1]);

                // Set properties
                string[] parts = x.Split(new char[] { ':', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 2; i < parts.Length; i += 2)
                    nameToProperty[parts[i]].SetValue(aunt, int.Parse(parts[i + 1]));

                return aunt;
            });
        }

        public static Aunt FindAunt(List<Aunt> aunts, Aunt sample)
        {
            Dictionary<string, int?> sampleProperties = sample.Properties;

            return aunts.Single(x =>
            {
                Dictionary<string, int?> properties = x.Properties;

                return properties.Where(y => y.Value.HasValue).All(y => y.Value == sampleProperties[y.Key]);
            });
        }

        public static Aunt FindAunt2(List<Aunt> aunts, Aunt sample)
        {
            Dictionary<string, int?> sampleProperties = sample.Properties;

            return aunts.Single(x =>
            {
                Dictionary<string, int?> properties = x.Properties;

                return properties.Where(y => y.Value.HasValue).All(y =>
                {
                    switch (y.Key)
                    {
                        case nameof(Aunt.Cats):
                        case nameof(Aunt.Trees):
                            return y.Value > sampleProperties[y.Key];
                        case nameof(Aunt.Pomeranians):
                        case nameof(Aunt.Goldfish):
                            return y.Value < sampleProperties[y.Key];
                        default:
                            return y.Value == sampleProperties[y.Key];
                    }
                });
            });
        }
    }
}
