using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfSimpleSpeedUpToMonitorEnemyManaFountain : ElfSimpleSpeedUpToMonitorEnemyGameObjects
    {
        public ElfSimpleSpeedUpToMonitorEnemyManaFountain(float weight, float distanceTarget, float minDistanceFromEnemyDefendingObject, float maxRangeFromEnemyGameObjectCircle, Circle monitoredCircle) : base(weight, distanceTarget, minDistanceFromEnemyDefendingObject, maxRangeFromEnemyGameObjectCircle, monitoredCircle)
        {
        }

        protected override Dictionary<int, GameObject> GetEnemyGameObjectsDictionary()
        {
            Dictionary<int, GameObject> gameObjectsDictionary = new Dictionary<int, GameObject>();

            if (monitoredCircle != null)
            {
                foreach (GameObject gameObject in Constants.GameCaching.GetEnemyManaFountainsInArea(monitoredCircle))
                {
                    gameObjectsDictionary[gameObject.UniqueId] = gameObject;
                }
            }
            else
            {
                foreach (GameObject gameObject in Constants.GameCaching.GetEnemyManaFountains())
                {
                    gameObjectsDictionary[gameObject.UniqueId] = gameObject;
                }
            }
            return gameObjectsDictionary;
        }
    }
}
