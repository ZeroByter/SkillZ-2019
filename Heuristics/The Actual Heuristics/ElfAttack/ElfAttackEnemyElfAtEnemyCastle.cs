using ElfKingdom;

namespace SkillZ.IndividualHeuristics
{
    class ElfAttackEnemyElfAtEnemyCastle : Heuristic
    {
        private float proximityToCastle;

        public ElfAttackEnemyElfAtEnemyCastle(float weight, float proximityToCastle) : base(weight)
        {
            this.proximityToCastle = proximityToCastle;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            Castle enemyCastle = Constants.Game.GetEnemyCastle();

            foreach (Elf enemyElf in virtualGame.GetEnemyLivingElves())
            {
                if (enemyElf.Distance(enemyCastle) <= proximityToCastle)
                {
                    score += virtualGame.CountAttacksOnGameObject(enemyElf);
                }
            }

            return score;
        }
    }
}
