using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfBuildCastleProtectionPortals : Heuristic
    {
        private int amountOfPortals;

        public ElfBuildCastleProtectionPortals(float weight, int amountOfPortals) : base(weight)
        {
            this.amountOfPortals = amountOfPortals;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            Castle myCastle = Constants.Game.GetMyCastle();
            Circle buildingArea = new Circle(myCastle, myCastle.Size * 3);

            if (Constants.GameCaching.GetMyPortalsInArea(buildingArea).Count + virtualGame.CountFuturePortalsInArea(buildingArea) > amountOfPortals) return 0;

            int score = 0;

            foreach (KeyValuePair<int, VirtualPortal> pair in virtualGame.futurePortals)
            {
                Location location = pair.Value.location;

                if (buildingArea.IsLocationInside(location))
                {
                    score++;
                }
            }

            return score;
        }
    }
}