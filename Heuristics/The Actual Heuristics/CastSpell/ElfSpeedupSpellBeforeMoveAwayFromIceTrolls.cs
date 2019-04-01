namespace SkillZ.IndividualHeuristics
{
    class ElfSpeedupSpellBeforeMoveAwayFromIceTrolls : Heuristic
    {
        public ElfSpeedupSpellBeforeMoveAwayFromIceTrolls(float weight) : base(weight)
        {

        }

        private float GetElfScore(VirtualGame virtualGame, VirtualSpeedUp virtualSpeedUp)
        {
            return Constants.GameCaching.GetEnemyIceTrollsInArea(new Circle(virtualSpeedUp.location, Constants.Game.IceTrollAttackRange)).Count;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            //should have a getter in VirtualGame to futureInvisibilitySpells
            foreach (VirtualSpeedUp virtualSpeedUp in virtualGame.futureSpeedUpSpells.Values)
            {
                score += GetElfScore(virtualGame, virtualSpeedUp);
            }

            return score;
        }
    }
}

