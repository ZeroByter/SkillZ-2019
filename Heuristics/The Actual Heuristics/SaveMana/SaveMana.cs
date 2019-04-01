using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class SaveMana : Heuristic
    {
        private float minCost;
        private float maxMana;

        public SaveMana(float weight) : base(weight)
        {
            minCost = Mathf.Min(Constants.Game.IceTrollCost, Constants.Game.LavaGiantCost);
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if(Constants.Game.GetMyMana() == virtualGame.mana) return 0;

            maxMana = Mathf.Min(minCost * 10, Constants.Game.GetEnemyMana());

            return -1 * (maxMana + minCost) / (virtualGame.mana + minCost);
        }
    }
}
