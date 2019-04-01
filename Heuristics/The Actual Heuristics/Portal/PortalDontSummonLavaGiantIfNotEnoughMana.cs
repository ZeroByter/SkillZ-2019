using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalDontSummonLavaGiantIfNotEnoughMana : Heuristic
    {
        private int minimumMana;

        public PortalDontSummonLavaGiantIfNotEnoughMana(float weight, int minimumMana) : base(weight)
        {
            this.minimumMana = minimumMana;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if(virtualGame.mana <= minimumMana)
            {
                return -1 * virtualGame.futureLavaGiants.Count;
            }
            else
            {
                return 0;
            }
        }
    }
}
