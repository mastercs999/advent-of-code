using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._02
{
    public static class Challenge02
    {
        public static void Run()
        {
            // Load input file
            List<Password> passwords = LoadPasswords(Path.Combine("02", "input.txt")).ToList();

            // Task 1
            List<Password> validPasswords = passwords.Where(x => { int count = x.Text.Count(y => y == x.Character); return count >= x.MinNumber && count <= x.MaxNumber; }).ToList();
            Console.WriteLine(validPasswords.Count);

            // Task 2
            List<Password> validPasswords2 = passwords.Where(x => (x.Text[x.MinNumber - 1] == x.Character ? 1 : 0) + (x.Text[x.MaxNumber - 1] == x.Character ? 1 : 0) == 1).ToList();
            Console.WriteLine(validPasswords2.Count);
        }

        public static IEnumerable<Password> LoadPasswords(string inputFile)
        {
            return File.ReadLines(inputFile).Select(x =>
            {
                string[] parts = x.Split(new char[] { ' ', ':', '-' }, StringSplitOptions.RemoveEmptyEntries);

                return new Password()
                {
                    MinNumber = int.Parse(parts[0]),
                    MaxNumber = int.Parse(parts[1]),
                    Character = parts[2][0],
                    Text = parts[3]
                };
            });
        }
    }
}
