using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveToBuildManaFountain : Heuristic
    {
        private int desiredAmountOfManaFountains;

        public ElfMoveToBuildManaFountain(float weight, int desiredAmountOfManaFountains) : base(weight)
        {
            this.desiredAmountOfManaFountains = desiredAmountOfManaFountains;
        }

        private float GetLocationScore(Location currentLocation, Location elfFutureLocation)
        {
            int keepAwayDistance = Constants.Game.CastleSize + Constants.Game.ManaFountainSize + 25;
            Location targetLocation = Constants.Game.GetMyCastle().GetClosestBuildableLocation(currentLocation, false);// Constants.Game.GetMyCastle().GetNewLocation(currentLocation, 0, keepAwayDistance);

            float futureDist = elfFutureLocation.DistanceF(targetLocation);

            return -futureDist;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (Constants.GameCaching.GetMyPortals().Length <= 2) return 0;
            if (Constants.GameCaching.GetMyManaFountains().Length >= desiredAmountOfManaFountains) return 0;
            if (Constants.Game.Turn < 15) return 0;

            float highestScore = 0;
            bool iterated = false;

            foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
            {
                Location elfCurrentLocation = pair.Value.GetElf().GetLocation();
                Location elfNextLocation = pair.Value.GetFutureLocation();

                float tempScore = GetLocationScore(elfCurrentLocation, elfNextLocation);

                if (!iterated || tempScore > highestScore)
                {
                    iterated = true;
                    highestScore = tempScore;
                }
            }

            return highestScore / Constants.Game.ElfMaxSpeed;
        }
    }
}