using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveToCircleAngryBirdBot : Heuristic
    {
        private Circle heuristicObjective;
        private float maxDistanceToReduce;
        private float minFactor;

        public ElfMoveToCircleAngryBirdBot(float weight, Circle heuristicObjective, float maxDistanceToReduce) : base(weight)
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
            float score = 0;

            foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
            {
                if(pair.Value.GetElf().Id == 0)
                {
                    Location elfCurrentLocation = pair.Value.GetFutureLocation().GetLocation();
                    if (heuristicObjective.IsLocationInside(elfCurrentLocation)) return 0;

                    Location elfNextLocation = pair.Value.GetFutureLocation();

                    score += GetLocationScore(elfCurrentLocation, elfNextLocation);// -elfNextLocation.DistanceF(heuristicObjective.location) - (heuristicObjective.radius + Constants.Game.ElfMaxSpeed);
                }
            }

            return score / Constants.Game.ElfMaxSpeed;
        }

        public override string ToString()
        {
            return $"Heuristic {GetType().Name} has weight {weight} and objective {heuristicObjective}";
        }
    }
}