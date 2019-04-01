using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfDontBuildPortalIfTooManyElvesWithinRange : Heuristic
    {
        private int minNumOfEnemyElvesInRange;
        private float range;
        public ElfDontBuildPortalIfTooManyElvesWithinRange(float weight, int minNumOfEnemyElvesInRange, float range) : base(weight)
        {
            this.minNumOfEnemyElvesInRange = minNumOfEnemyElvesInRange;
            this.range = range;
        }


        private float GetPortalScore(Location portalLocation)
        {
            int numOfEnemyElvesInRange = Constants.GameCaching.GetEnemyElvesInArea(new Circle(portalLocation, range)).Count;
            return Mathf.Min(0, -1 * (numOfEnemyElvesInRange - minNumOfEnemyElvesInRange));
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (VirtualPortal portal in virtualGame.futurePortals.Values)
            {
                score += GetPortalScore(portal.location);
            }

            return score;
        }
    }
}