using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._22
{
    public class Spell
    {
        public SpellType SpellType { get; set; }
        public int Cost { get; set; }
        public int Timer { get; set; }

        public bool IsInstant => Timer == 1;

        private readonly Character Attacker;
        private readonly Character Victim;
        private readonly Action<int, Character, Character> SpellAction;

        public Spell(SpellType spellType, int cost, int timer, Character attacker, Character victim, Action<int, Character, Character> spellAction)
        {
            SpellType = spellType;
            Cost = cost;
            Timer = timer;
            Attacker = attacker;
            Victim = victim;
            SpellAction = spellAction;
        }

        public void Execute()
        {
            SpellAction(Timer, Attacker, Victim);

            --Timer;
        }

        public void Finish()
        {

        }
    }
}
