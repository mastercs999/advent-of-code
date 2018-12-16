using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2018._16
{
    public static class Challenge16
    {
        private static readonly Dictionary<string, Action<uint[], uint, uint, uint>> Instructions = new Dictionary<string, Action<uint[], uint, uint, uint>>()
        {
            { "addr", (reg, a, b, c) => reg[c] = reg[a] + reg[b] },
            { "addi", (reg, a, b, c) => reg[c] = reg[a] + b },
            { "mulr", (reg, a, b, c) => reg[c] = reg[a] * reg[b] },
            { "muli", (reg, a, b, c) => reg[c] = reg[a] * b },
            { "banr", (reg, a, b, c) => reg[c] = reg[a] & reg[b] },
            { "bani", (reg, a, b, c) => reg[c] = reg[a] & b },
            { "borr", (reg, a, b, c) => reg[c] = reg[a] | reg[b] },
            { "bori", (reg, a, b, c) => reg[c] = reg[a] | b },
            { "setr", (reg, a, b, c) => reg[c] = reg[a] },
            { "seti", (reg, a, b, c) => reg[c] = a },
            { "gtir", (reg, a, b, c) => reg[c] = a > reg[b] ? 1 : 0u },
            { "gtri", (reg, a, b, c) => reg[c] = reg[a] > b ? 1 : 0u },
            { "gtrr", (reg, a, b, c) => reg[c] = reg[a] > reg[b] ? 1 : 0u },
            { "eqir", (reg, a, b, c) => reg[c] = a == reg[b] ? 1 : 0u },
            { "eqri", (reg, a, b, c) => reg[c] = reg[a] == b ? 1 : 0u },
            { "eqrr", (reg, a, b, c) => reg[c] = reg[a] == reg[b] ? 1 : 0u },
        };

        public static void Run()
        {
            // Load test cases
            List<TestCase> testCases = LoadTestCases(Path.Combine("16", "input")).ToList();

            // Task 1
            int manyCandidates = testCases.Where(x => Candidates(x).Count() >= 3).Count();

            Console.WriteLine(manyCandidates);

            // Task 2
            // Create translation from test cases
            Dictionary<uint, string> opcodeToName = CreateOpcodeTable(testCases);

            // Load program
            List<Instruction> program = LoadProgram(Path.Combine("16", "input")).ToList();

            // Execute all instructions of program
            uint[] registers = ExecuteProgram(program, opcodeToName);

            uint register0 = registers[0];

            Console.WriteLine(register0);
        }

        private static IEnumerable<TestCase> LoadTestCases(string path)
        {
            using (StreamReader sr = new StreamReader(path))
                while (true)
                {
                    // Read block
                    string before = sr.ReadLine();
                    string instruction = sr.ReadLine();
                    string after = sr.ReadLine();
                    sr.ReadLine();

                    // End of test cases
                    if (!before.StartsWith("Before"))
                        break;

                    // Parse numbers function
                    IEnumerable<uint> parseNumbers(string line) => Regex.Matches(line, @"(\d+)").Cast<Match>().Select(x => uint.Parse(x.Groups[1].Value));

                    // Split instruction
                    uint[] instructionParts = parseNumbers(instruction).ToArray();

                    // Create test case
                    yield return new TestCase()
                    {
                        Before = parseNumbers(before).ToArray(),
                        Instruction = new Instruction()
                        {
                            OpCode = instructionParts[0],
                            A = instructionParts[1],
                            B = instructionParts[2],
                            C = instructionParts[3],
                        },
                        After = parseNumbers(after).ToArray()
                    };
                };
        }
        private static IEnumerable<Instruction> LoadProgram(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int from = lines.ToList().FindLastIndex(x => x.StartsWith("After")) + 1;

            return lines.Skip(from).Where(x => !String.IsNullOrWhiteSpace(x)).Select(x =>
            {
                uint[] numbers = Regex.Matches(x, @"(\d+)").Cast<Match>().Select(y => uint.Parse(y.Groups[1].Value)).ToArray();

                return new Instruction()
                {
                    OpCode = numbers[0],
                    A = numbers[1],
                    B = numbers[2],
                    C = numbers[3],
                };
            });

        }

        private static IEnumerable<string> Candidates(TestCase testCase)
        {
            foreach (KeyValuePair<string, Action<uint[], uint, uint, uint>> instruction in Instructions)
            {
                uint[] registers = testCase.Before.ToArray();

                // Peform instruction
                instruction.Value(registers, testCase.Instruction.A, testCase.Instruction.B, testCase.Instruction.C);

                if (registers.SequenceEqual(testCase.After))
                    yield return instruction.Key;
            }
        }

        private static Dictionary<uint, string> CreateOpcodeTable(List<TestCase> testCases)
        {
            // All candidates
            Dictionary<uint, List<string>> opcodeToNames = testCases.SelectMany(x => Candidates(x).Select(y => (op: x.Instruction.OpCode, name: y))).ToLookup(x => x.op, x => x.name).ToDictionary(x => x.Key, x => x.Distinct().ToList());

            // Detect translations
            while (opcodeToNames.Any(x => x.Value.Count > 1))
            {
                foreach ((uint opCode, string name) in opcodeToNames.Where(x => x.Value.Count == 1).Select(x => (x.Key, x.Value.Single())))
                    foreach (List<string> candidates in opcodeToNames.Where(x => x.Key != opCode).Select(x => x.Value))
                        candidates.Remove(name);
            }

            return opcodeToNames.ToDictionary(x => x.Key, x => x.Value.Single());
        }
        private static uint[] ExecuteProgram(List<Instruction> program, Dictionary<uint, string> opcodeToName)
        {
            uint[] registers = new uint[4];

            // Execute every instruction
            foreach (Instruction instruction in program)
            {
                string name = opcodeToName[instruction.OpCode];

                Instructions[name](registers, instruction.A, instruction.B, instruction.C);
            }

            return registers;
        }
    }
}
