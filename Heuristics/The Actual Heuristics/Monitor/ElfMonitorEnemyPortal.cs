using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMonitorEnemyPortal : ElfMonitorEnemyGameObject
    {
        public ElfMonitorEnemyPortal(float weight, float distanceTarget, float minDistanceFromEnemyDefendingObject, float maxRangeFromEnemyGameObjectCircle, Circle monitorArea) : base(weight, distanceTarget, minDistanceFromEnemyDefendingObject, maxRangeFromEnemyGameObjectCircle, monitorArea)
        {
        }

        protected override Dictionary<int, GameObject> GetEnemyGameObjectsDictionary()
        {
            Dictionary<int, GameObject> enemyPortals = new Dictionary<int, GameObject>();

            foreach (Portal enemyPortal in Constants.GameCaching.GetEnemyPortalsInArea(GetMonitorArea()))
            {
                enemyPortals[enemyPortal.UniqueId] = enemyPortal;
            }
            return enemyPortals;
        }

        protected override float GetMaxRangeFromEnemyGameObjectCircle()
        {
            return Mathf.Min(maxRangeFromEnemyGameObjectCircle, (Constants.Game.MaxTurns - Constants.Game.Turn) * Constants.Game.ElfMaxSpeed);
        }

        protected override float GetMonitorAreaRadius()
        {
            return Mathf.Min(monitorArea.GetRadius(), (Constants.Game.MaxTurns - Constants.Game.Turn) * Constants.Game.ElfMaxSpeed);
        }
    }
}

/* Heuristic for keeping a short distance from enemy attacking portal
in order to get to an attacking range when the chance comes.
In this heuristic we will use the same greedy method that we already used in other places.
Meaning, that the scoring will be for the pair elf and enemy attacking portal.
enemy attacking portal - enemy portal which is close enough to send a lava that will attack my castle

members:
	distanceTarget - The goal is to get to a distanceTraget distance from the attacking portal
	minDistanceFromEnemyDefendingObject - Don't get nearer to the attacking portal if it gets you within minDistanceFromEnemyDefendingObject range from the enemy ice troll or elf.
	maxRangeFromPortalCircle - The distance from the circle of an attacking portal that my elf is being considered as an option for that portal.
							   The circle of an attacking portal is the location of the portal and the distanceTarget.

scoring:
  greedy method:
  in a while loop each time decide which pair <my elf, enemy portal> get the best score
  scoring a pair:
    should take in to account 2 factors and multipy them together:
	- how close my elf to the distanceTarget in relation to maxRangeFromPortalCircle. I suggest the following function: Math.pow((maxRangeFromPortalCircle - distFromPortalCircle),1.1)
	- how close the portal is to my castle attack range I suggest here a simple linear funtion over the distance. the factor should be between 0 and 1.

	Notice that you should handle the following problem: 
		if your elf is very close to the targetDistance (for example 1 unit away) then the action of getting
		in to the target distance should get the same score as entering from a larger distance (for example 100)
		If you will not do that many times the heuristic will not bring the elf in to the circle.

I forgot to say: try to normalize the score to be between 0 and 1 */