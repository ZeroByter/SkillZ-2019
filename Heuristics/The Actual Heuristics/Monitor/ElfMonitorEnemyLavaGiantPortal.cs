using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMonitorEnemyLavaGiantPortal : ElfMonitorEnemyGameObject
    {
        public ElfMonitorEnemyLavaGiantPortal(float weight, float distanceTarget, float minDistanceFromEnemyDefendingObject, float maxRangeFromEnemyGameObjectCircle, Circle monitorArea) : base(weight, distanceTarget, minDistanceFromEnemyDefendingObject, maxRangeFromEnemyGameObjectCircle, monitorArea)
        {
        }

        private bool IsLavaGiantOnPortal(Portal enemyPortal)
        {
            foreach(LavaGiant lavaGiant in Constants.GameCaching.GetEnemyLavaGiantsInArea(new Circle(enemyPortal, enemyPortal.Size)))
            {
                if (lavaGiant.GetLocation() == enemyPortal.GetLocation()) return true;
            }

            return false;
        }

        protected override Dictionary<int, GameObject> GetEnemyGameObjectsDictionary()
        {
            Dictionary<int, GameObject> enemyPortals = new Dictionary<int, GameObject>();

            foreach (Portal enemyPortal in Constants.GameCaching.GetEnemyPortalsInArea(GetMonitorArea()))
            {
                if(enemyPortal.CurrentlySummoning == "LavaGiant" || IsLavaGiantOnPortal(enemyPortal))
                {
                    enemyPortals[enemyPortal.UniqueId] = enemyPortal;
                }
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