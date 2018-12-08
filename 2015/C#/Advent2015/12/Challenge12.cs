using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2015._12
{
    public static class Challenge12
    {
        public static void Run()
        {
            // Load json
            string json = File.ReadAllText(Path.Combine("12", "input"));

            // Task 1
            // Extract numbers
            int sum = ExtractNumbers(json).Sum();

            Console.WriteLine(sum);

            // Task 2
            // Extract not red numbers
            sum = ExtractNotRedNumbers(JToken.Parse(json)).Sum();

            Console.WriteLine(sum);
        }

        public static IEnumerable<int> ExtractNumbers(string text)
        {
            MatchCollection matches = Regex.Matches(text, "(?<![\"a-zA-Z])(-?\\d+)(?![\"a-zA-Z])");

            return matches.Cast<Match>().Select(x => int.Parse(x.Groups[1].Value));
        }

        public static IEnumerable<int> ExtractNotRedNumbers(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Array: return token.SelectMany(x => ExtractNotRedNumbers(x));
                case JTokenType.Object:
                    if (token.Where(x => x.Type == JTokenType.Property).Cast<JProperty>().Where(x => x.Value.Type == JTokenType.String && x.Value.Value<string>() == "red").Any())
                        return Enumerable.Empty<int>();
                    else
                    return token.SelectMany(x => ExtractNotRedNumbers(x));
                case JTokenType.Property: return ExtractNotRedNumbers((token as JProperty).Value);
                case JTokenType.Integer: return new int[] { token.Value<int>() };
                case JTokenType.String: return Enumerable.Empty<int>();
                default: throw new InvalidOperationException();
            }
        }
    }
}
