using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveToEnemyPortals: Heuristic
    {
        public ElfMoveToEnemyPortals(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            Portal[] enemyPortals = Constants.GameCaching.GetEnemyPortals();
            Dictionary<int, FutureLocation> myElvesLocations = virtualGame.GetFutureLocations();

            foreach (Portal enemyPortal in enemyPortals)
            {
                foreach (FutureLocation elfLocation in myElvesLocations.Values)
                {
                    score -= Mathf.Pow(elfLocation.GetFutureLocation().Distance(enemyPortal), 0.7f) / Mathf.Pow(Constants.Game.ElfMaxSpeed, 0.7f);
                }
            }

            return score;
        }
    }
}
