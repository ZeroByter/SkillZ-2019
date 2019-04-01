using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfCastInvisibleWhenBeingChased : Heuristic
    {
        private float activateRadius;
        private int whatIsLowHealth;

        public ElfCastInvisibleWhenBeingChased(float weight, float activateRadius, int whatIsLowHealth) : base(weight)
        {
            this.activateRadius = activateRadius;
            this.whatIsLowHealth = whatIsLowHealth;
        }

        private float GetElfScore(VirtualGame virtualGame, Elf elf)
        {
            float score = 0;

            foreach(IceTroll enemyIceTroll in Constants.GameCaching.GetEnemyIceTrollsInArea(new Circle(elf, activateRadius)))
            {
                if (enemyIceTroll.IsHeadingTowards(elf, 0.8f)) //for more precision
                {
                    score++;
                }
            }
            foreach(Elf enemyElf in Constants.GameCaching.GetEnemyElvesInArea(new Circle(elf, activateRadius)))
            {
                if (enemyElf.IsHeadingTowards(elf, 0.8f)) //for more precision
                {
                    score++;
                }
            }

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

/* Heuristic added by Rom
 * I wanted a heuristic that would get us to go invisible when we are being chased and have low health.
 * members:
 *  - activateRadius = the radius at which we look for enemies near our elf
 *  - lowHealth = what is considered a low health? If our elf has more health than this then he isn't really in danger
 * Scoring:
 *  - If elf has low health, for each enemy icetroll and elf in activateRadius of elf we add score if he actually heading towards us for more precision
 */