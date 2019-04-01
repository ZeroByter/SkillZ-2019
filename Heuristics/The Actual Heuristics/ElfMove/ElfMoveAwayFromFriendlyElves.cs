using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    /// <summary>
    /// Copied from ElfMoveAwayFromIceTrolls but changed to suit getting away from other elves...
    /// 'Elves are antisocial elves, they don't like being near each other... :('
    /// </summary>
    class ElfMoveAwayFromFriendlyElves : Heuristic
    {
        private int avoidRadius;
        private int untilTurn;

        public ElfMoveAwayFromFriendlyElves(float weight, int avoidRadius, int untilTurn = 0) : base(weight)
        {
            this.avoidRadius = avoidRadius;
            this.untilTurn = untilTurn;
        }

        private float GetElfScore(VirtualGame virtualGame, Elf sourceElf)
        {
            FutureLocation elfFutureLocation = virtualGame.GetFutureLocation(sourceElf);

            float score = 0;

            foreach(KeyValuePair<int, FutureLocation> myOtherElfPair in virtualGame.GetFutureLocations())
            {
                Location myOtherElfFutureLocation = myOtherElfPair.Value.GetFutureLocation();

                float distance = elfFutureLocation.GetFutureLocation().DistanceF(myOtherElfFutureLocation);

                if (myOtherElfPair.Key != sourceElf.UniqueId && distance < avoidRadius)
                {
                    score -= avoidRadius - distance;
                }
            }

            return score;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (untilTurn != 0 && Constants.Game.Turn > untilTurn) return 0;

            float score = 0;

            foreach (Elf myElf in Constants.GameCaching.GetMyLivingElves())
            {
                score += GetElfScore(virtualGame, myElf);
            }

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}

//The goal here is to make it harder for enemy defence to cover our elves.