using ElfKingdom;
using System.Collections.Generic;
using System.Linq;

namespace SkillZ
{
    public class MyBot : ISkillzBot
    {
        private List<int> turnTimes;

        private static void Main() { }

        public void DoTurn(Game game)
        {
            Constants.Game = game; //setup constant game value
            Constants.GameCaching = new GameCaching();

            Logger.EnableDebug(Constants.EnableDebug);

            Bots.BotFactory.GetInstance().PreDoTurn(game);
            Bots.BotFactory.GetInstance().DoTurn(game);
            Bots.BotFactory.GetInstance().PostDoTurn(game);

            Logger.PrintAllTheWarningAndErrors();

            if (Constants.EnableDebug)
            {
                if (game.Turn == 1 && turnTimes == null) turnTimes = new List<int>();

                int timeToCalculateTurn = game.GetMaxTurnTime() - game.GetTimeRemaining();

                turnTimes.Add(timeToCalculateTurn);

                int maxTime = game.GetMaxTurnTime();

                int max = turnTimes.Max();
                int maxIndex = turnTimes.IndexOf(max);
                int min = turnTimes.Min();
                int minIndex = turnTimes.IndexOf(min);
                double aver = turnTimes.Average();

                int current = game.GetMaxTurnTime() - game.GetTimeRemaining();

                Logger.Info($"=== Turn Times Data - Max Time = {maxTime} ===");
                Logger.Info($"Max turn time so far: {max}ms ({(float)max / (float)maxTime * 100f}% of max time) happened at turn #{maxIndex}");
                Logger.Info($"Min turn time so far: {min}ms ({(float)min / (float)maxTime * 100f}% of max time) happened at turn #{minIndex}");
                Logger.Info($"Average turn time so far: {aver}ms ({(float)aver / (float)maxTime * 100f}% of max time)");
                Logger.Info($"Time it took to calculate current turn: {current}ms ({(float)current / (float)maxTime * 100f}% of max time)");
            }
        }
    }
}
