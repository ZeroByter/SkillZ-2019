using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class SaveManaForTornadoes : Heuristic
    {
        public SaveManaForTornadoes(float weight) : base(weight)
        {
            
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (virtualGame.mana == Constants.Game.GetMyMana()) return 0;
            if (Constants.GameCaching.GetEnemyPortals().Length == 0 && Constants.GameCaching.GetEnemyManaFountains().Length == 0) return 0;

            if (Constants.Game.GetMyMana() < Constants.Game.TornadoCost && Constants.GameCaching.GetMyTornadoes().Length == 0)
            {
                int discourageOtherCosts = -1 * (virtualGame.futurePortals.Count + virtualGame.futureSpeedUpSpells.Count + virtualGame.futureInvisibilitySpells.Count + virtualGame.futureLavaGiants.Count);
                return discourageOtherCosts;
            }

            return 0;
        }
    }
}
