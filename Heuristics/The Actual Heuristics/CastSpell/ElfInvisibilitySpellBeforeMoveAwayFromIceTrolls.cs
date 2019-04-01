namespace SkillZ.IndividualHeuristics
{
    class ElfInvisibilitySpellBeforeMoveAwayFromIceTrolls : Heuristic
    {
        public ElfInvisibilitySpellBeforeMoveAwayFromIceTrolls(float weight) : base(weight)
        {
            
        }

        private float GetElfScore(VirtualGame virtualGame, VirtualInvisibility virtualInvisibility)
        {
            return Constants.GameCaching.GetEnemyIceTrollsInArea(new Circle(virtualInvisibility.location, Constants.Game.IceTrollAttackRange)).Count;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            //should have a getter in VirtualGame to futureInvisibilitySpells
            foreach (VirtualInvisibility virtualInvisibility in virtualGame.futureInvisibilitySpells.Values)
            {
                score += GetElfScore(virtualGame, virtualInvisibility);
            }

            return score;
        }
    }
}

