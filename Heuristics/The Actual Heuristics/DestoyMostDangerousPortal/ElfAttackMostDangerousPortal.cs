using ElfKingdom;

namespace SkillZ.IndividualHeuristics
{
    class ElfAttackMostDangerousPortal : Heuristic
    {
        public ElfAttackMostDangerousPortal(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            float mostDangerousPortalScore = -1;
            Portal mostDangerousPortal = null;
            float currentPortalScore = 0;

            foreach (Portal enemyPortal in Constants.GameCaching.GetEnemyPortals())
            {
                currentPortalScore = Constants.Game.LavaGiantAttackMultiplier * (Constants.Game.LavaGiantMaxHealth - Constants.Game.LavaGiantSuffocationPerTurn * (enemyPortal.Distance(Constants.Game.GetMyCastle()) / Constants.Game.LavaGiantMaxSpeed));// + enemyPortal.CurrentlySummoning;
                if (mostDangerousPortalScore > currentPortalScore)
                {
                    mostDangerousPortalScore = currentPortalScore;
                    mostDangerousPortal = enemyPortal;
                }
            }

            if(mostDangerousPortal != null)
                score = virtualGame.CountAttacksOnGameObject(mostDangerousPortal);

            return score;
        }
    }
}
