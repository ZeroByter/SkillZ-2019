using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveToAttackEnemyManaFountains : Heuristic
    {
        public ElfMoveToAttackEnemyManaFountains(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            ManaFountain[] enemyManaFountains = Constants.GameCaching.GetEnemyManaFountains();
            var myElvesLocations = virtualGame.GetFutureLocations();

            foreach (ManaFountain enemyManaFountain in enemyManaFountains)
            {
                foreach (FutureLocation elfLocation in myElvesLocations.Values)
                {
                    score -= Mathf.Pow(elfLocation.GetFutureLocation().Distance(enemyManaFountain), 0.7f) / Mathf.Pow(Constants.Game.ElfMaxSpeed, 0.7f);
                }
            }

            return score;
        }
    }
}