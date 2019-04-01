using ElfKingdom;
using System.Collections.Generic;
using System.Linq;

namespace SkillZ
{
    public static class PlayerExtensions
    {
        public static List<Player> GetEnemies(this Player player)
        {
            List<Player> enemies = new List<Player>();

            foreach (Player enemy in Constants.Game.GetAllPlayers())
            {
                if (enemy != player) enemies.Add(enemy);
            }

            return enemies;
        }

        public static Player GetEnemy(this Player player)
        {
            foreach (Player enemy in Constants.Game.GetAllPlayers())
            {
                if (enemy != player) return enemy;
            }

            return null;
        }

        public static Castle GetCastle(this Player player)
        {
            foreach(Castle castle in Constants.Game.GetAllCastles())
            {
                if (castle.Owner == player) return castle;
            }

            return null;
        }

        public static List<Elf> GetAllElves(this Player player)
        {
            List<Elf> elves = new List<Elf>();

            foreach (Elf elf in Constants.Game.GetAllElves())
            {
                if (elf.Owner == player) elves.Add(elf);
            }

            return elves;
        }

        public static List<Portal> GetAllPortals(this Player player)
        {
            List<Portal> portals = new List<Portal>();

            foreach(Portal portal in Constants.Game.GetAllPortals())
            {
                if (portal.Owner == player) portals.Add(portal);
            }

            return portals;
        }

        public static Elf[] GetLivingElves(this Player player)
        {
            if (Constants.Game.GetMyself() == player)
            {
                return Constants.GameCaching.GetMyLivingElves();
            }
            else
            {
                return Constants.GameCaching.GetEnemyLivingElves();
            }
        }

        public static LavaGiant[] GetLavaGiants(this Player player)
        {
            if (Constants.Game.GetMyself() == player)
            {
                return Constants.GameCaching.GetMyLavaGiants();
            }
            else
            {
                return Constants.GameCaching.GetEnemyLavaGiants();
            }
        }

        public static IceTroll[] GetIceTrolls(this Player player)
        {
            if (Constants.Game.GetMyself() == player)
            {
                return Constants.GameCaching.GetMyIceTrolls();
            }
            else
            {
                return Constants.GameCaching.GetEnemyIceTrolls();
            }
        }

        public static List<GameObject> GetAllGameObjects(this Player player)
        {
            List<GameObject> objects = new List<GameObject>();

            objects.AddRange(player.LivingElves);
            objects.AddRange(player.Creatures);
            objects.AddRange(player.GetAllPortals());
            objects.Add(player.GetCastle());

            return objects;
        }

        /// <summary>
        /// Returns all gameObjects of the player which can cause damage to 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static List<GameObject> GetCustomGameObjects(this Player player, System.Predicate<GameObject> shouldGetGameObject)
        {
            List<GameObject> gameObjects = new List<GameObject>();

            foreach(GameObject gameObject in player.GetAllGameObjects())
            {
                if (shouldGetGameObject.Invoke(gameObject)) gameObjects.Add(gameObject);
            }

            return gameObjects;
        }
    }
}
