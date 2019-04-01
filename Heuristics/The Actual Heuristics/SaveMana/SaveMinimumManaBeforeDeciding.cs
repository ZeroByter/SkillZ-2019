namespace SkillZ.IndividualHeuristics
{
    class SaveMinimumManaBeforeDeciding : Heuristic
    {
        private int MinimumMana;
        public SaveMinimumManaBeforeDeciding(float weight, int minimumMana) : base(weight)
        {
            this.MinimumMana = minimumMana;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (virtualGame.mana == Constants.Game.GetMyMana()) return 0;
            return (Constants.Game.GetMyMana() < MinimumMana) ? -1f : 0f;
        }
    }
}
