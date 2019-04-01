using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummonLavaGiantsHPBHAA : Heuristic
    {
        public PortalSummonLavaGiantsHPBHAA(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            if(Constants.Game.Turn > 35)
            {
                score += virtualGame.futureLavaGiants.Count;
            }

            return score;
        }
    }
}