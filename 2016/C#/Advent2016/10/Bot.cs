using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2016._10
{
    public class Bot
    {
        public int Id { get; set; }

        public List<int?> Chips { get; set; }
        
        public int LowerChipIndex => Chips.IndexOf(Chips.Min());
        public int HigherChipIndex => Chips.LastIndexOf(Chips.Max());

        public bool HasBothChips => Chips.All(x => x.HasValue);
        public bool HasChip => Chips.Any(x => x.HasValue);
        public bool CanAccept => !HasBothChips;

        public Action Action { get; set; }

        public Bot(int id)
        {
            Id = id;
            Chips = new List<int?>() { null, null };
        }

        public void Receive(int chip)
        {
            int index = Chips.FindIndex(x => !x.HasValue);

            Chips[index] = chip;
        }

        public int Withdraw(int index)
        {
            int chip = Chips[index].Value;
            Chips[index] = null;

            return chip;
        }
    }
}
