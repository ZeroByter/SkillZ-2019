using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ
{
    static class Heuristics
    {
        private static List<Heuristic> HeuristicsList = new List<Heuristic>();

        /// <summary>
        /// !!!
        /// Only call this when setting up specific challenge bots!
        /// Only call this from 'turn #1'!!!
        /// You really shouldn't call this from anywhere else!!!!
        /// Or Rom will be angry >:(
        /// !!!
        /// </summary>
        /// <param name="heuristic"></param>
        public static void AddHeuristic(Heuristic heuristic)
        {
            if (Constants.Game.Turn != 1) return; //Only from turn 1!!

            HeuristicsList.Add(heuristic);
        }

        public static void PreDoTurn(Game game)
        {
            foreach(Heuristic heuristic in HeuristicsList)
            {
                heuristic.UpdateState(game);
            }
        }

        public static float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (Heuristic heuristic in HeuristicsList)
            {
                float tempScore = heuristic.GetWeightedScore(virtualGame);
                if (float.IsNaN(tempScore))
                {
                    tempScore = 0;
                }
                score += tempScore;
            }

            return score;
        }

        public static float GetScoreDebug(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (Heuristic heuristic in HeuristicsList)
            {
                float tempScore = heuristic.GetWeightedScore(virtualGame);
                if (float.IsNaN(tempScore))
                {
                    tempScore = 0;
                }
                score += tempScore;

                Logger.Debug("{0} = {1}", tempScore, heuristic);
            }

            return score;
        }
    }
}
