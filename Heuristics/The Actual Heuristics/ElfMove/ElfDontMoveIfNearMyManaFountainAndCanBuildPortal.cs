using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfDontMoveIfNearMyManaFountainAndCanBuildPortal : Heuristic
    {
        private float maxDistFromMyManaFountain;

        public ElfDontMoveIfNearMyManaFountainAndCanBuildPortal(float weight, float maxDistFromMyManaFountain) : base(weight)
        {
            this.maxDistFromMyManaFountain = maxDistFromMyManaFountain;
        }

        private float GetElfScore(FutureLocation elfFutureLocation)
        {
            Location curLocation = elfFutureLocation.GetElf().GetLocation();
            Location futureLocation = elfFutureLocation.GetFutureLocation();
            if (curLocation.Row == futureLocation.Row && curLocation.Col == futureLocation.Col) return 0;
            if (!Constants.Game.CanBuildPortalAt(curLocation)) return 0;
            if (Constants.GameCaching.GetMyManaFountainsInArea(new Circle(curLocation, maxDistFromMyManaFountain)).Count == 0) return 0;

            return -1;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
            {
                score += GetElfScore(pair.Value);
            }

            return score;
        }
    }
}
