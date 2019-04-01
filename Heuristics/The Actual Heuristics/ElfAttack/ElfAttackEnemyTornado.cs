using ElfKingdom;

namespace SkillZ.IndividualHeuristics
{
    class ElfAttackEnemyTornado : Heuristic
    {
        public ElfAttackEnemyTornado(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (Tornado tornado in Constants.GameCaching.GetEnemyTornadoes())
            {
                score += virtualGame.CountAttacksOnGameObject(tornado);
            }

            return score;
        }
    }
}
