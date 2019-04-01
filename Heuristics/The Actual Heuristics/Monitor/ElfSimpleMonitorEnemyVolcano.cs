using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfSimpleMonitorVolcano : ElfSimpleMonitorEnemyGameObject
    {
        private int maxTurnsToActive;

        public ElfSimpleMonitorVolcano(float weight, float distanceTarget, float minDistanceFromEnemyDefendingObject, float maxRangeFromEnemyGameObjectCircle, Circle monitoredCircle, int maxTurnsToActive) : base(weight, distanceTarget, minDistanceFromEnemyDefendingObject, maxRangeFromEnemyGameObjectCircle, monitoredCircle)
        {
            this.maxTurnsToActive = maxTurnsToActive;
        }

        protected override float GetSpeedUpMultiplier()
        {
            return 2.5f;
        }

        protected override Dictionary<int, GameObject> GetEnemyGameObjectsDictionary()
        {
            Dictionary<int, GameObject> gameObjectsDictionary = new Dictionary<int, GameObject>();

            Volcano volcano = Constants.Game.GetVolcano();

            if (volcano.TurnsToActive < maxTurnsToActive && volcano.DamageByEnemy <= volcano.MaxHealth / 2)
            {
                gameObjectsDictionary.Add(volcano.UniqueId, volcano);
            }
            
            return gameObjectsDictionary;
        }
    }
}
