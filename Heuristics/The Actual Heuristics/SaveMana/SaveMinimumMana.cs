
namespace SkillZ.IndividualHeuristics
{
    class SaveMinimumMana : Heuristic
    {
        int MinimumMana;
        public SaveMinimumMana(float weight, int minimumMana) : base(weight)
        {
            this.MinimumMana = minimumMana;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            return (virtualGame.mana < MinimumMana) ? -1f : 0f;
        }
    }
}
