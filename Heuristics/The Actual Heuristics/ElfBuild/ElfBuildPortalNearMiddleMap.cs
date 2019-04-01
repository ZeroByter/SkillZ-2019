using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfBuildPortalNearMiddleMap : Heuristic
    {
        private int maxPortals;
        private float minDistanceFromEnemyObjects;
        private float distanceRatio;
        private float buildAreaRadius;

        public ElfBuildPortalNearMiddleMap(float weight, int maxPortals, float minDistanceFromEnemyObjects, float distanceRatio, float buildAreaRadius) : base(weight)
        {
            this.maxPortals = maxPortals;
            this.minDistanceFromEnemyObjects = minDistanceFromEnemyObjects;
            this.distanceRatio = distanceRatio;
            this.buildAreaRadius = buildAreaRadius;
        }

        private float GetPortalScore(Location futurePortalLocation)
        {
            float distanceBetweenCastles = Constants.Game.GetMyCastle().DistanceF(Constants.Game.GetEnemyCastle()) * distanceRatio;
            Location buildLocation = Constants.Game.GetEnemyCastle().GetNewLocation(futurePortalLocation, 0, Mathf.RoundToInt(distanceBetweenCastles));

            if (Constants.GameCaching.GetEnemyElvesInArea(new Circle(buildLocation, minDistanceFromEnemyObjects)).Count > 0) return 0;
            if (Constants.GameCaching.GetEnemyPortalsInArea(new Circle(buildLocation, minDistanceFromEnemyObjects)).Count > 0) return 0;

            return Mathf.Lerp(1, 0, futurePortalLocation.DistanceF(buildLocation) / buildAreaRadius);
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (Constants.GameCaching.GetMyPortals().Length >= maxPortals) return 0;

            float score = 0;

            foreach(var pair in virtualGame.futurePortals)
            {
                score += GetPortalScore(pair.Value.location);
            }

            return score;
        }
    }
}
