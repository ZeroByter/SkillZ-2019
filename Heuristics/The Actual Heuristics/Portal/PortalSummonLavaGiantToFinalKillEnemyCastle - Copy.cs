using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummonLavaGiantToFinalKillEnemyCastle : Heuristic
    {
        public PortalSummonLavaGiantToFinalKillEnemyCastle(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            Castle enemyCastle = Constants.Game.GetEnemyCastle();

            int combinedDamageOutputToCastle = virtualGame.GetCombinedDamageToEnemyCastle();

            if (combinedDamageOutputToCastle > enemyCastle.CurrentHealth * 2)
            {
                return combinedDamageOutputToCastle / enemyCastle.CurrentHealth;
            }
            else
            {
                return 0;
            }
        }
    }
}

/*Portals should summon LavaGiant if we detect that we are aboutt to kill the enemy castle
 Scoring: get combined damage output to castle. We we have enough damage to kill castle twice right now, we are about to finally kill the enemy castle, and return combined damage output / current enemy castle health*/