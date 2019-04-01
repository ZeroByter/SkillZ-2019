using ElfKingdom;
using System.Collections.Generic;
using System.Linq;

namespace SkillZ
{
    /// <summary>
    /// A class meant to 'simulate' a virtual game state
    /// </summary>
    public class VirtualGame
    {
        /// <summary>
        /// How much mana do we have?
        /// </summary>
        public int mana;

        #region Storing enemies' GameObject states
        /// <summary>
        /// A list of all living enemyGameObjects
        /// </summary>
        public Dictionary<int, VirtualGameObject> livingEnemyGameObjects = new Dictionary<int, VirtualGameObject>();
        /// <summary>
        /// Once we attack something, here we can keep a count of how many times any of our GameObjects attacked it
        /// </summary>
        public Dictionary<GameObject, List<GameObject>> attackedToAttackersList = new Dictionary<GameObject, List<GameObject>>();
        #endregion

        //Predicting our soon-to-be created GameObjects
        public Dictionary<int, VirtualPortal> futurePortals = new Dictionary<int, VirtualPortal>();
        public Dictionary<int, VirtualManaFountain> futureManaFountains = new Dictionary<int, VirtualManaFountain>();
        public Dictionary<int, VirtualLavaGiant> futureLavaGiants = new Dictionary<int, VirtualLavaGiant>();
        public Dictionary<int, VirtualIceTroll> futureIceTrolls = new Dictionary<int, VirtualIceTroll>();
        public Dictionary<int, VirtualTornado> futureTornadoes = new Dictionary<int, VirtualTornado>();
        public Dictionary<int, VirtualInvisibility> futureInvisibilitySpells = new Dictionary<int, VirtualInvisibility>();
        public Dictionary<int, VirtualSpeedUp> futureSpeedUpSpells = new Dictionary<int, VirtualSpeedUp>();

        private Dictionary<int, FutureLocation> futureLocations = new Dictionary<int, FutureLocation>();

        public VirtualGame()
        {
            mana = Constants.Game.GetMyMana();

            foreach (GameObject enemyGameObject in Constants.GameCaching.GetAllEnemyGameObjects())
            {
                livingEnemyGameObjects.Add(enemyGameObject.UniqueId, new VirtualGameObject(enemyGameObject));
            }
        }

        #region Attack combat stuff
        public void DoAttackDamage(GameObject attacker, GameObject attacked)
        {
            List<GameObject> attackersList;

            if (!attackedToAttackersList.TryGetValue(attacked, out attackersList))
            {
                attackersList = new List<GameObject>();
            }

            attackersList.Add(attacker);

            attackedToAttackersList[attacked] = attackersList;

            livingEnemyGameObjects[attacked.UniqueId].health -= attacker.GetAttackMultiplier();
        }

        public void RevertAttackDamage(GameObject attacker, GameObject attacked)
        {
            List<GameObject> attackersList = attackedToAttackersList[attacked];
            attackersList.Remove(attacker);
            if (attackersList.Count == 0)
            {
                attackedToAttackersList.Remove(attacked);
            }

            livingEnemyGameObjects[attacked.UniqueId].health += attacker.GetAttackMultiplier();
        }

        public int GetCombinedDamage(GameObject attacked)
        {
            int damage = 0;

            foreach (GameObject attacker in attackedToAttackersList[attacked])
            {
                damage += attacker.GetAttackMultiplier();
            }

            return damage;
        }

        public int CountAttacksOnGameObject(GameObject attacked)
        {
            if (!attackedToAttackersList.ContainsKey(attacked))
            {
                return 0;
            }
            else
            {
                return attackedToAttackersList[attacked].Count;
            }
        }

        public List<GameObject> GetAllFutureAttacks()
        {
            List<GameObject> attacks = new List<GameObject>();

            foreach(var attackedPair in attackedToAttackersList)
            {
                foreach(var attacker in attackedPair.Value)
                {
                    attacks.Add(attacker);
                }
            }

            return attacks;
        }

        #endregion
        #region Getting enemy living elves and enemy GameObjects from VirtualGame
        public List<Elf> GetEnemyLivingElves()
        {
            List<Elf> elves = new List<Elf>();
            foreach (VirtualGameObject enemyGameObject in livingEnemyGameObjects.Values)
            {
                if (enemyGameObject.realGameObject is Elf)
                {
                    elves.Add((Elf)enemyGameObject.realGameObject);
                }
            }

            return elves;
        }

