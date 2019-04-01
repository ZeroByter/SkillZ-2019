using ElfKingdom;

namespace SkillZ.IndividualHeuristics
{
    class ElfAttackEnemyElfZBot : Heuristic
    {
        public ElfAttackEnemyElfZBot(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (Constants.Game.Turn < 127) return 0;
            if (Constants.Game.Turn > 137) return 0;

            float score = 0;

            foreach (Elf enemyElf in virtualGame.GetEnemyLivingElves())
            {
                score += virtualGame.CountAttacksOnGameObject(enemyElf);
            }

            return score;
        }
    }
}
