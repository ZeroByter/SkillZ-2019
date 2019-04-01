using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfSimpleMoveToBestBuildManaFountainArea : Heuristic
    {
        private List<Circle> heuristicObjectives = new List<Circle>();
        private float maxDistanceToCircle;

        public ElfSimpleMoveToBestBuildManaFountainArea(float weight, List<Circle> heuristicObjectives, float maxDistanceToCircle) : base(weight)
        {
            this.heuristicObjectives = heuristicObjectives;
            this.maxDistanceToCircle = maxDistanceToCircle;
        }

        private float GetLocationScore(Location elfFutureLocation, Circle heuristicObjective)
        {
            float futureDist = elfFutureLocation.DistanceF(heuristicObjective.GetCenter()) - heuristicObjective.GetRadius();
            if (futureDist <= 0)
            {
                return maxDistanceToCircle + Constants.Game.ElfMaxSpeed;
            }
            return Mathf.Max(0, maxDistanceToCircle - futureDist);
        }

        private float GetScore(Dictionary<int, FutureLocation> myFutureElfLocations, List<Circle> heuristicObjectives)
        {
            float score = 0;

            //currently this while is not being used there is a break in the end of the loop.
            //Only one Elf should go and build Mana Fountain.
            while (myFutureElfLocations.Count > 0 && heuristicObjectives.Count > 0)
            {
                float maxScore = 0;
                bool iterated = false;

                int myElfUniqueId = -1;
                Circle minHeuristicObjective = null;

                foreach (KeyValuePair<int, FutureLocation> myFutureElfLocation in myFutureElfLocations)
                {
                    foreach (Circle heuristicObjective in heuristicObjectives)
                    {
                        float tempScore = GetLocationScore(myFutureElfLocation.Value.GetFutureLocation(), heuristicObjective);

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
                break;
            }

            return score;
        }

        private List<Circle> GetNewListOfHeuristicObjectives()
        {
            List<Circle> heuristicObjectives = new List<Circle>();
            heuristicObjectives.Capacity = this.heuristicObjectives.Capacity;
            foreach (Circle heuristicObjective in this.heuristicObjectives)
            {
                int countManaFountainsInArea = Constants.GameCaching.GetMyManaFountainsInArea(new Circle(heuristicObjective.GetCenter(), heuristicObjective.GetRadius() + Constants.Game.ManaFountainSize*2)).Count;
                int countPortalsInArea = Constants.GameCaching.GetMyPortalsInArea(new Circle(heuristicObjective.GetCenter(), heuristicObjective.GetRadius() + Constants.Game.ManaFountainSize + Constants.Game.PortalSize)).Count;
                
                if (countPortalsInArea + countManaFountainsInArea <= 0)
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

            return (score / Constants.Game.ElfMaxSpeed) * Utilities.GetManaFountainRatio();
        }
    }
}