        public List<LavaGiant> GetEnemyLavaGiants()
        {
            List<LavaGiant> lavaGiants = new List<LavaGiant>();

            foreach (VirtualGameObject enemyGameObject in livingEnemyGameObjects.Values)
            {
                if (enemyGameObject.realGameObject is LavaGiant)
                {
                    lavaGiants.Add((LavaGiant)enemyGameObject.realGameObject);
                }
            }

            return lavaGiants;
        }

        public List<Portal> GetEnemyPortals()
        {
            List<Portal> elves = new List<Portal>();

            foreach (VirtualGameObject enemyGameObject in livingEnemyGameObjects.Values)
            {
                if (enemyGameObject.realGameObject is Portal)
                {
                    elves.Add((Portal)enemyGameObject.realGameObject);
                }
            }

            return elves;
        }

        public GameObject GetEnemyGameObject(int uniqueId)
        {
            foreach (KeyValuePair<int, VirtualGameObject> pair in livingEnemyGameObjects)
            {
                if (pair.Key == uniqueId) return pair.Value.realGameObject;
            }

            return null;
        }
        #endregion
        
        #region Future GameObjects
        public int CountFuturePortalsInArea(Location location, float radius)
        {
            int count = 0;

            foreach (VirtualPortal futurePortal in futurePortals.Values)
            {
                if (futurePortal.location.InRange(location, Mathf.RoundToInt(radius)))
                {
                    count++;
                }
            }

            return count;
        }

        public int CountFuturePortalsInArea(Circle area)
        {
            return CountFuturePortalsInArea(area.GetCenter(), area.GetRadius());
        }

        public int CountFutureManaFountainsInArea(Location location, float radius)
        {
            int count = 0;

            foreach (VirtualManaFountain futureManaFountain in futureManaFountains.Values)
            {
                if (futureManaFountain.location.InRange(location, Mathf.RoundToInt(radius)))
                {
                    count++;
                }
            }

            return count;
        }

