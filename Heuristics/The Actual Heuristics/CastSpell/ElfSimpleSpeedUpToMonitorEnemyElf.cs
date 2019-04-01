using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfSimpleSpeedUpToMonitorEnemyElf : ElfSimpleSpeedUpToMonitorEnemyGameObjects
    {
        public ElfSimpleSpeedUpToMonitorEnemyElf(float weight, float distanceTarget, float minDistanceFromEnemyDefendingObject, float maxRangeFromEnemyGameObjectCircle, Circle monitoredCircle) : base(weight, distanceTarget, minDistanceFromEnemyDefendingObject, maxRangeFromEnemyGameObjectCircle, monitoredCircle)
        {
        }

        protected override Dictionary<int, GameObject> GetEnemyGameObjectsDictionary()
        {
            Dictionary<int, GameObject> gameObjectsDictionary = new Dictionary<int, GameObject>();

            foreach (GameObject gameObject in Constants.GameCaching.GetEnemyLivingElves())
            {
                if (monitoredCircle == null || gameObject.InRange(monitoredCircle.GetCenter(), Mathf.FloorToInt(monitoredCircle.GetRadius())))
                {
                    gameObjectsDictionary[gameObject.UniqueId] = gameObject;
                }
            }
            return gameObjectsDictionary;
        }
    }
}
