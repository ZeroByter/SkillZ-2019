using ElfKingdom;

namespace SkillZ.IndividualHeuristics
{
    class ElfAttackEnemyElf : Heuristic
    {
        public ElfAttackEnemyElf(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (Elf enemyElf in virtualGame.GetEnemyLivingElves())
            {
                score += virtualGame.CountAttacksOnGameObject(enemyElf);
            }

            return score;
        }
    }
}
