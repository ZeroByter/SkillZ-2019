using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfSimpleCastInvisibilityTrapBot : Heuristic
    {
        public ElfSimpleCastInvisibilityTrapBot(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if(Constants.Game.GetMyLivingElves().Length == 0)
            {
                return -1;
            }

            IceTroll closestIceTroll = (IceTroll)Utilities.GetClosest(Constants.GameCaching.GetMyLivingElves()[0], Constants.GameCaching.GetEnemyIceTrolls());
            int disFromClosestIceTroll = virtualGame.GetFutureLocation(Constants.GameCaching.GetMyLivingElves()[0]).GetFutureLocation().Distance(closestIceTroll.GetLocation());

            if (Constants.Game.Turn >= 4 && disFromClosestIceTroll <= Constants.Game.IceTrollAttackRange)
            {
                return virtualGame.futureInvisibilitySpells.Count;
            }

            return -1;
        }
    }
}