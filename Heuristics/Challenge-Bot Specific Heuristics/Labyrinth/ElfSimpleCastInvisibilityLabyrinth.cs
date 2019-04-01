using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfSimpleCastInvisibilityLabyrinth : Heuristic
    {
        public ElfSimpleCastInvisibilityLabyrinth(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (Constants.Game.GetAllMyElves()[0].Invisible)
                return -1;

            return virtualGame.futureInvisibilitySpells.Count;
        }
    }
}