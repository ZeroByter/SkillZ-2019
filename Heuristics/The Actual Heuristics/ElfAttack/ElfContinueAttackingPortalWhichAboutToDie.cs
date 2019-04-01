using ElfKingdom;

namespace SkillZ.IndividualHeuristics
{
    class ElfContinueAttackingPortalWhichAboutToDie : Heuristic
    {
        private int portalMaxHealth;
        public ElfContinueAttackingPortalWhichAboutToDie(float weight, int portalMaxHealth) : base(weight)
        {
            this.portalMaxHealth = portalMaxHealth;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (Portal enemyPortal in Constants.GameCaching.GetEnemyPortals())
            {
                if (enemyPortal.CurrentHealth <= portalMaxHealth)
                {
                    score += virtualGame.CountAttacksOnGameObject(enemyPortal);
                }
            }

            return score;
        }
    }
}
