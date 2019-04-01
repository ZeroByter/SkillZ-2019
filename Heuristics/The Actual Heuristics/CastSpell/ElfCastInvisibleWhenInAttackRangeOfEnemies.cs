using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfCastInvisibleWhenInAttackRangeOfEnemies : Heuristic
    {
        public ElfCastInvisibleWhenInAttackRangeOfEnemies(float weight) : base(weight)
        {
        }

        private float GetElfScore(VirtualGame virtualGame, Elf elf)
        {
            float score = 0;

            score += Constants.GameCaching.GetEnemyIceTrollsInArea(new Circle(elf, elf.AttackRange + Constants.Game.IceTrollAttackRange)).Count;
            score += Constants.GameCaching.GetEnemyElvesInArea(new Circle(elf, elf.AttackRange * 2)).Count;

            return score;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (var futureInvisiblePair in virtualGame.futureInvisibilitySpells)
            {
                score += GetElfScore(virtualGame, (Elf)futureInvisiblePair.Value.realGameObject);
            }

            return score;
        }
    }
}