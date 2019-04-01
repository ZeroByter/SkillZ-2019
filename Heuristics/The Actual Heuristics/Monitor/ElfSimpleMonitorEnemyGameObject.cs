using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    abstract class ElfSimpleMonitorEnemyGameObject : Heuristic
    {
        private float distanceTarget;
        private float minDistanceFromEnemyDefendingObject;
        private float maxRangeFromEnemyGameObjectCircle;
        protected Circle monitoredCircle;

        public ElfSimpleMonitorEnemyGameObject(
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

        protected virtual float GetSpeedUpMultiplier()
        {
            return 1;
        }

        private float GetScore(Dictionary<int, FutureLocation> myFutureElfLocations, Dictionary<int, GameObject> enemyGameObjects)
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
                    FutureLocation elfFutureLocation = myFutureElfLocation.Value;
                    foreach (KeyValuePair<int, GameObject> enemyGameObjectPair in enemyGameObjects)
                    {
                        float tempScore = GetPairScore(enemyGameObjectPair, elfFutureLocation.GetFutureLocation());
                        foreach (Spell spell in elfFutureLocation.GetElf().CurrentSpells)
                        {
                            if (spell is SpeedUp)
                            {
                                tempScore = tempScore * GetSpeedUpMultiplier();
                                break;
                            }
                        }
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

            return score;
        }

        private Dictionary<int, FutureLocation> GetFutureElfLocations(VirtualGame virtualGame)
        {
            Dictionary<int, FutureLocation> myFutureElfLocations = new Dictionary<int, FutureLocation>();

            foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
            {
                myFutureElfLocations.Add(pair.Key, pair.Value);
            }
            return myFutureElfLocations;
        }

        protected abstract Dictionary<int, GameObject> GetEnemyGameObjectsDictionary();


        protected virtual bool UseAllElves()
        {
            return false;
        }


        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            Dictionary<int, FutureLocation> myFutureElfLocations = GetFutureElfLocations(virtualGame);
            if (myFutureElfLocations.Count == 0) return 0;

            Dictionary<int, GameObject> enemyGameObjects = GetEnemyGameObjectsDictionary();
            if (enemyGameObjects.Count == 0) return 0;

            score = GetScore(myFutureElfLocations, enemyGameObjects);

            if (myFutureElfLocations.Count > 0 && UseAllElves())
            {
                enemyGameObjects = GetEnemyGameObjectsDictionary();
                score += GetScore(myFutureElfLocations, enemyGameObjects);
            }

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}
