using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._04
{
    public static class Challenge04
    {
        public static void Run()
        {
            // Load passports
            Dictionary<string, string>[] passports = LoadPassports(Path.Combine("04", "input.txt"));

            // Task 1
            int validPassports = passports.Count(x => x.Count >= (x.ContainsKey("cid") ? 8 : 7));
            Console.WriteLine(validPassports);

            // Task 2
            int validPassports2 = passports.Count(x => 
                x.Count >= (x.ContainsKey("cid") ? 8 : 7) &&
                x.All(y =>
                {
                    return y.Key switch
                    {
                        "byr" => int.TryParse(y.Value, out int r) && r is >= 1920 and <= 2002,
                        "iyr" => int.TryParse(y.Value, out int r) && r is >= 2010 and <= 2020,
                        "eyr" => int.TryParse(y.Value, out int r) && r is >= 2020 and <= 2030,
                        "hgt" => y.Value.Length > 2 && int.TryParse(y.Value[0..^2], out int r) && (y.Value.EndsWith("cm") && r is >= 150 and <= 193 || y.Value.EndsWith("in") && r is >= 59 and <= 76),
                        "hcl" => y.Value.StartsWith('#') && y.Value.Length == 7 && y.Value.Skip(1).All(z => z is >= '0' and <= '9' or >= 'a' and <= 'f'),
                        "ecl" => y.Value is "amb" or "blu" or "brn" or "gry" or "grn" or "hzl" or "oth",
                        "pid" => y.Value.Length == 9 && ulong.TryParse(y.Value, out _),
                        _ => true 
                    };
                })
            );
            Console.WriteLine(validPassports2);
        }

        public static Dictionary<string, string>[] LoadPassports(string filePath)
        {
            return File
                .ReadAllText(filePath)
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x
                    .Split((char[])null, StringSplitOptions.RemoveEmptyEntries)
                    .ToDictionary(y => y.Split(':')[0], y => y.Split(':')[1]))
                .ToArray();
        }
    }
}
