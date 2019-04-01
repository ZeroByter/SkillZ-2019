namespace SkillZ.IndividualHeuristics
{
    class ElfAttackEnemyCastle : Heuristic
    {
        public ElfAttackEnemyCastle(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            return virtualGame.CountAttacksOnGameObject(Constants.Game.GetEnemyCastle());
        }
    }
}
