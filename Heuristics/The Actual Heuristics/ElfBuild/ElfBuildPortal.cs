using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfBuildPortal : Heuristic
    {
        private Circle heuristicObjective;

        public ElfBuildPortal(float weight, Circle heuristicObjective) : base(weight)
        {
            this.heuristicObjective = heuristicObjective;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (heuristicObjective == null) return 0;

            int countPortalsInArea = Constants.GameCaching.GetMyPortalsInArea(heuristicObjective).Count;// GetCustomGameObjectsList(x => x.IsMine() && x is Portal && x.InRange(heuristicObjective.location, heuristicObjective.radius)).Count;

            if(countPortalsInArea > 0) //if there is atleast portal or future portal in the area
            {
                return 0;
            }
            else
            {
                int futurePortalsInArea = virtualGame.CountFuturePortalsInArea(heuristicObjective.GetCenter(), heuristicObjective.GetRadius()); //the goal is to build one portal in the radius so it doesn't matter how many futurePortal you have.
                if (futurePortalsInArea > 0) return 1;

                return 0;
            }
        }

        public override string ToString()
        {
            return $"Heuristic {GetType().Name} has weight {weight} and objective {heuristicObjective}";
        }
    }
}
