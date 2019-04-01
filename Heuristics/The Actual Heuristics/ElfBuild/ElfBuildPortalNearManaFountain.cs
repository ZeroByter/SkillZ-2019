using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfBuildPortalNearManaFountain : Heuristic
    {
        private float maxDistFromMyManaFountain;

        public ElfBuildPortalNearManaFountain(float weight, float maxDistFromMyManaFountain) : base(weight)
        {
            this.maxDistFromMyManaFountain = maxDistFromMyManaFountain;
        }

        private float GetFuturePortalScore(Location portalFutureLocation)
        {
            if (Constants.GameCaching.GetMyManaFountainsInArea(new Circle(portalFutureLocation, maxDistFromMyManaFountain)).Count == 0) return 0;

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
