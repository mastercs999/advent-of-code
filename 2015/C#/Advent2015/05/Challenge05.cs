using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._05
{
    public static class Challenge05
    {
        private static readonly char[] vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };
        private static readonly string[] forbidden = new string[] { "ab", "cd", "pq", "xy" };

        public static void Run()
        {
            string[] words = File.ReadAllLines(Path.Combine("05", "input"));

            // Task 1
            int niceWords1 = words.Where(x => IsNice1(x)).Count();

            Console.WriteLine(niceWords1);

            // Task 1
            int niceWords2 = words.Where(x => IsNice2(x)).Count();

            Console.WriteLine(niceWords2);
        }

        public static bool IsNice1(string word)
        {
            return
                word.Count(c => vowels.Contains(c)) >= 3 &&
                word.Zip(word.Skip(1), (c1, c2) => c1 == c2).Count(x => x) >= 1 &&
                forbidden.All(x => !word.Contains(x));
        }

        public static bool IsNice2(string word)
        {
            return
                word.Zip(word.Skip(1), (c1, c2) => c1 + "" + c2).Any(x => word.LastIndexOf(x) - word.IndexOf(x) > 1) &&
                word.Skip(2).Select((x, i) => (c: x, index: i)).Any(x => word[x.index] == x.c);
        }
    }
}
