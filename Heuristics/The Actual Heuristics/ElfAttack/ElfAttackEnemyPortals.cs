using ElfKingdom;

namespace SkillZ.IndividualHeuristics
{
    class ElfAttackEnemyPortals: Heuristic
    {
        public ElfAttackEnemyPortals(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (Portal enemyPortal in Constants.GameCaching.GetEnemyPortals())
            {
                score += virtualGame.CountAttacksOnGameObject(enemyPortal);
            }

            return score;
        }
    }
}
