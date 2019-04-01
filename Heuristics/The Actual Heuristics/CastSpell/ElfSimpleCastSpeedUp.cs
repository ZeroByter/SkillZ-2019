using ElfKingdom;

namespace SkillZ.IndividualHeuristics
{
    class ElfSimpleCastSpeedUp : Heuristic
    {
        public ElfSimpleCastSpeedUp(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            //if (Constants.GameCaching.GetMyLivingElves().Length <= 1) return -1 * virtualGame.futureSpeedUpSpells.Count; //tried this, this is worse.
            if (Constants.GameCaching.GetEnemyLivingElves().Length == 0 &&
                Constants.GameCaching.GetEnemyIceTrolls().Length == 0)
            {
                return -1 * virtualGame.futureSpeedUpSpells.Count;
            }
            if (virtualGame.mana < Constants.Game.PortalCost + Constants.Game.ManaFountainCost) return -1 * virtualGame.futureSpeedUpSpells.Count;

            float score = 0;
            foreach (VirtualSpeedUp virtualSpeedUp in virtualGame.futureSpeedUpSpells.Values)
            {
                Circle circle = new Circle(virtualSpeedUp.location, 9 * Constants.Game.ElfMaxSpeed);
                if (Constants.GameCaching.GetEnemyPortalsInArea(circle).Count > 0)
                {
                    continue;
                }
                if (Constants.GameCaching.GetEnemyManaFountainsInArea(circle).Count > 0)
                {
                    continue;
                }
                score++;
            }
            return score;
        }
    }
}
