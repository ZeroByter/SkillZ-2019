using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfSimpleCastInvisibility : Heuristic
    {
        public ElfSimpleCastInvisibility(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            return virtualGame.futureInvisibilitySpells.Count;
        }
    }
}
