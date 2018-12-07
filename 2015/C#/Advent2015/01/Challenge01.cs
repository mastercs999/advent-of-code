using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._01
{
    public static class Challenge01
    {
        public static void Run()
        {
            string input = File.ReadAllText(Path.Combine("01", "input"));

            // Task 1
            int floor = input.Count(x => x == '(') - input.Count(x => x == ')');

            Console.WriteLine(floor);

            // Task 2
            int sum = 0;
            int position = Infinite(input.Select(x => x == '(' ? 1 : -1)).Select(x => sum += x).Select((x, i) => (x: x, index: i)).First(x => x.x == -1).index + 1;

            Console.WriteLine(position);
        }

        public static IEnumerable<int> Infinite(IEnumerable<int> source)
        {
            while (true)
                foreach (int num in source)
                    yield return num;
        }
    }
}
