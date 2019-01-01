using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._22
{
    public class CharacterFactory
    {
        private readonly int BossHitPoints;
        private readonly int PlayerHitPoints;
        private readonly int PlayerMana;

        public CharacterFactory(int bossHitPoints, int playerHitPoints, int playerMana)
        {
            BossHitPoints = bossHitPoints;
            PlayerHitPoints = playerHitPoints;
            PlayerMana = playerMana;
        }

        public Character CreateBoss() => new Character(10, BossHitPoints);
        public Character CreatePlayer() => new Character(PlayerMana, PlayerHitPoints);
    }
}
