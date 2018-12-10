using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._15
{
    public static class Extensions
    {
        public static void Next(this int[] amounts, int cap)
        {
            do
            {
                int index = amounts.Length - 1;
                bool overflow = false;

                do
                {
                    overflow = false;
                    ++amounts[index];

                    if (amounts[index] > cap)
                    {
                        overflow = true;
                        amounts[index] = 0;

                        --index;
                    }
                } while (overflow);
            }
            while (amounts.Sum() != cap);
        }

        public static int Score(this IEnumerable<Ingredient> ingredients, IEnumerable<int> amounts, Func<Ingredient, int> getIngredientValueFunc)
        {
            return Math.Max(0, amounts.Zip(ingredients, (a, i) => a * getIngredientValueFunc(i)).Sum());
        }
    }
}