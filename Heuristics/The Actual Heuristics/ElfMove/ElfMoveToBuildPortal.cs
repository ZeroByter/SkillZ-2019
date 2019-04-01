using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveToBuildPortal : Heuristic
    {
        private Circle heuristicObjective;
        private float maxDistanceToReduce;
        private float minFactor;

        public ElfMoveToBuildPortal(float weight, Circle heuristicObjective, float maxDistanceToReduce) : base(weight)
        {
            if (heuristicObjective.GetRadius() < Constants.Game.ElfMaxSpeed)
            {
                throw new System.Exception("Radius can't be less than ElfMaxSpeed");
            }

            this.heuristicObjective = heuristicObjective;
            this.maxDistanceToReduce = maxDistanceToReduce;
            minFactor = 1 / (maxDistanceToReduce / Constants.Game.ElfMaxSpeed);
        }

        private float GetLocationScore(Location currentLocation, Location elfFutureLocation)
        {
            float currDist = currentLocation.DistanceF(heuristicObjective.GetCenter()) - heuristicObjective.GetRadius();
            if (currDist <= 0)
            {
                return 0;
            }
            float futureDist = elfFutureLocation.DistanceF(heuristicObjective.GetCenter()) - heuristicObjective.GetRadius();

            float score = (currDist - futureDist);
            float distanceFactor = 0;
            if (futureDist <= 0)
            {
                distanceFactor = 1;
            }
            else if (futureDist < maxDistanceToReduce)
            {
                distanceFactor = Mathf.Max(minFactor, (maxDistanceToReduce - futureDist) / maxDistanceToReduce);
            }
            return -1 * futureDist + score * distanceFactor;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            int countPortalsInArea = Constants.GameCaching.GetMyPortalsInArea(heuristicObjective).Count;
            int countManaFountainsInArea = Constants.GameCaching.GetMyManaFountainsInArea(new Circle(heuristicObjective.GetCenter(), heuristicObjective.GetRadius() + Constants.Game.ManaFountainSize * 2)).Count;

            float highestScore = 0;
            bool iterated = false;

            if (countPortalsInArea + countManaFountainsInArea <= 0) //if the area is not occupied
            {
                //int futurePortalsInArea = nextTurnActions.virtualGame.CountFuturePortalsInArea(heuristicObjective.location, heuristicObjective.radius + Constants.Game.PortalSize * 2); //starting from minus one because we don't count the portal we are currently checking, but we are only checking other future planned portals
                //if (futurePortalsInArea > 0) return 0;

                foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
                {
                    Location elfCurrentLocation = pair.Value.GetElf().GetLocation();
                    if (heuristicObjective.IsLocationInside(elfCurrentLocation)) return 0;

                    Location elfNextLocation = pair.Value.GetFutureLocation();

                    float tempScore = GetLocationScore(elfCurrentLocation, elfNextLocation);// -elfNextLocation.DistanceF(heuristicObjective.location) - (heuristicObjective.radius + Constants.Game.ElfMaxSpeed);

                    if (!iterated || tempScore >= highestScore)
                    {
                        iterated = true;
                        highestScore = tempScore;
                    }
                }
            }

            return highestScore / Constants.Game.ElfMaxSpeed;
        }

        public override string ToString()
        {
            return $"Heuristic {GetType().Name} has weight {weight} and objective {heuristicObjective}";
        }
    }
}

/* Elf should move to HeuristicObjective with the purpose of building a portal there
 * Members:
 *  - heuristicObjective = the area (location + radius) where we want to build a specific portal
 * 
 * Scoring:
 *   If there are no portals where we want to build the portal, for each elf future location, we return the highest score*/
