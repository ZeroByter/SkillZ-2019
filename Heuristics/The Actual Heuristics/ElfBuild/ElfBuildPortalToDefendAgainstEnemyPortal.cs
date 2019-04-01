using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfBuildPortalToDefendAgainstEnemyPortal : Heuristic
    {
        public ElfBuildPortalToDefendAgainstEnemyPortal(float weight) : base(weight)
        {
        }

        private float GetFuturePortalScore(VirtualPortal virtualPortal)
        {
            float score = 0;



            return score;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (VirtualPortal virtualPortal in virtualGame.futurePortals.Values)
            {
                score += GetFuturePortalScore(virtualPortal);
            }

            return score;
        }
    }
}