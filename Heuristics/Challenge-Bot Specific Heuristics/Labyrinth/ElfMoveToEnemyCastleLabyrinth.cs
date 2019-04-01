using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveToEnemyCastleLabyrinth : Heuristic
    {
        public ElfMoveToEnemyCastleLabyrinth(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            Castle enemyCastle = Constants.Game.GetEnemyCastle();

            foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
            {
                Location elfNextLocation = pair.Value.GetFutureLocation();

                float distance = elfNextLocation.Distance(enemyCastle);

                score -= distance / Constants.Game.ElfMaxSpeed;
            }

            return score;
        }
    }
}
