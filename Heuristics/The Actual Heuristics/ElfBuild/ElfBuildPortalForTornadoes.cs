using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfBuildPortalForTornadoes : Heuristic
    {

        public ElfBuildPortalForTornadoes(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            int score = 0;

            if(Constants.GameCaching.GetEnemyPortals().Length > 2 && Constants.GameCaching.GetMyPortals().Length < 2)
            {
                foreach(VirtualPortal virtualPortal in virtualGame.futurePortals.Values)
                {
                    score += 1;
                }
            }

            return score;
        }
    }
}