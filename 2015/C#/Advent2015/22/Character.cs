using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._22
{
    public class Character
    {
        public int Mana { get; set; }
        public int HitPoints { get; set; }
        public int Armor { get; set; }
        public HashSet<Spell> ActiveSpells { get; set; }

        public bool IsDead => HitPoints <= 0;
        public bool IsAlive => !IsDead;

        public Character(int mana, int hitPoints)
        {
            Mana = mana;
            HitPoints = hitPoints;
            ActiveSpells = new HashSet<Spell>();
        }

        public void TakeDamage(int damage)
        {
            HitPoints -= Math.Max(1, damage - Armor);
        }

        public bool CanCastSpell(Spell spell)
        {
            // We have to have mana to spend and the spall cannot be active yet
            return Mana >= spell.Cost && !ActiveSpells.Select(x => x.SpellType).Contains(spell.SpellType);
        }

        public void ExecuteSpells()
        {
            // Execute every single spell
            foreach (Spell spell in ActiveSpells)
                spell.Execute();

            // Remove inactive spells
            ActiveSpells.RemoveWhere(x => x.Timer <= 0);
        }
    }
}
