using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfSimpleBuildPortal : Heuristic
    {
        public ElfSimpleBuildPortal(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            return virtualGame.futurePortals.Count;
        }
    }
}
