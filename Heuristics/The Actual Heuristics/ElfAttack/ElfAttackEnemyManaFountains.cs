using ElfKingdom;

namespace SkillZ.IndividualHeuristics
{
    class ElfAttackEnemyManaFountains : Heuristic
    {
        public ElfAttackEnemyManaFountains(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (ManaFountain enemyManaFountain in Constants.GameCaching.GetEnemyManaFountains())
            {
                score += virtualGame.CountAttacksOnGameObject(enemyManaFountain);
            }

            return score;
        }
    }
}
