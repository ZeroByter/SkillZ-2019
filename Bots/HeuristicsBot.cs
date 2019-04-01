using ElfKingdom;

namespace SkillZ.Bots
{
    abstract class HeuristicsBot : Bot
    {
        public HeuristicsBot()
        {
            SetupHeuristics();
        }

        public abstract void SetupHeuristics();

        public override void PreDoTurn(Game game)
        {
            base.PreDoTurn(game);
            Heuristics.PreDoTurn(game);
        }

        public override void DoTurn(Game game)
        {
            DoNextTurnHeuristics.RunNextTurn(new GameNextTurnActions());
        }
    }
}
