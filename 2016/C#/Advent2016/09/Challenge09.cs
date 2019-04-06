using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2016._09
{
    public static class Challenge09
    {
        public static void Run()
        {
            // Load instructions
            string encodedContent = File.ReadAllText(Path.Combine("09", "input"));

            // Clean all whitespace
            encodedContent = Regex.Replace(encodedContent, @"\s", "");

            // Task 1
            string decodedContent = Decode(encodedContent);
            int length = decodedContent.Length;

            Console.WriteLine(length);

            // Task 2
            ulong length2 = Decode2(encodedContent);

            Console.WriteLine(length2);
        }

        public static string Decode(string input)
        {
            // Find all markers
            Dictionary<int, Marker> indexToMarker = FindMarkers(input).ToDictionary(x => x.Index);

            IEnumerable<char> decode()
            {
                int position = 0;

                while (position < input.Length)
                {
                    // Get marker
                    indexToMarker.TryGetValue(position, out Marker marker);

                    if (marker == null)
                        yield return input[position++];
                    else
                    {
                        int begin = marker.Index + marker.MarkerLength;
                        int end = begin + marker.SampleLength;

                        for (int i = 0; i < marker.SampleCount; ++i)
                            for (int j = begin; j < end; ++j)
                                yield return input[j];

                        position = end;
                    }
                }
            }

            return new string(decode().ToArray());
        }

        public static ulong Decode2(string input)
        {
            // Find all markers
            Dictionary<int, Marker> indexToMarker = FindMarkers(input).ToDictionary(x => x.Index);

            ulong processPart(int begin, int end)
            {
                ulong count = 0;

                for (int i = begin; i < end; ++i)
                {
                    indexToMarker.TryGetValue(i, out Marker marker);

                    if (marker == null)
                        ++count;
                    else
                    {
                        count += markLength(marker);

                        i += marker.MarkerLength + marker.SampleLength - 1;
                    }
                }

                return count;
            }

            ulong markLength(Marker marker)
            {
                int begin = marker.Index + marker.MarkerLength;
                int end = begin + marker.SampleLength;

                ulong count = processPart(begin, end);

                return count * (ulong)marker.SampleCount;
            }

            return processPart(0, input.Length);
        }

        private static IEnumerable<Marker> FindMarkers(string input)
        {
            return Regex.Matches(input, @"\((\d+)x(\d+)\)").Cast<Match>().Select(x => new Marker()
            {
                Index = x.Index,
                SampleLength = int.Parse(x.Groups[1].Value),
                SampleCount = int.Parse(x.Groups[2].Value),
                MarkerLength = x.Length
            });
        }
    }
}
