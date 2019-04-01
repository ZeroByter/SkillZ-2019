using ElfKingdom;

namespace SkillZ.IndividualHeuristics
{
    class ElfAttackEnemyPortalsZBot : Heuristic
    {
        public ElfAttackEnemyPortalsZBot(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (Constants.Game.Turn <= 82) return 0;

            float score = 0;

            foreach (Portal enemyPortal in Constants.GameCaching.GetEnemyPortals())
            {
                score += virtualGame.CountAttacksOnGameObject(enemyPortal);
            }

            return score;
        }
    }
}
