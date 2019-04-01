using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    abstract class ElfMonitorEnemyGameObject : Heuristic
    {
        private float distanceTarget;
        private float minDistanceFromEnemyDefendingObject;
        protected float maxRangeFromEnemyGameObjectCircle;

        protected Circle monitorArea;

        private float factor1Normalization;

        public ElfMonitorEnemyGameObject(float weight, float distanceTarget, float minDistanceFromEnemyDefendingObject, float maxRangeFromEnemyGameObjectCircle, Circle monitorArea) : base(weight)
        {
            this.distanceTarget = distanceTarget;
            this.minDistanceFromEnemyDefendingObject = minDistanceFromEnemyDefendingObject;
            this.maxRangeFromEnemyGameObjectCircle = maxRangeFromEnemyGameObjectCircle;
            this.monitorArea = monitorArea;

            factor1Normalization = 1 / (Mathf.Pow(GetMaxRangeFromEnemyGameObjectCircle(), 1.1f) - Mathf.Pow(GetMaxRangeFromEnemyGameObjectCircle() - Constants.Game.ElfMaxSpeed, 1.1f));
        }
        
        protected Circle GetMonitorArea()
        {
            return new Circle(monitorArea.GetCenter(), GetMonitorAreaRadius());
        }

        private float GetPairScore(KeyValuePair<int, GameObject> pair, Location elfLocation)
        {
            GameObject enemyGameObject = pair.Value;
            float distFromBuildLocationCircle = elfLocation.DistanceF(enemyGameObject) - distanceTarget;

            if (distFromBuildLocationCircle >= GetMaxRangeFromEnemyGameObjectCircle()) return 0;

            float factor1;
            if (distFromBuildLocationCircle <= 0)
            {
                factor1 = Mathf.Pow(GetMaxRangeFromEnemyGameObjectCircle(), 1.1f) + 1 / factor1Normalization;
            }
            else
            {
                factor1 = Mathf.Pow(GetMaxRangeFromEnemyGameObjectCircle() - distFromBuildLocationCircle, 1.1f);
            }
            float factor2 = Mathf.InverseLerp(0, (GetMonitorAreaRadius() - Constants.Game.LavaGiantAttackRange - Constants.Game.CastleSize), enemyGameObject.Distance(monitorArea.GetCenter()) - Constants.Game.LavaGiantAttackRange - Constants.Game.CastleSize);

            return factor1 * factor2;
        }

        private Dictionary<int, Location> GetValidFutureElfLocations(VirtualGame virtualGame)
        {
            List<GameObject> enemyGameObjectsThatCanAttackElf = Constants.GameCaching.GetAllEnemyObjectsThatCanAttackElf();
            Dictionary<int, Location> myFutureElfLocations = new Dictionary<int, Location>();

            foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
            {
                bool add = true;

                foreach (GameObject enemyObject in enemyGameObjectsThatCanAttackElf)
                {
                    if (enemyObject.InRange(pair.Value.GetFutureLocation(), Mathf.RoundToInt(monitorArea.GetRadius())))
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                {
                    myFutureElfLocations.Add(pair.Key, pair.Value.GetFutureLocation());
                }
            }
            return myFutureElfLocations;
        }

        private float GetScore(Dictionary<int, Location> myFutureElfLocations, Dictionary<int, GameObject> enemyGameObjects)
        {
            float score = 0;

            while (myFutureElfLocations.Count > 0 && enemyGameObjects.Count > 0)
            {
                float bestScore = 0;
                bool iterated = false;

                int myElfUniqueId = -1;
                int enemyGameObjectUniqueId = -1;

                foreach (KeyValuePair<int, Location> myFutureElfLocation in myFutureElfLocations)
                {
                    Location elfLocation = myFutureElfLocation.Value;
                    foreach (KeyValuePair<int, GameObject> enemyGameObjectPair in enemyGameObjects)
                    {
                        float tempScore = GetPairScore(enemyGameObjectPair, elfLocation);

                        if (!iterated || tempScore > bestScore)
                        {
                            iterated = true;
                            bestScore = tempScore;

                            myElfUniqueId = myFutureElfLocation.Key;
                            enemyGameObjectUniqueId = enemyGameObjectPair.Key;
                        }
                    }
                }

                myFutureElfLocations.Remove(myElfUniqueId);
                enemyGameObjects.Remove(enemyGameObjectUniqueId);

                score += bestScore;
            }

            return score * factor1Normalization;
        }

        protected abstract Dictionary<int, GameObject> GetEnemyGameObjectsDictionary();
        protected abstract float GetMaxRangeFromEnemyGameObjectCircle();
        protected abstract float GetMonitorAreaRadius();

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            Dictionary<int, Location> myFutureElfLocations = GetValidFutureElfLocations(virtualGame);
            if (myFutureElfLocations.Count == 0) return 0;

            Dictionary<int, GameObject> enemyGameObjects = GetEnemyGameObjectsDictionary();
            if (enemyGameObjects.Count == 0) return 0;

            score = GetScore(myFutureElfLocations, enemyGameObjects);

            return score;
        }
    }
}
