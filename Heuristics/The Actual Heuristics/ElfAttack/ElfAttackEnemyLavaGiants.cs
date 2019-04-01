using ElfKingdom;

namespace SkillZ.IndividualHeuristics
{
    class ElfAttackEnemyLavaGiants : Heuristic
    {
        public ElfAttackEnemyLavaGiants(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (LavaGiant enemyLavaGiants in Constants.GameCaching.GetEnemyLavaGiants())
            {
                score += virtualGame.CountAttacksOnGameObject(enemyLavaGiants);
            }

            return score;
        }
    }
}
