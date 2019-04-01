using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMonitorEnemyManaFountains : ElfMonitorEnemyGameObject
    {
        public ElfMonitorEnemyManaFountains(float weight, float distanceTarget, float minDistanceFromEnemyDefendingObject, float maxRangeFromEnemyGameObjectCircle, Circle monitorArea) : base(weight, distanceTarget, minDistanceFromEnemyDefendingObject, maxRangeFromEnemyGameObjectCircle, monitorArea)
        {
        }

        protected override Dictionary<int, GameObject> GetEnemyGameObjectsDictionary()
        {
            Dictionary<int, GameObject> enemyManaFountains = new Dictionary<int, GameObject>();
            foreach (ManaFountain enemyManaFountain in Constants.GameCaching.GetEnemyManaFountainsInArea(GetMonitorArea()))
            {
                enemyManaFountains[enemyManaFountain.UniqueId] = enemyManaFountain;
            }
            return enemyManaFountains;
        }

        protected override float GetMaxRangeFromEnemyGameObjectCircle()
        {
            return Mathf.Min(maxRangeFromEnemyGameObjectCircle, (Constants.Game.MaxTurns - Constants.Game.Turn) * Constants.Game.ElfMaxSpeed);
        }

        protected override float GetMonitorAreaRadius()
        {
            return 20000;
        }
    }
}
