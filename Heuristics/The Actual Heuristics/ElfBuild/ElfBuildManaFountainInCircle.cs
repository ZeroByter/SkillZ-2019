using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfBuildManaFountainInCircle : Heuristic
    {
        private Circle heuristicObjective;

        public ElfBuildManaFountainInCircle(float weight, Circle heuristicObjective) : base(weight)
        {
            this.heuristicObjective = heuristicObjective;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (heuristicObjective == null) return 0;

            int countManaFountainsInArea = Constants.GameCaching.GetMyManaFountainsInArea(heuristicObjective).Count;

            if (countManaFountainsInArea > 0)
            {
                return 0;
            }
            else
            {
                int futurePortalsInArea = virtualGame.CountFutureManaFountainsInArea(heuristicObjective.GetCenter(), heuristicObjective.GetRadius());
                if (futurePortalsInArea > 0)
                {
                    return Utilities.GetManaFountainRatio(1.25f, 0.25f);
                }

                return 0;
            }
        }

        public override string ToString()
        {
            return $"Heuristic {GetType().Name} has weight {weight} and objective {heuristicObjective}";
        }
    }
}