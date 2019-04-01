namespace SkillZ.IndividualHeuristics
{
    class ElfAttackEnemyCastleLabyrinth : Heuristic
    {
        public ElfAttackEnemyCastleLabyrinth(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            return virtualGame.CountAttacksOnGameObject(Constants.Game.GetEnemyCastle());
        }
    }
}
