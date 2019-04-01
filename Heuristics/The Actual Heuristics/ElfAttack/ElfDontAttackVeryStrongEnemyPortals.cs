using ElfKingdom;

namespace SkillZ.IndividualHeuristics
{
    class ElfDontAttackVeryStrongEnemyPortals : Heuristic
    {
        public ElfDontAttackVeryStrongEnemyPortals(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (Portal enemyPortal in Constants.GameCaching.GetEnemyPortals())
            {
                if(enemyPortal.CurrentHealth > 8)
                {
                    score -= virtualGame.CountAttacksOnGameObject(enemyPortal);
                }
            }

            return score;
        }
    }
}
