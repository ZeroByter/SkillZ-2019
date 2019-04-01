using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    abstract class ElfBuildPortalToDefendCastle : Heuristic
    {
        public float maxDistanceFromEnemy;
        public float closestDistance;

        public ElfBuildPortalToDefendCastle(float weight, float maxDistanceFromEnemy, float closestDistance) : base(weight)
        {
            this.maxDistanceFromEnemy = maxDistanceFromEnemy;
            this.closestDistance = closestDistance;
        }

        public abstract List<GameObject> GetEnemyGameObjectInArea(Location portalLocation);

        protected int GetPortalScore(VirtualGame virtualGame, Location portalLocation)
        {
            Castle myCastle = Constants.Game.GetMyCastle();

            int score = 0;
            
            foreach(GameObject enemyGameObject in GetEnemyGameObjectInArea(portalLocation))
            {
                //distance from planned portal location to our castle
                int distanceToMyCastle = portalLocation.Distance(myCastle);
                //living enemy elf distance to our castle
                int enemyDistanceToMyCastle = enemyGameObject.Distance(myCastle);

                //if the enemy distance to our castle minus our distance to our castle is bigger than closestDistance
                if (enemyDistanceToMyCastle - distanceToMyCastle > closestDistance)
                {
                    score++;
                }
            }

            return score;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            int score = 0;
            
            foreach (VirtualPortal portal in virtualGame.futurePortals.Values)
            {
                score += GetPortalScore(virtualGame, portal.location);
            }

            return score;
        }
    }
}


/*Building Portal in order to prevent enemy elf to get to my castle.
  The scoring is per enemy elf.
  members:
    maxDistanceFromEnemyElf - the maximum distance of virtual portal from the enemy elf.
	dist -  the virtual portal should be closer to my castle than the enemy elf by at least 'dist'
  score: The scoring is per enemy elf.
    1 for each virtual portal that answers the memebers definitions
  defaults:
    maxDistanceFromEnemyElf:elfAttackRange + (portalBuildingDuration + iceTrollSummoningDuration + 3)*elfMaxSpeed
	dist: (iceTrollSummoningDuration + 4) * game.elfMaxSpeed*/