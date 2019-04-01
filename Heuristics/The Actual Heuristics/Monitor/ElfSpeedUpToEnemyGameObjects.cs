using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    abstract class ElfSpeedUpToEnemyGameObjects : Heuristic
    {
        private float distanceTarget;
        private float minDistanceFromEnemyDefendingObject;
        private float maxRangeFromEnemyGameObjectCircle;
        protected Circle monitoredCircle;

        public ElfSpeedUpToEnemyGameObjects(
            float weight, float distanceTarget,
            float minDistanceFromEnemyDefendingObject,
            float maxRangeFromEnemyGameObjectCircle,
            Circle monitoredCircle) : base(weight)
        {
            this.distanceTarget = distanceTarget;
            this.minDistanceFromEnemyDefendingObject = minDistanceFromEnemyDefendingObject;
            this.maxRangeFromEnemyGameObjectCircle = maxRangeFromEnemyGameObjectCircle;
            this.monitoredCircle = monitoredCircle;
        }

        private float GetPairScore(KeyValuePair<int, GameObject> enemyKeyValuePair, Location elfLocation)
        {
            GameObject enemyGameObject = enemyKeyValuePair.Value;
            float elfDistFromEnemyCircle = elfLocation.DistanceF(enemyGameObject) - distanceTarget;

            if (elfDistFromEnemyCircle >= maxRangeFromEnemyGameObjectCircle) return 0;
            if (elfDistFromEnemyCircle <= 0) return maxRangeFromEnemyGameObjectCircle + Constants.Game.ElfMaxSpeed;

            return maxRangeFromEnemyGameObjectCircle - elfDistFromEnemyCircle;
        }


        private float GetScore(VirtualGame virtualGame, Dictionary<int, FutureLocation> myFutureElfLocations, Dictionary<int, GameObject> enemyGameObjects)
        {
            float score = 0;

            while (myFutureElfLocations.Count > 0 && enemyGameObjects.Count > 0)
            {
                float bestScore = 0;
                bool iterated = false;

                int myElfUniqueId = -1;
                int enemyGameObjectUniqueId = -1;

                foreach (KeyValuePair<int, FutureLocation> myFutureElfLocation in myFutureElfLocations)
                {
                    Location elfLocation = myFutureElfLocation.Value.GetFutureLocation();
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

                if (virtualGame.futureSpeedUpSpells.ContainsKey(myElfUniqueId))
                {
                    Elf myElf = myFutureElfLocations[myElfUniqueId].GetElf();
                    GameObject enemyGameObject = enemyGameObjects[enemyGameObjectUniqueId];
                    float dist = myElf.Distance(enemyGameObject);
                    if(dist > Constants.Game.ElfMaxSpeed * 2)
                    {
                        score += 1;
                    }
                }

                myFutureElfLocations.Remove(myElfUniqueId);
                enemyGameObjects.Remove(enemyGameObjectUniqueId);
            }

            return score;
        }

        private Dictionary<int, FutureLocation> GetFutureElfLocations(VirtualGame virtualGame)
        {
            Dictionary<int, FutureLocation> myFutureElfLocations = new Dictionary<int, FutureLocation>();
            bool isExistElfWithSpeedupSpell = false;
            foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
            {
                if (Constants.GameCaching.GetEnemyIceTrollsInArea(new Circle(pair.Value.GetFutureLocation(), minDistanceFromEnemyDefendingObject)).Count > 0) continue;
                if (Constants.GameCaching.GetEnemyElvesInArea(new Circle(pair.Value.GetFutureLocation(), minDistanceFromEnemyDefendingObject)).Count > 0) continue;

                if (virtualGame.futureSpeedUpSpells.ContainsKey(pair.Key)) isExistElfWithSpeedupSpell = true;
                myFutureElfLocations.Add(pair.Key, pair.Value);
            }
            if (isExistElfWithSpeedupSpell)
            {
                return myFutureElfLocations;
            }
            else
            {
                return new Dictionary<int, FutureLocation>();
            }
            
        }

        protected abstract Dictionary<int, GameObject> GetEnemyGameObjectsDictionary();


        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            if (virtualGame.futureSpeedUpSpells.Count == 0) return 0;

            Dictionary<int, FutureLocation> myFutureElfLocations = GetFutureElfLocations(virtualGame);
            if (myFutureElfLocations.Count == 0) return 0;

            Dictionary<int, GameObject> enemyGameObjects = GetEnemyGameObjectsDictionary();
            if (enemyGameObjects.Count == 0) return 0;

            score = GetScore(virtualGame, myFutureElfLocations, enemyGameObjects);

            return score;
        }
    }
}
