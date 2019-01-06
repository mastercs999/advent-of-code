using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2016._06
{
    public static class Challenge06
    {
        public static void Run()
        {
            // Load words
            List<string> words = File.ReadAllLines(Path.Combine("06", "input")).ToList();

            // Task 1
            string message = new string(Enumerable.Range(0, words.First().Length).Select(x => words.Select(y => y[x]).GroupBy(y => y).OrderByDescending(y => y.Count()).First().Key).ToArray());

            Console.WriteLine(message);

            // Task 2
            message = new string(Enumerable.Range(0, words.First().Length).Select(x => words.Select(y => y[x]).GroupBy(y => y).OrderBy(y => y.Count()).First().Key).ToArray());

            Console.WriteLine(message);
        }
    }
}
