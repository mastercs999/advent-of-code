using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2015._06
{
    public static class Challenge06
    {
        public static void Run()
        {
            List<Command> commands = LoadCommands(Path.Combine("06", "input")).ToList();

            // Task 1
            int[,] field = new int[1000, 1000];
            foreach (Command cmd in commands)
                cmd.Apply(field, ActionFunc1);

            int isOn = field.Cast<int>().Count(x => x > 0);

            Console.WriteLine(isOn);

            // Task 2
            field = new int[1000, 1000];
            foreach (Command cmd in commands)
                cmd.Apply(field, ActionFunc2);

            isOn = field.Cast<int>().Sum();

            Console.WriteLine(isOn);
        }

        public static IEnumerable<Command> LoadCommands(string path)
        {
            Regex regex = new Regex(@"(\d+),(\d+) through (\d+),(\d+)");

            return File.ReadLines(path).Select(x =>
            {
                Match match = regex.Match(x);

                return new Command()
                {
                    CommandType = x.Contains("toggle") ? CommandType.Toggle : x.Contains("off") ? CommandType.TurnOff : CommandType.TurnOn,
                    TopLeft = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
                    BottomRight = new Point(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))
                };
            });
        }

        public static int ActionFunc1(CommandType commandType, int initial)
        {
            switch (commandType)
            {
                case CommandType.TurnOn: return 1;
                case CommandType.TurnOff: return 0;
                case CommandType.Toggle: return initial == 0 ? 1 : 0;
            }

            throw new InvalidOperationException();
        }

        public static int ActionFunc2(CommandType commandType, int initial)
        {
            switch (commandType)
            {
                case CommandType.TurnOn: return initial + 1;
                case CommandType.TurnOff: return Math.Max(0, initial - 1);
                case CommandType.Toggle: return initial + 2;
            }

            throw new InvalidOperationException();
        }
    }
}
