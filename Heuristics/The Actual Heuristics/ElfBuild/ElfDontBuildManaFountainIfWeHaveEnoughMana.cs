using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfDontBuildManaFountainIfWeHaveEnoughMana : Heuristic
    {
        public ElfDontBuildManaFountainIfWeHaveEnoughMana(float weight) : base(weight)
        {
        }

        private float GetManaRatio()
        {
            return Mathf.Max(0, (Constants.Game.GetMyMana() / Constants.Game.GetEnemyMana()) - 1);
        }

        private float GetManaPerTurnRatio()
        {
            return Mathf.Max(0, (Constants.Game.GetMyself().ManaPerTurn / Constants.Game.GetEnemy().ManaPerTurn) - 1);
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = Mathf.Min(GetManaRatio(), GetManaPerTurnRatio());

            return -1 * score * virtualGame.futureManaFountains.Count;
        }
    }
}