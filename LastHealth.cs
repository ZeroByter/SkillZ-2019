using ElfKingdom;
using System.Collections.Generic;
using System.Linq;

namespace SkillZ
{
    public static class LastHealth
    {
        public static Dictionary<int, int> LastHealths = new Dictionary<int, int>();
        
        /// <summary>
        /// Call this before ships make their decisions
        /// </summary>
        public static void UpdateLastHealths()
        {
            LastHealths = new Dictionary<int, int>();

            if (Constants.Game == null) return;

            foreach (GameObject gameObject in Constants.GameCaching.GetAllGameObjects().Values)
            {
                LastHealths[gameObject.UniqueId] = gameObject.CurrentHealth;
            }
        }

        /// <summary>
        /// Returns the health difference from right now and last turn
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static int HealthDifference(GameObject gameObject)
        {
            if (!LastHealths.ContainsKey(gameObject.UniqueId))
            {
                if(gameObject is Creature)
                {
                    return ((Creature)gameObject).GetSuffocation();
                }
                else
                {
                    return 0;
                }
            }

            return gameObject.CurrentHealth - LastHealths[gameObject.UniqueId];
        }
    }
}
