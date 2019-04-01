using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveToBuildProtectionPortals : Heuristic
    {
        private int amountOfPortals;

        public ElfMoveToBuildProtectionPortals(float weight, int amountOfPortals) : base(weight)
        {
            this.amountOfPortals = amountOfPortals;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            Castle myCastle = Constants.Game.GetMyCastle();
            Circle buildingArea = new Circle(myCastle, myCastle.Size * 3);

            if (Constants.GameCaching.GetMyPortalsInArea(buildingArea).Count + virtualGame.CountFuturePortalsInArea(buildingArea) > amountOfPortals) return 0;

            float highestScore = 0;
            bool iterated = false;

            foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
            {
                Location elfNextLocation = pair.Value.GetFutureLocation();

                float tempScore = -1 * elfNextLocation.Distance(myCastle.GetClosestBuildableLocation(elfNextLocation));

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