using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._22
{
    public class SpellFactory
    {
        private readonly int BossDamage;

        public SpellFactory(int bossDamage)
        {
            BossDamage = bossDamage;
        }

        public Spell CreateSpell(SpellType spellType, Character attacker, Character victim)
        {
            switch (spellType)
            {
                case SpellType.BossDamage:
                    return new Spell(spellType, 0, 1, attacker, victim, (t, a, v) => v.TakeDamage(BossDamage));
                case SpellType.MagicMissile:
                    return new Spell(spellType, 53, 1, attacker, victim, (t, a, v) => v.TakeDamage(4));
                case SpellType.Drain:
                    return new Spell(spellType, 73, 1, attacker, victim, (t, a, v) =>
                    {
                        v.TakeDamage(2);
                        a.HitPoints += 2;
                    });
                case SpellType.Shield:
                    return new Spell(spellType, 113, 6, attacker, victim, (t, a, v) => a.Armor += (t == 6 ? 7 : t == 1 ? -7 : 0));
                case SpellType.Poison:
                    return new Spell(spellType, 173, 6, attacker, victim, (t, a, v) => v.TakeDamage(3));
                case SpellType.Recharge:
                    return new Spell(spellType, 229, 5, attacker, victim, (t, a,v_) => a.Mana += 101);
            }

            throw new InvalidOperationException();
        }
    }
}
