using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2016._04
{
    public class Room
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Checksum { get; set; }

        public bool IsValid()
        {
            HashSet<char> topFive = Name.Where(x => x != '-').GroupBy(x => x).Select(x => (c: x.Key, count: x.Count())).OrderByDescending(x => x.count).ThenBy(x => x.c).Select(x => x.c).Take(5).ToHashSet();

            return topFive.SetEquals(Checksum.ToHashSet());
        }

        public string DecodeName()
        {
            char[] letters = Name.ToCharArray();

            for (int i = 0; i < letters.Length; ++i)
            {
                if (letters[i] == '-')
                    letters[i] = ' ';
                else
                    for (int k = 0; k < Id; ++k)
                        letters[i] = (char)(((letters[i] + 1) - 'a') % ('z' - 'a' + 1) + 'a');
            }

            return new string(letters);
        }
    }
}
