using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._13
{
    public static class Challenge13
    {
        public static void Run()
        {
            // Load input
            (ulong timestamp, ulong?[] buses) = LoadBuses(Path.Combine("13", "input.txt"));

            // Task 1
            ulong result = buses
                .Where(x => x.HasValue)
                .Select(x => x.Value)
                .Select(x => (bus: x, time: CalculateNearest(timestamp, x)))
                .OrderBy(x => x.time)
                .Select(x => x.bus * (x.time - timestamp))
                .First();
            Console.WriteLine(result);

            // Task 2
            ulong result2 = FindSync(buses);
            Console.WriteLine(result2);
        }

        //private static ulong FindSync(ulong?[] buses)
        //{
        //    Dictionary<ulong, ulong> busToOffset = buses.Select((x, i) => (x, i)).Where(x => x.x.HasValue).ToDictionary(x => x.x.Value, x => (ulong)x.i);

        //    ulong maxValue = busToOffset.Max(x => x.Key);
        //    ulong testedTimestamp = CalculateNearest(100000000000000ul, maxValue) - busToOffset[maxValue];
        //    ulong xxx = ulong.MaxValue - 3 * maxValue;
        //    while (true)
        //    {
        //        testedTimestamp += maxValue;
        //        if (busToOffset.All(x => (testedTimestamp + x.Value) % x.Key == 0))
        //            return testedTimestamp;
        //    }
        //}
        private static ulong FindSync(ulong?[] buses)
        {
            Dictionary<ulong, ulong> busToOffset = buses.Select((x, i) => (x, i)).Where(x => x.x.HasValue).ToDictionary(x => x.x.Value, x => (ulong)x.i);

            ulong maxValue = busToOffset.Max(x => x.Key);
            ulong testedTimestamp = maxValue - busToOffset[maxValue];
            ulong xxx = ulong.MaxValue - 3 * maxValue;
            while (true)
            {
                testedTimestamp += maxValue;
                foreach (var x in busToOffset)
                    Console.Write((testedTimestamp) % x.Key + "\t");
                Console.WriteLine();
                if (busToOffset.All(x => (testedTimestamp + x.Value) % x.Key == 0))
                    return testedTimestamp;
            }
        }


        private static ulong CalculateNearest(ulong timestamp, ulong bus)
        {
            if (timestamp % bus == 0)
                return timestamp;

            return (timestamp / bus + 1) * bus;
        }

        private static (ulong, ulong?[]) LoadBuses(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            return (ulong.Parse(lines[0]), lines[1].Split(',').Select(x => x == "x" ? (ulong?)null : ulong.Parse(x)).ToArray());
        }
    }
}
