using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfCastInvisibilityWhenNearEnemyElfAndLowHealth : Heuristic
    {
        private int lowHealth;
        private int maxDistanceFromEnemyElves;

        public ElfCastInvisibilityWhenNearEnemyElfAndLowHealth(float weight, int lowHealth, int maxDistanceFromEnemyElves) : base(weight)
        {
            this.lowHealth = lowHealth;
            this.maxDistanceFromEnemyElves = maxDistanceFromEnemyElves;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach(VirtualInvisibility castInvisibility in virtualGame.futureInvisibilitySpells.Values)
            {
                Elf elf = (Elf)castInvisibility.realGameObject;

                if(elf.CurrentHealth <= lowHealth)
                {
                    List<Elf> enemyElves = Constants.GameCaching.GetEnemyElvesInArea(new Circle(elf.GetLocation(), maxDistanceFromEnemyElves));

                    if(enemyElves.Count > 0)
                    {
                        score++;
                    }
                }
            }

            return score;
        }
    }
}
