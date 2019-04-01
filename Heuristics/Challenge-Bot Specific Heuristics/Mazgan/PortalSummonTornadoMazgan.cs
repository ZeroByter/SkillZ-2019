using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummonTornadoMazgan : Heuristic
    {
        public PortalSummonTornadoMazgan(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            score += virtualGame.futureTornadoes.Count;

            return score;
        }
    }
}