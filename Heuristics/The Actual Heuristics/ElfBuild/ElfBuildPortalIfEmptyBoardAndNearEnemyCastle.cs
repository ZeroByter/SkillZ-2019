using ElfKingdom;
using System.Collections.Generic;

//Goal of this heuristic: If we cleared the field from enemy objects then our high priority goal should be building portals near the enemy castle.

namespace SkillZ.IndividualHeuristics
{
    class ElfBuildPortalIfEmptyBoardAndNearEnemyCastle : Heuristic
    {
        private float nearByDist;

        public ElfBuildPortalIfEmptyBoardAndNearEnemyCastle(float weight, float nearByDist) : base(weight)
        {
            this.nearByDist = nearByDist;
        }

        private float GetVirtualPortalScore(VirtualPortal virtualPortal)
        {
            float distanceToEnemyCastleCircle = virtualPortal.location.Distance(Constants.Game.GetEnemyCastle().GetLocation()) - Constants.Game.CastleSize;
            if (distanceToEnemyCastleCircle > nearByDist) return 0;
            float score = (nearByDist - distanceToEnemyCastleCircle) / nearByDist;

            //the reason that we didn't just added 1 was to give higher priority to a closer place.
            //Still all the locations get near 1. By doing that we ensure to build 2 portals if there is enough mana and 2 elves are close to the castle
            return (score + 5) / 6;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (Constants.GameCaching.GetEnemyLivingElves().Length > 0) return 0;
            if (Constants.GameCaching.GetEnemyPortals().Length > 0) return 0;
            if (Constants.GameCaching.GetEnemyManaFountains().Length > 0) return 0;
            if (Constants.GameCaching.GetEnemyIceTrolls().Length > 0) return 0;
            if (Constants.GameCaching.GetEnemyTornadoes().Length > 0) return 0;

            float score = 0;
            foreach(VirtualPortal virtualPortal in virtualGame.futurePortals.Values)
            {
                score += GetVirtualPortalScore(virtualPortal);
            }

            return score;
        }

        public override string ToString()
        {
            return $"Heuristic {GetType().Name} has weight {weight} and radius {nearByDist}";
        }
    }
}
