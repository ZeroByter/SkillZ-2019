namespace SkillZ.IndividualHeuristics
{
    class EarlyGameScoreBoost : Heuristic
    {
        private int maxTurn;
        private int maxManaFountains;

        public EarlyGameScoreBoost(float weight, int maxTurn, int maxManaFountains) : base(weight)
        {
            this.maxTurn = maxTurn;
            this.maxManaFountains = maxManaFountains;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (Constants.Game.Turn > maxTurn) return 0;
            if (Constants.GameCaching.GetMyManaFountains().Length > maxManaFountains) return 0;

            return 1;
        }
    }
}

//The original idea was to create ManaFountains very early game, but by accident I forget to `return virtualGame.futureManaFountains.Count` and instead `return 1` for the first 25 turns. Somehow, for some reason this worked really well, so I decided to keep it as is

/*The *(ORIGINAL)* objective of this heuristic is to build mana fountains at the very start of the game to ensure we will have enough
 * Members:
    - maxTurn: The turn number which this heuristic will be active for
    - maxManaFountains: how many mana fountains do we want to have at the start of the game
 * Scoring: Pretty self explantory*/