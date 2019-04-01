using ElfKingdom;

namespace SkillZ.IndividualHeuristics
{
    class ElfDontCastSpeedUpIfLowMana : Heuristic
    {
        private int lowMana;

        public ElfDontCastSpeedUpIfLowMana(float weight, int lowMana) : base(weight)
        {
            this.lowMana = lowMana;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (Constants.Game.GetMyMana() == virtualGame.mana) return 0;

            if (Constants.Game.GetMyMana() > lowMana) return 0;

            return -1 * virtualGame.futureSpeedUpSpells.Count;
        }
    }
}
