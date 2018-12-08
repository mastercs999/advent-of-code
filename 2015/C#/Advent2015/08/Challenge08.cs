using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2015._08
{
    public static class Challenge08
    {
        public static void Run()
        {
            // Load string
            string[] texts = File.ReadAllLines(Path.Combine("08", "input"));

            // Task 1
            // Unescape the text
            string[] clean = texts.Select(Clean).ToArray();

            // Calculate diff in number of characters
            int diff = texts.Sum(x => x.Length) - clean.Sum(x => x.Length);

            Console.WriteLine(diff);

            // Task 2
            // Do the escapes
            string[] dirty = texts.Select(Dirty).ToArray();

            // Calculate diff in number of characters
            diff = dirty.Sum(x => x.Length) - texts.Sum(x => x.Length);

            Console.WriteLine(diff);
        }

        public static string Clean(string text)
        {
            StringBuilder sb = new StringBuilder(text);

            // Remove first and last character
            sb.Remove(0, 1);
            sb.Remove(sb.Length - 1, 1);

            // Escape sequence \\
            sb.Replace(@"\\", @"\");
            
            // Escape sequence \"
            sb.Replace("\\\"", "\"");

            // Control sequence \xNN
            Regex regex = new Regex("\\\\x([a-fA-F0-9][a-fA-F0-9])");
            string result = regex.Replace(sb.ToString(), x => ((char)Convert.ToInt32(x.Groups[1].Value, 16)).ToString());

            return result;
        }

        public static string Dirty(string text)
        {
            StringBuilder sb = new StringBuilder(text);

            // Escape \
            sb.Replace("\\", "\\\\");

            // Escape "
            sb.Replace("\"", "\\\"");

            // Add " at the beginning and in the end
            sb.Insert(0, '"');
            sb.Append('"');

            return sb.ToString();
        }
    }
}
