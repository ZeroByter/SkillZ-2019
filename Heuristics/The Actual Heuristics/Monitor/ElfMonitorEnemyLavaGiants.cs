using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMonitorEnemyLavaGiants : ElfMonitorEnemyGameObject
    {
        public ElfMonitorEnemyLavaGiants(float weight, float distanceTarget, float minDistanceFromEnemyDefendingObject, float maxRangeFromEnemyGameObjectCircle, Circle monitorArea) : base(weight, distanceTarget, minDistanceFromEnemyDefendingObject, maxRangeFromEnemyGameObjectCircle, monitorArea)
        {
        }

        protected override Dictionary<int, GameObject> GetEnemyGameObjectsDictionary()
        {
            Dictionary<int, GameObject> enemyLavaGiants = new Dictionary<int, GameObject>();

            foreach (LavaGiant enemyLavaGiant in Constants.GameCaching.GetEnemyLavaGiantsInArea(GetMonitorArea()))
            {
                enemyLavaGiants[enemyLavaGiant.UniqueId] = enemyLavaGiant;
            }
            return enemyLavaGiants;
        }

        protected override float GetMaxRangeFromEnemyGameObjectCircle()
        {
            return Mathf.Min(maxRangeFromEnemyGameObjectCircle, (Constants.Game.MaxTurns - Constants.Game.Turn) * Constants.Game.ElfMaxSpeed);
        }

        protected override float GetMonitorAreaRadius()
        {
            if (Constants.Game.Turn < 26) return 0;

            return Mathf.Min(monitorArea.GetRadius(), (Constants.Game.MaxTurns - Constants.Game.Turn) * Constants.Game.LavaGiantMaxSpeed);
        }
    }
}
