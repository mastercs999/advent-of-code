using Advent2015._07.Gates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2015._07
{
    public static class Challenge07
    {
        public static void Run()
        {
            // Load gates
            string path = Path.Combine("07", "input");
            List<Gate> gates = LoadGates(path).ToList();

            // Task 1
            ComputeSignals(gates);

            // Get the signal we are intersted in
            ushort signalA = gates.First(x => x.OutputSignal.Id == "a").OutputSignal.Value.Value;

            Console.WriteLine(signalA);

            // Task 2

            // Load gates again
            gates = LoadGates(path).ToList();

            // Set signal B to value of signal A
            foreach (Signal signal in gates.SelectMany(x => x.InputSignals).Where(x => x.Id == "b"))
                signal.Value = signalA;

            // Remove all gates which are producing signal B
            gates.RemoveAll(x => x.OutputSignal.Id == "b");

            // Compute result again
            ComputeSignals(gates);

            // Get the signal we are intersted in
            signalA = gates.First(x => x.OutputSignal.Id == "a").OutputSignal.Value.Value;

            Console.WriteLine(signalA);
        }

        public static IEnumerable<Gate> LoadGates(string path)
        {
            Dictionary<Regex, Func<Match, Gate>> parseOptions = new Dictionary<Regex, Func<Match, Gate>>()
            {
                {  new Regex(@"^(\w+|\d+) -> (\w+)$"), match => new Move(new Signal(match.Groups[1].Value), new Signal(match.Groups[2].Value)) },
                {  new Regex(@"^(\w+|\d+) AND (\w+|\d+) -> (\w+)$"), match => new And(new Signal(match.Groups[1].Value), new Signal(match.Groups[2].Value), new Signal(match.Groups[3].Value)) },
                {  new Regex(@"^(\w+|\d+) OR (\w+|\d+) -> (\w+)$"), match => new Or(new Signal(match.Groups[1].Value), new Signal(match.Groups[2].Value), new Signal(match.Groups[3].Value)) },
                {  new Regex(@"^NOT (\w+|\d+) -> (\w+)"), match => new Not(new Signal(match.Groups[1].Value), new Signal(match.Groups[2].Value)) },
                {  new Regex(@"^(\w+|\d+) LSHIFT (\w+|\d+) -> (\w+)$"), match => new LeftShift(new Signal(match.Groups[1].Value), ushort.Parse(match.Groups[2].Value), new Signal(match.Groups[3].Value)) },
                {  new Regex(@"^(\w+|\d+) RSHIFT (\w+|\d+) -> (\w+)$"), match => new RightShift(new Signal(match.Groups[1].Value), ushort.Parse(match.Groups[2].Value), new Signal(match.Groups[3].Value)) },
            };

            return File.ReadLines(path).Select(x => 
            {
                KeyValuePair<Regex, Func<Match, Gate>> kvp = parseOptions.Single(y => y.Key.Match(x).Success);

                return kvp.Value(kvp.Key.Match(x));
            });
        }

        public static void ComputeSignals(List<Gate> gates)
        {
            // Gather all signals
            List<Signal> allSignals = gates.SelectMany(x => x.InputSignals).Concat(gates.Select(x => x.OutputSignal)).ToList();
            ILookup<string, Signal> idToSignals = allSignals.Where(x => x.Id != null).ToLookup(x => x.Id);

            // Compute all signals
            while (allSignals.Any(x => !x.Value.HasValue))
            {
                foreach (Gate gate in gates.Where(x => x.InputSignals.All(y => y.Value.HasValue)))
                {
                    gate.Calculate();

                    // Sync all signals with the same name
                    foreach (Signal signal in idToSignals[gate.OutputSignal.Id])
                        signal.Value = gate.OutputSignal.Value;
                }
            }
        }
    }
}
