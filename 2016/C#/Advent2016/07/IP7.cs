using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2016._07
{
    public class IP7
    {
        public string[] Outside { get; set; }
        public string[] Inside { get; set; }

        public bool SupportTls => Outside.Any(x => SearchAbba(x).Any()) && !Inside.Any(x => SearchAbba(x).Any());
        public bool SupportSsl => Outside.SelectMany(x => SearchAba(x).Select(y => $"{y[1]}{y[0]}{y[1]}")).Intersect(Inside.SelectMany(x => SearchAba(x))).Any();

        private IEnumerable<string> SearchAbba(string text)
        {
            for (int i = 3; i < text.Length; ++i)
                if (text[i - 3] == text[i] && text[i - 2] == text[i - 1] && text[i] != text[i - 1])
                    yield return text.Substring(i - 3, 4);
        }
        private IEnumerable<string> SearchAba(string text)
        {
            for (int i = 2; i < text.Length; ++i)
                if (text[i - 2] == text[i] && text[i] != text[i - 1])
                    yield return text.Substring(i - 2, 3);
        }
    }
}
