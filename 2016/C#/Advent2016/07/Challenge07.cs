using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2016._07
{
    public static class Challenge07
    {
        public static void Run()
        {
            // Load all IPs
            List<IP7> ips = LoadIPs(Path.Combine("07", "input")).ToList();

            // Task 1
            int tlsCount = ips.Count(x => x.SupportTls);

            Console.WriteLine(tlsCount);

            // Task 1
            int sslCount = ips.Count(x => x.SupportSsl);

            Console.WriteLine(sslCount);
        }

        private static IEnumerable<IP7> LoadIPs(string path)
        {
            return File.ReadLines(path).Select(line =>
            {
                MatchCollection outsideMatches = Regex.Matches(line, @"(\w+)(\[|$)");
                MatchCollection insideMatches = Regex.Matches(line, @"\[(\w+)\]");

                return new IP7()
                {
                    Outside = outsideMatches.Cast<Match>().Select(x => x.Groups[1].Value).ToArray(),
                    Inside = insideMatches.Cast<Match>().Select(x => x.Groups[1].Value).ToArray(),
                };
            });
        }
    }
}
