using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._11
{
    public static class Challenge11
    {
        private static readonly char[] Forbidden = new char[] { 'i', 'o', 'l' };

        public static void Run()
        {
            // Input password
            string inputText = "vzbxkghb";
            char[] password = inputText.ToArray();

            // Task 1
            // Get next password
            string nextPassword = new string(GenerateNext(password));

            Console.WriteLine(nextPassword);

            // Task 2
            // Again get next password
            nextPassword = new string(GenerateNext(nextPassword.ToArray()));

            Console.WriteLine(nextPassword);
        }

        public static char[] GenerateNext(char[] input)
        {
            // Copy to new array
            char[] password = input.ToArray();

            // Do increment until we find a new valid password
            do
            {
                Increment(password);
            } while (!Pass(password));

            return password;
        }

        public static bool Pass(char[] password)
        {
            string passwordStr = new string(password);

            return
                password.Zip(password.Skip(1), password.Skip(2), (x, y, z) => (x: x, y: y, z: z)).Any(x => x.x + 1 == x.y && x.y + 1 == x.z) &&
                !password.Intersect(Forbidden).Any() &&
                password.Zip(password.Skip(1), (c1, c2) => (c1: c1, c2: c2)).Where(x => x.c1 == x.c2).Distinct().Count() >= 2;
        }

        public static void Increment(char[] password)
        {
            int index = password.Length - 1;
            do
            {
                password[index] = password[index] == 'z' ? 'a' : (char)(password[index] + 1);
            } while (password[index--] == 'a');
        }
    }
}
