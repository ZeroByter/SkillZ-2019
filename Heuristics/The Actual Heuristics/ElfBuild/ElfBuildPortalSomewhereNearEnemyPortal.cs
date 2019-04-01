using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfBuildPortalSomewhereNearEnemyPortal : Heuristic
    {
        private float rangeFromEnemyPortal;

        public ElfBuildPortalSomewhereNearEnemyPortal(float weight, float rangeFromEnemyPortal) : base(weight)
        {
            this.rangeFromEnemyPortal = rangeFromEnemyPortal;
        }

        private float GetPortalScore(Location futurePortalLocation)
        {
            float score = 0;

            foreach(var enemyPortal in Constants.GameCaching.GetEnemyPortals())
            {
                float distance = futurePortalLocation.DistanceF(enemyPortal);

                if (distance < rangeFromEnemyPortal)
                {
                    score += Mathf.Lerp(0, 1, distance / rangeFromEnemyPortal); //the closer we are the maximum distance to build portal from enemy portal, the higher the score
                }
            }

            return score;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (Constants.GameCaching.GetEnemyPortals().Length == 0) return 0;

            float score = 0;

            foreach(var pair in virtualGame.futurePortals)
            {
                score += GetPortalScore(pair.Value.location);
            }

            return score;
        }
    }
}
