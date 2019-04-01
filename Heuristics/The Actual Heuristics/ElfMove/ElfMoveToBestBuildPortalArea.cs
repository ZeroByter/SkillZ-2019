using ElfKingdom;
using System.Collections.Generic;
using System.Linq;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveToBestBuildPortalArea : Heuristic
    {
        private List<Circle> heuristicObjectives = new List<Circle>();
        private float maxDistanceToReduce;
        private float minFactor;

        public ElfMoveToBestBuildPortalArea(float weight, List<Circle> heuristicObjectives, float maxDistanceToReduce) : base(weight)
        {
            this.heuristicObjectives = heuristicObjectives;
            this.maxDistanceToReduce = maxDistanceToReduce;
            minFactor = 1 / (maxDistanceToReduce / Constants.Game.ElfMaxSpeed);
        }

        private float GetLocationScore(Location currentLocation, Location elfFutureLocation, Circle heuristicObjective)
        {
            float currDist = currentLocation.DistanceF(heuristicObjective.GetCenter()) - heuristicObjective.GetRadius();
            if (currDist <= 0)
            {
                return 2 * Constants.Game.ElfMaxSpeed * Constants.Game.SpeedUpMultiplier;
            }
            float futureDist = elfFutureLocation.DistanceF(heuristicObjective.GetCenter()) - heuristicObjective.GetRadius();

            float score = (currDist - futureDist);
            float distanceFactor = 0;
            if (futureDist <= 0)
            {
                return Constants.Game.ElfMaxSpeed * Constants.Game.SpeedUpMultiplier;
            }
            else if (futureDist < maxDistanceToReduce)
            {
                distanceFactor = Mathf.Max(minFactor, (maxDistanceToReduce - futureDist) / maxDistanceToReduce);
            }
            return -1 * futureDist + score * distanceFactor;
        }

        private float GetScore(Dictionary<int, FutureLocation> myFutureElfLocations, List<Circle> heuristicObjectives)
        {
            float score = 0;

            while (myFutureElfLocations.Count > 0 && heuristicObjectives.Count > 0)
            {
                float maxScore = 0;
                bool iterated = false;

                int myElfUniqueId = -1;
                Circle minHeuristicObjective = null;

                foreach (KeyValuePair<int, FutureLocation> myFutureElfLocation in myFutureElfLocations)
                {
                    Location elfCurrentLocation = myFutureElfLocation.Value.GetElf().GetLocation();

                    foreach (Circle heuristicObjective in heuristicObjectives)
                    {
                        float tempScore = GetLocationScore(elfCurrentLocation, myFutureElfLocation.Value.GetFutureLocation(), heuristicObjective);

                        if (!iterated || tempScore > maxScore)
                        {
                            iterated = true;
                            maxScore = tempScore;

                            myElfUniqueId = myFutureElfLocation.Key;
                            minHeuristicObjective = heuristicObjective;
                        }
                    }
                }

                myFutureElfLocations.Remove(myElfUniqueId);
                heuristicObjectives.Remove(minHeuristicObjective);
                score += maxScore;
            }

            return score;
        }

        private List<Circle> GetNewListOfHeuristicObjectives()
        {
            List<Circle> heuristicObjectives = new List<Circle>();
            heuristicObjectives.Capacity = this.heuristicObjectives.Capacity;
            foreach(Circle heuristicObjective in this.heuristicObjectives)
            {
                int countPortalsInArea = Constants.GameCaching.GetMyPortalsInArea(new Circle(heuristicObjective.GetCenter(), heuristicObjective.GetRadius() + Constants.Game.PortalSize * 2)).Count;
                int countManaFountainsInArea = Constants.GameCaching.GetMyManaFountainsInArea(new Circle(heuristicObjective.GetCenter(), heuristicObjective.GetRadius() + Constants.Game.ManaFountainSize * 2)).Count;

                if(countPortalsInArea + countManaFountainsInArea <= 0)
                {
                    heuristicObjectives.Add(heuristicObjective);
                }
            }
            return heuristicObjectives;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            Dictionary<int, FutureLocation> myFutureElfLocations = new Dictionary<int, FutureLocation>();
            foreach (var pair in virtualGame.GetFutureLocations()) myFutureElfLocations.Add(pair.Key, pair.Value);
            if (myFutureElfLocations.Count == 0) return 0;

            List<Circle> heuristicObjectives = GetNewListOfHeuristicObjectives();
            if (heuristicObjectives.Count == 0) return 0;

            score = GetScore(myFutureElfLocations, heuristicObjectives);

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}

/* Elf should move to the best heuristic objective in the list of objectives with the purpose of building a portal there
 * Members:
 *  - heuristicObjectives = list of areas (location + radius) where we want to build specific portals
 * 
 * Scoring:
 *   Greedy method to get best location for each elf
 */
