using ElfKingdom;
using System.Collections.Generic;
using System.Linq;

namespace SkillZ
{
    public static class GameExtensions
    {
        //Couldn't place this into GameCaching since we also need maxTurnsToRevive
        public static List<Elf> GetAllDeadButAlmostRevivedElvesInArea(this Game game, Circle area, int maxTurnsToRevive)
        {
            List<Elf> list = new List<Elf>();

            foreach(Elf enemyElf in Constants.GameCaching.GetAllEnemyElves())
            {
                if(!enemyElf.IsAlive() && area.IsLocationInside(enemyElf.InitialLocation) && enemyElf.TurnsToRevive <= maxTurnsToRevive)
                {
                    list.Add(enemyElf);
                }
            }

            return list;
        }

        public static Building GetBuildingAtLocation(this Game game, MapObject location, int ourSize = 0)
        {
            foreach (Building building in game.GetAllBuildings())
            {
                if (building.InRange(location, building.Size + ourSize)) return building;
            }

            return null;
        }

        /// <summary>
        /// Returns how many defensive portals we have. A defensive portal is any portal of ours on our side of the map
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static List<Portal> GetDefensivePortals(this Game game)
        {
            List<Portal> portals = new List<Portal>();

            foreach (Portal portal in game.GetMyPortals())
            {
                if (portal.GetLocation().OnSameSideAsCastle()) portals.Add(portal);
            }

            return portals;
        }

        /// <summary>
        /// Returns how many offensive portals we have. An offensive portal is any portal of ours on the OTHER our side of the map
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static List<Portal> GetOffensivePortals(this Game game)
        {
            List<Portal> portals = new List<Portal>();

            foreach (Portal portal in game.GetMyPortals())
            {
                if (!portal.GetLocation().OnSameSideAsCastle()) portals.Add(portal);
            }

            return portals;
        }
    }
}
