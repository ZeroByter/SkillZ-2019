using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveAwayFromMapEdges : Heuristic
    {
        private int edgeAvoidDistance;

        public ElfMoveAwayFromMapEdges(float weight, int edgeExtraAvoidDistance = 0) : base(weight)
        {
            this.edgeAvoidDistance = Constants.Game.PortalSize + edgeExtraAvoidDistance;
        }

        private float GetScore(VirtualGame virtualGame, Location elfLocation)
        {
            int maxRows = Constants.Game.Rows;
            int maxCols = Constants.Game.Cols;

            float score = 0;

            //check top edge
            if (elfLocation.Row <= edgeAvoidDistance)
            {
                score -= edgeAvoidDistance - elfLocation.Row;
            }

            //check bottom edge
            if (elfLocation.Row >= maxRows - edgeAvoidDistance)
            {
                score -= elfLocation.Row - (maxRows - edgeAvoidDistance);
            }

            //check left edge
            if (elfLocation.Col <= edgeAvoidDistance)
            {
                score -= edgeAvoidDistance - elfLocation.Col;
            }

            //check right edge
            if (elfLocation.Col >= maxCols - edgeAvoidDistance)
            {
                score -= elfLocation.Col - (maxCols - edgeAvoidDistance);
            }

            return score;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
            {
                score += GetScore(virtualGame, pair.Value.GetFutureLocation());
            }

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}

/*So you may add a heuristic that keep the elves away from the borders of the game such that the elf will be able to build portal if he wants to.

    Members:
     - edgeAvoidDistance = the distance we should avoid from the map edges

    Scoring:
     For each elf future location:
      we check if the location is too close to the map edges. If it is, we give minus score based on how close it is*/