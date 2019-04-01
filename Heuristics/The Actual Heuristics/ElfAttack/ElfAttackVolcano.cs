namespace SkillZ.IndividualHeuristics
{
    class ElfAttackVolcano : Heuristic
    {
        public ElfAttackVolcano(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            return virtualGame.CountAttacksOnGameObject(Constants.Game.GetVolcano());
        }
    }
}
