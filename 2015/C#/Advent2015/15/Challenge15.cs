using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2015._15
{
    public static class Challenge15
    {
        public static void Run()
        {
            // Load ingredients
            List<Ingredient> ingredients = LoadIngredient(Path.Combine("15", "input")).ToList();

            // Task 1
            // Init amounts per ingredient
            int[] amounts = new int[ingredients.Count];
            
            // Find out max score
            int max = int.MinValue;
            do
            {
                amounts.Next(100);

                int score =
                    ingredients.Score(amounts, x => x.Capacity) *
                    ingredients.Score(amounts, x => x.Durability) *
                    ingredients.Score(amounts, x => x.Flavor) *
                    ingredients.Score(amounts, x => x.Texture);

                max = Math.Max(max, score);
            } while (amounts[0] != 100);

            Console.WriteLine(max);

            // Task 2
            // Init amounts per ingredient
            amounts = new int[ingredients.Count];

            // Find out max score
            max = int.MinValue;
            do
            {
                amounts.Next(100);

                if (ingredients.Score(amounts, x => x.Calories) != 500)
                    continue;

                int score =
                    ingredients.Score(amounts, x => x.Capacity) *
                    ingredients.Score(amounts, x => x.Durability) *
                    ingredients.Score(amounts, x => x.Flavor) *
                    ingredients.Score(amounts, x => x.Texture);

                max = Math.Max(max, score);
            } while (amounts[0] != 100);

            Console.WriteLine(max);
        }

        public static IEnumerable<Ingredient> LoadIngredient(string path)
        {
            Regex regex = new Regex(@"(\w+): capacity (-?\d+), durability (-?\d+), flavor (-?\d+), texture (-?\d+), calories (-?\d+)");

            return File.ReadLines(path).Select(text =>
            {
                Match x = regex.Match(text);

                return new Ingredient(x.Groups[1].Value, int.Parse(x.Groups[2].Value), int.Parse(x.Groups[3].Value), int.Parse(x.Groups[4].Value), int.Parse(x.Groups[5].Value), int.Parse(x.Groups[6].Value));
            });
        }
    }
}
