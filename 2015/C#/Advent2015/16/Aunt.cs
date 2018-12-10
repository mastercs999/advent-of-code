using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._16
{
    public class Aunt
    {
        public int Id { get; set; }

        public int? Children { get; set; }
        public int? Cats { get; set; }
        public int? Samoyeds { get; set; }
        public int? Pomeranians { get; set; }
        public int? Akitas { get; set; }
        public int? Vizslas { get; set; }
        public int? Goldfish { get; set; }
        public int? Trees { get; set; }
        public int? Cars { get; set; }
        public int? Perfumes { get; set; }

        public Dictionary<string, int?> Properties => typeof(Aunt).GetProperties().Where(x => x.Name != nameof(Aunt.Id) && x.CanWrite).ToDictionary(x => x.Name, x => (int?)x.GetValue(this));
    }
}
