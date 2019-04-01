using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class SaveManaForManaFountains : Heuristic
    {
        public SaveManaForManaFountains(float weight) : base(weight)
        {
            
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (Constants.Game.GetMyMana() < 120 && Constants.GameCaching.GetMyManaFountains().Length == 0)
            {
                int discourageOtherCosts = -1 * (virtualGame.futurePortals.Count + virtualGame.futureSpeedUpSpells.Count + virtualGame.futureInvisibilitySpells.Count);
                int encourageManaFountains = virtualGame.futureManaFountains.Count;
                return discourageOtherCosts + encourageManaFountains;
                //return (discourageOtherCosts + encourageManaFountains) * (3 - Constants.GameCaching.GetMyManaFountains().Length) * (Constants.Game.GetEnemy().ManaPerTurn - Constants.Game.GetMyself().ManaPerTurn);
                //return -1 * Constants.Game.GetMyMana() - virtualGame.mana;
            }

            return 0;
        }
    }
}
