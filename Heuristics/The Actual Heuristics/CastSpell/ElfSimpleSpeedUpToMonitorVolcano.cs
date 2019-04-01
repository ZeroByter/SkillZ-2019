using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfSimpleSpeedUpToMonitorVolcano : ElfSimpleSpeedUpToMonitorEnemyGameObjects
    {
        public ElfSimpleSpeedUpToMonitorVolcano(float weight, float distanceTarget, float minDistanceFromEnemyDefendingObject, float maxRangeFromEnemyGameObjectCircle, Circle monitoredCircle) : base(weight, distanceTarget, minDistanceFromEnemyDefendingObject, maxRangeFromEnemyGameObjectCircle, monitoredCircle)
        {
        }

        protected override Dictionary<int, GameObject> GetEnemyGameObjectsDictionary()
        {
            Dictionary<int, GameObject> gameObjectsDictionary = new Dictionary<int, GameObject>();

            Volcano volcano = Constants.Game.GetVolcano();

            if (volcano.IsActive() && volcano.DamageByEnemy <= volcano.MaxHealth / 2)
            {
                gameObjectsDictionary.Add(volcano.UniqueId, volcano);
            }

            return gameObjectsDictionary;
        }
    }
}
