using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ
{
    class TrackPortalCreations
    {
        private static Dictionary<int, int> LavaGiantCounts = new Dictionary<int, int>();
        private static Dictionary<int, int> IceTrollCounts = new Dictionary<int, int>();

        public static Portal IsPortalInLocation(MapObject mapObject)
        {
            foreach(Portal portal in Constants.GameCaching.GetEnemyPortals()/*InArea(new Circle(mapObject, Constants.Game.PortalSize))*/)
            {
                if (portal.GetLocation() == mapObject.GetLocation()) return portal;
            }

            return null;
        }

        public static void UpdateEnemyPortalCreations()
        {
            foreach(LavaGiant lavaGiant in Constants.GameCaching.GetEnemyLavaGiants())
            {
                Portal portal = IsPortalInLocation(lavaGiant);

                if (portal != null)
                {
                    int count = 0;

                    LavaGiantCounts.TryGetValue(portal.UniqueId, out count);

                    LavaGiantCounts[portal.UniqueId] = count + 1;
                }
            }

            foreach (IceTroll iceTroll in Constants.GameCaching.GetEnemyIceTrolls())
            {
                Portal portal = IsPortalInLocation(iceTroll);

                if (portal != null)
                {
                    int count = 0;

                    if (IceTrollCounts.TryGetValue(portal.UniqueId, out count))
                    {
                        count++;
                    }

                    IceTrollCounts[portal.UniqueId] = count;
                }
            }
        }

        public static int GetPortalLavaGiantsCount(Portal portal)
        {
            int count = 0;

            LavaGiantCounts.TryGetValue(portal.UniqueId, out count);

            return count;
        }

        public static int GetPortalIceTrollCount(Portal portal)
        {
            int count = 0;

            IceTrollCounts.TryGetValue(portal.UniqueId, out count);

            return count;
        }
    }
}
