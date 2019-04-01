using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMonitorAreaFromEnemyElves : Heuristic
    {
        private float minRadius;
        private Circle monitorArea;

        public ElfMonitorAreaFromEnemyElves(float weight, float minRadius, Circle monitorArea) : base(weight)
        {
            this.minRadius = minRadius;
            this.monitorArea = monitorArea;
        }

        private float GetScore(Dictionary<int, FutureLocation> myFutureElfLocations, Dictionary<int, Elf> enemyElves)
        {
            float score = 0;

            while (myFutureElfLocations.Count > 0 && enemyElves.Count > 0)
            {
                float minDistance = 0;
                bool iterated = false;

                int myElfUniqueId = -1;
                int enemyElfUniqueId = -1;

                foreach (KeyValuePair<int, FutureLocation> myFutureElfLocation in myFutureElfLocations)
                {
                    foreach (KeyValuePair<int, Elf> enemyElfPair in enemyElves)
                    {
                        int distanceModifier = 0;
                        Elf enemyElf = enemyElfPair.Value;
                        Location enemyElfLocation = enemyElf.GetLocation();
                        /*if (enemyElf.Invisible)
                        {
                            distanceModifier = Constants.Game.ElfMaxSpeed * Constants.Game.SpeedUpMultiplier * Constants.Game.SpeedUpExpirationTurns;
                            enemyElfLocation = LastPosition.GetLastLocation(enemyElf);
                        }
                        else
                        {
                            enemyElfLocation = enemyElf.GetLocation();
                        }*/

                        float distance = enemyElfLocation.DistanceF(myFutureElfLocation.Value.GetFutureLocation()) + distanceModifier;

                        if (!iterated || distance < minDistance)
                        {
                            iterated = true;
                            minDistance = distance;

                            myElfUniqueId = myFutureElfLocation.Key;
                            enemyElfUniqueId = enemyElfPair.Key;
                        }
                    }
                }

                myFutureElfLocations.Remove(myElfUniqueId);
                enemyElves.Remove(enemyElfUniqueId);
                //Logger.Debug("minRadius = {0}, minDistance = {1}", minRadius, minDistance);
                score -= Mathf.Max(minRadius, minDistance);
            }

            return score;
        }

        private Dictionary<int, Elf> GetEnemyElvesDictionary()
        {
            Dictionary<int, Elf> enemyElves = new Dictionary<int, Elf>();
            foreach (Elf enemyElf in Constants.GameCaching.GetEnemyElvesInArea(monitorArea))
            {
                enemyElves[enemyElf.UniqueId] = enemyElf;
            }
            return enemyElves;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            Dictionary<int, FutureLocation> myFutureElfLocations = virtualGame.GetFutureLocations();
            if (myFutureElfLocations.Count == 0) return 0;

            Dictionary<int, Elf> enemyElves = GetEnemyElvesDictionary();
            if (enemyElves.Count == 0) return 0;

            score = GetScore(myFutureElfLocations, enemyElves);

            /*if(myFutureElfLocations.Count > 0)
            {
                enemyElves = GetEnemyElvesDictionary();

                score += GetScore(myFutureElfLocations, enemyElves);
            }*/

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}

/*Elves 'monitoring' and keeping a distance from enemy elves, so that we may go to attack range in the future
 Members:
   - radius - 
   - monitorArea - the area (location + radius) where we want to monitor enemy elves (for instance maybe our castle or something)
  Scoring:
   - for each future elf move location, we get dictionary of all elves we want to monitor and return minimum distance*/