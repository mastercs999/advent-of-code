using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._04
{
    public static class Challenge04
    {
        public static void Run()
        {
            // Load input
            List<Action> actions = Load(Path.Combine("04", "input")).OrderBy(x => x.DateTime).ToList();

            // Fill missing guard numbers
            FillGuards(actions);

            // Find sleeping intervals
            List<SleepInterval> sleeps = FindSleepIntervals(actions).ToList();

            // Get sleep minutes for every guard
            Dictionary<int, int[]> guardToSleepMinutes = sleeps.GroupBy(x => x.Guard).ToDictionary(x => x.Key, x => x.SelectMany(y => Minutes(y.From, y.To)).ToArray());

            // Task 1
            // Find guard with most sleeping minutes
            int guard = sleeps.GroupBy(x => x.Guard).OrderByDescending(x => x.Sum(y => (y.To - y.From).TotalMinutes)).First().Key;

            // Find the minutes when he sleep the most
            int minute = guardToSleepMinutes[guard].GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;

            Console.WriteLine(guard * minute);

            // Task 2
            // Find a guard who slept on any minute the most
            int minuteGuard = guardToSleepMinutes.OrderByDescending(x => x.Value.GroupBy(y => y).Max(y => y.Count())).First().Key;

            // Find the minute he slept the most
            int mostMinute = guardToSleepMinutes[minuteGuard].GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;

            Console.WriteLine(minuteGuard * mostMinute);
        }

        public static IEnumerable<Action> Load(string path)
        {
            return File.ReadLines(path).Select(x => new Action(x));
        }

        public static void FillGuards(List<Action> actions)
        {
            int guardNumber = actions.First().GuardNumber;
            if (guardNumber == default(int))
                throw new InvalidOperationException();

            foreach (Action action in actions)
            {
                if (action.ActionType == ActionType.BeginShift)
                    guardNumber = action.GuardNumber;

                action.GuardNumber = guardNumber;
            }
        }

        public static IEnumerable<SleepInterval> FindSleepIntervals(List<Action> actions)
        {
            for (int i = 0; i < actions.Count; ++i)
                if (actions[i].ActionType == ActionType.WakeUp)
                {
                    if (actions[i - 1].ActionType != ActionType.Asleep)
                        throw new InvalidOperationException();

                    yield return new SleepInterval()
                    {
                        Guard = actions[i].GuardNumber,
                        From = actions[i - 1].DateTime,
                        To = actions[i].DateTime
                    };
                }
        }

        public static IEnumerable<int> Minutes(DateTime from, DateTime to)
        {
            for (int i = from.Minute; i < to.Minute; ++i)
                yield return i;
        }
    }
}
