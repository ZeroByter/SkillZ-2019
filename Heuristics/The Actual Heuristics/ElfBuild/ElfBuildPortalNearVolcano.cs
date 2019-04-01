using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfBuildPortalNearVolcano : Heuristic
    {
        private float maxDistFromVolcano;

        public ElfBuildPortalNearVolcano(float weight, float maxDistFromVolcano) : base(weight)
        {
            this.maxDistFromVolcano = maxDistFromVolcano;
        }

        private float GetFuturePortalScore(Location portalFutureLocation)
        {
            Volcano volcano = Constants.Game.GetVolcano();

            if (volcano.Distance(portalFutureLocation) > maxDistFromVolcano) return 0;

            return 1;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (KeyValuePair<int, VirtualPortal> pair in virtualGame.futurePortals)
            {
                score += GetFuturePortalScore(pair.Value.location);
            }

            return score;
        }
    }
}
