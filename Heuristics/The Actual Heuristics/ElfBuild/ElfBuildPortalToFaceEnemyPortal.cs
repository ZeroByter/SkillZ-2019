using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfBuildPortalToFaceEnemyPortal : ElfBuildPortalToDefendCastle
    {
        public ElfBuildPortalToFaceEnemyPortal(float weight, int maxDistanceFromEnemy, int closestDistance) : base(weight, maxDistanceFromEnemy, closestDistance)
        {
        }

        public override List<GameObject> GetEnemyGameObjectInArea(Location portalLocation)
        {
            List<GameObject> enemyPortals = new List<GameObject>();
            Circle circle = new Circle(portalLocation, maxDistanceFromEnemy);
            enemyPortals.AddRange(Constants.GameCaching.GetEnemyPortalsInArea(circle));
            enemyPortals.AddRange(Constants.GameCaching.GetEnemyElvesInAreaCurrentlyBuildingPortals(circle));

            return enemyPortals;
        }
    }
}

/*Building Portal to face enemy portal.
  The scoring is per enemy portal.
  members:
    maxDistanceFromEnemyElf - the maximum distance of virtual portal from the enemy portal.
	dist -  the virtual portal should be closer to my castle than the enemy portal by at least 'dist'
  score: The scoring is per enemy portal.
    1 for each virtual portal that answers the memebers definitions
  defaults:
    maxDistanceFromEnemyElf:game.elfMaxSpeed*11
	dist: game.elfMaxSpeed*7*/