        private int CountAllElvesBuildingPortalsInArea(Location location, float radius)
        {
            int count = 0;

            foreach (Elf elf in Constants.GameCaching.GetMyLivingElves())
            {
                if (elf.InRange(location, Mathf.RoundToInt(radius)))
                {
                    if (elf.IsBuilding && elf.CurrentlyBuilding == "Portal")
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public int CountAllPortalsInArea(Location location, float radius)
        {
            int futurePortals = CountFuturePortalsInArea(location, radius);
            int currentlyBuildingPortals = CountAllElvesBuildingPortalsInArea(location, radius);
            
            return futurePortals + currentlyBuildingPortals + Constants.GameCaching.GetMyPortalsInArea(new Circle(location, radius)).Count;
        }

        /// <summary>
        /// Gets a future virtual portal inside a location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private VirtualPortal GetFuturePortalInLocation(Location location)
        {
            int portalSize = Constants.Game.PortalSize;
            foreach(VirtualPortal virtualGameObject in futurePortals.Values)
            {
                if (virtualGameObject.location.InRange(location, portalSize * 2))
                {
                    return (VirtualPortal)virtualGameObject;
                }
            }

            return null;
        }

        private VirtualManaFountain GetFutureManaFountainInLocation(Location location)
        {
            int manaFountainSize = Constants.Game.ManaFountainSize;
            foreach (VirtualManaFountain virtualGameObject in futureManaFountains.Values)
            {
                if (virtualGameObject.location.InRange(location, manaFountainSize * 2))
                {
                    return (VirtualManaFountain)virtualGameObject;
                }
            }

            return null;
        }

        public bool AddFuturePortal(Elf creator)
        {
            if (mana >= Constants.Game.PortalCost && GetFuturePortalInLocation(creator.GetLocation()) == null
            && GetFutureManaFountainInLocation(creator.GetLocation()) == null)
            {
                futurePortals.Add(creator.UniqueId, new VirtualPortal(Constants.Game.GetMyself(), creator.GetLocation(), Constants.Game.PortalMaxHealth, creator));
                mana -= Constants.Game.PortalCost;

                return true;
            }

            return false;
        }

        public bool AddFutureManaFountain(Elf creator)
        {
            if (mana >= Constants.Game.ManaFountainCost && GetFutureManaFountainInLocation(creator.GetLocation()) == null
				&& GetFuturePortalInLocation(creator.GetLocation()) == null)
            {
                futureManaFountains.Add(creator.UniqueId, new VirtualManaFountain(Constants.Game.GetMyself(), creator.GetLocation(), Constants.Game.ManaFountainMaxHealth, creator));
                mana -= Constants.Game.ManaFountainCost;

                return true;
            }

            return false;
        }

        public bool AddFutureIceTroll(Portal creator)
        {
            if(mana >= Constants.Game.IceTrollCost)
            {
                futureIceTrolls.Add(creator.UniqueId, new VirtualIceTroll(Constants.Game.GetMyself(), creator.GetLocation(), Constants.Game.IceTrollMaxHealth, creator));
                mana -= Constants.Game.IceTrollCost;

                return true;
            }

            return false;
        }

        public bool AddFutureLavaGiant(Portal creator)
        {
            if (mana >= Constants.Game.LavaGiantCost)
            {
                futureLavaGiants.Add(creator.UniqueId, new VirtualLavaGiant(Constants.Game.GetMyself(), creator.GetLocation(), Constants.Game.LavaGiantMaxHealth, creator));
                mana -= Constants.Game.LavaGiantCost;

                return true;
            }

            return false;
        }

        public bool AddFutureTornado(Portal creator)
        {
            if (mana >= Constants.Game.TornadoCost)
            {
                futureTornadoes.Add(creator.UniqueId, new VirtualTornado(Constants.Game.GetMyself(), creator.GetLocation(), Constants.Game.TornadoMaxHealth, creator));
                mana -= Constants.Game.TornadoCost;

                return true;
            }

            return false;
        }

        public void RemoveFuturePortal(Elf creator)
        {
            futurePortals.Remove(creator.UniqueId);
            mana += Constants.Game.PortalCost;
        }

        public void RemoveFutureManaFountain(Elf creator)
        {
            futureManaFountains.Remove(creator.UniqueId);
            mana += Constants.Game.ManaFountainCost;
        }

        public void RemoveFutureIceTroll(Portal creator)
        {
            futureIceTrolls.Remove(creator.UniqueId);
            mana += Constants.Game.IceTrollCost;
        }

        public void RemoveFutureLavaGiant(Portal creator)
        {
            futureLavaGiants.Remove(creator.UniqueId);
            mana += Constants.Game.LavaGiantCost;
        }

        public void RemoveFutureTornado(Portal creator)
        {
            futureTornadoes.Remove(creator.UniqueId);
            mana += Constants.Game.TornadoCost;
        }

        public int GetCombinedDamageToEnemyCastle()
        {
            int combinedDamageOutputToCastle = 0;

            //looping through all future plans to create LavaGiants
            foreach (VirtualLavaGiant virtualLavaGiant in futureLavaGiants.Values)
            {
                //we want to calculate if we can kill the enemy castle right now...
                //all this doesnt of course take into account enemy icetrolls or elves

                combinedDamageOutputToCastle += virtualLavaGiant.PredictedDamageDoneToTarget(Constants.Game.GetEnemyCastle());
            }

            //looping through all portals that are summoning stuff right now
            foreach (Portal portal in Constants.GameCaching.GetMyCurrentlySummoningLavaGiantsPortals())
            {
                combinedDamageOutputToCastle += portal.GetLocation().PredictLavaGiantDamageDoneToCastleIfNotHitByEnemy(Constants.Game.GetEnemyCastle());
            }

            //looping through all current LavaGiants
            foreach (LavaGiant lavaGiant in Constants.GameCaching.GetMyLavaGiants())
            {
                combinedDamageOutputToCastle += lavaGiant.GetLocation().PredictLavaGiantDamageDoneToCastleIfNotHitByEnemy(Constants.Game.GetEnemyCastle());
            }

            return combinedDamageOutputToCastle;
        }

        public Dictionary<int, VirtualIceTroll> GetVirtualIceTrollsInArea(Location location, float radius)
        {
            Dictionary<int, VirtualIceTroll> virtualIceTrollsInArea = new Dictionary<int, VirtualIceTroll>();

            foreach (KeyValuePair<int, VirtualIceTroll> pair in futureIceTrolls)
            {
                if (pair.Value.location.InRange(location, Mathf.RoundToInt(radius))) virtualIceTrollsInArea.Add(pair.Key, pair.Value);
            }

            return virtualIceTrollsInArea;
        }

        public Dictionary<int, VirtualPortal> GetVirtualPortalsInArea(Location location, float radius)
        {
            Dictionary<int, VirtualPortal> virtualPortalsInArea = new Dictionary<int, VirtualPortal>();

            foreach (KeyValuePair<int, VirtualPortal> pair in futurePortals)
            {
                if (pair.Value.location.InRange(location, Mathf.RoundToInt(radius))) virtualPortalsInArea.Add(pair.Key, pair.Value);
            }

            return virtualPortalsInArea;
        }
        #endregion

        #region Future locations of our own GameObjects
        public Dictionary<int, FutureLocation> GetFutureLocations()
        {
            Dictionary<int, FutureLocation> dictionary = new Dictionary<int, FutureLocation>();

            foreach(Elf elf in Constants.GameCaching.GetMyLivingElves())
            {
                dictionary.Add(elf.UniqueId, GetFutureLocation(elf));
            }

            return dictionary;
        }

        //There is no need to set the future location of a GameObject to itself since that behavior occurs anyways if the key doesnt exist
        /// <summary>
        /// Sets the future location of a GameObject to itself
        /// (this is done by removing the future location, then if we try to get the future location we will get the GameObject's current location.
        /// </summary>
        /// <param name="uniqueId"></param>
        public void SetFutureLocation(int uniqueId)
        {
            RemoveFutureLocation(uniqueId);
        }

        public void SetFutureLocation(Elf gameObject, Location futureLocation)
        {
            futureLocations[gameObject.UniqueId] = new FutureLocation(gameObject, futureLocation);
        }

        public void RemoveFutureLocation(int uniqueId)
        {
            futureLocations.Remove(uniqueId);
        }

        public void RemoveFutureLocation(GameObject gameObject)
        {
            RemoveFutureLocation(gameObject.UniqueId);
        }

        public FutureLocation GetFutureLocation(int uniqueId)
        {
            FutureLocation futureLocation;

            if(!futureLocations.TryGetValue(uniqueId, out futureLocation))
            {
                //edge case prevention
                Elf elf = (Elf)Constants.GameCaching.GetGameObjectByUniqueId(uniqueId);
                return new FutureLocation(elf, elf.GetLocation());
            }

            return futureLocation;
        }

        public FutureLocation GetFutureLocation(GameObject gameObject)
        {
            return GetFutureLocation(gameObject.UniqueId);
        }

        public bool DoesElfHaveFutureLocationIn(Elf elf, Location checkLocation)
        {
            foreach(var pair in GetFutureLocations())
            {
                if (pair.Key == elf.UniqueId && pair.Value.GetFutureLocation() == checkLocation) return true;
            }

            return false;
        }
        #endregion

        #region Virtual future spells
        public bool SetFutureInvisibility(Elf elf)
        {
            if(mana >= Constants.Game.InvisibilityCost)
            {
                futureInvisibilitySpells[elf.UniqueId] = new VirtualInvisibility(elf);
                mana -= Constants.Game.InvisibilityCost;

                return true;
            }

            return false;
        }

        public void RemoveFutureInvisibility(Elf elf)
        {
            futureInvisibilitySpells.Remove(elf.UniqueId);
            mana += Constants.Game.InvisibilityCost;
        }

        public bool SetFutureSpeedUp(Elf elf)
        {
            if(mana >= Constants.Game.SpeedUpCost)
            {
                futureSpeedUpSpells[elf.UniqueId] = new VirtualSpeedUp(elf);
                mana -= Constants.Game.SpeedUpCost;

                return true;
            }
            
            return false;
        }

        public void RemoveFutureSpeedUp(Elf elf)
        {
            futureSpeedUpSpells.Remove(elf.UniqueId);
            mana += Constants.Game.SpeedUpCost;
        }

        public bool DoesElfHaveFutureSpeedUp(Elf elf)
        {
            return futureSpeedUpSpells.ContainsKey(elf.UniqueId);
        }
        #endregion
    }
}
