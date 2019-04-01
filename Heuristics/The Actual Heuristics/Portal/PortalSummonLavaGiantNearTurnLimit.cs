using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummonLavaGiantNearTurnLimit : Heuristic
    {
        private int turnsBeforeLimit;

        public PortalSummonLavaGiantNearTurnLimit(float weight, int turnsBeforeLimit) : base(weight)
        {
            this.turnsBeforeLimit = turnsBeforeLimit;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            Castle enemyCastle = Constants.Game.GetEnemyCastle();

            if (Constants.Game.Turn < Constants.Game.MaxTurns - turnsBeforeLimit) return 0;

            return virtualGame.GetCombinedDamageToEnemyCastle();
        }
    }
}