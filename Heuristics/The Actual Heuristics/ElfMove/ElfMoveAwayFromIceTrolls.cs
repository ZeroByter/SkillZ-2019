using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveAwayFromIceTrolls : Heuristic
    {
        private float avoidRadius;

        private float addMoreElfMaxStepsToAvoidRadiusWhenElfHasNotMuchHealth;
        private int whatIsNotMuchHealth;

        public ElfMoveAwayFromIceTrolls(float weight, float avoidRadius, float addMoreElfMaxStepsToAvoidRadiusWhenElfHasNotMuchHealth, int whatIsNotMuchHealth) : base(weight)
        {
            this.avoidRadius = avoidRadius;

            this.addMoreElfMaxStepsToAvoidRadiusWhenElfHasNotMuchHealth = Constants.Game.ElfMaxSpeed * addMoreElfMaxStepsToAvoidRadiusWhenElfHasNotMuchHealth;
            this.whatIsNotMuchHealth = whatIsNotMuchHealth;
        }

        private float GetElfScore(VirtualGame virtualGame, FutureLocation elfFutureLocation)
        {
            IceTroll[] enemyIceTrolls = Constants.GameCaching.GetEnemyIceTrolls();

            float score = 0;

            foreach (IceTroll enemyIceTroll in enemyIceTrolls)
            {
                if (enemyIceTroll.CurrentHealth == 1) continue;

                float distance = elfFutureLocation.GetFutureLocation().DistanceF(enemyIceTroll);
                float avoidRadiusFromTroll = avoidRadius;
                if (elfFutureLocation.GetElf().CurrentHealth < whatIsNotMuchHealth)
                {
                    avoidRadiusFromTroll += addMoreElfMaxStepsToAvoidRadiusWhenElfHasNotMuchHealth;
                }

                avoidRadiusFromTroll = Mathf.Min(avoidRadiusFromTroll, enemyIceTroll.CurrentHealth * enemyIceTroll.MaxSpeed + enemyIceTroll.AttackRange);

                if (distance < avoidRadiusFromTroll)
                {
                    score -= avoidRadiusFromTroll - distance;
                }
            }

            return score;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
            {
                score += GetElfScore(virtualGame, pair.Value);
            }

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}

/*Elves running away from/avoiding IceTrolls
 * Members:
 *  - avoidRadius - the radius to avoid around every IceTroll
 * 
 * Scoring:
 *  - For each elf, we check enemy icetrolls within avoidrange of elf and give minus score the closer we are to the icetroll
 */
