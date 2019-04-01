using ElfKingdom;
using System.Collections.Generic;
using System.Linq;

namespace SkillZ
{
    public static class MapObjectExtensions
    {
        public static float DistanceF(this MapObject source, MapObject target)
        {
            return Mathf.Sqrt(Mathf.Pow(source.GetLocation().Col - target.GetLocation().Col, 2) + Mathf.Pow(source.GetLocation().Row - target.GetLocation().Row, 2));
        }

        public static float DistanceF(this MapObject source, Circle circle)
        {
            return source.DistanceF(circle.GetCenter());
        }

        public static Location Lerp(this MapObject a, MapObject b, float lerp)
        {
            return new Location(Mathf.RoundToInt(Mathf.Lerp(a.GetLocation().Row, b.GetLocation().Row, lerp)), Mathf.RoundToInt(Mathf.Lerp(a.GetLocation().Col, b.GetLocation().Col, lerp)));
        }

        /// <summary>
        /// Is this location on the same side of the map as our castle?
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static bool OnSameSideAsCastle(this MapObject location)
        {
            if (location == null) return false;

            Game game = Constants.Game;

            int lineLength = Mathf.RoundToInt(game.GetMyCastle().Distance(game.GetEnemyCastle()) * 0.5f);
            Location lineStart = game.GetMyCastle().GetTLocation(game.GetEnemyCastle(), true, lineLength, 100);
            Location lineEnd = game.GetMyCastle().GetTLocation(game.GetEnemyCastle(), false, lineLength, 100);

            return Mathf.IsLocationOnRightOfLine(lineStart, lineEnd, location.GetLocation());
        }

        public static float GetAngle(this MapObject source, MapObject target)
        {
            return Mathf.GetAngle(target, source);
        }

        public static Location GetNewLocation(this MapObject source, float angle, int length)
        {
            return Mathf.GetNewLocationFromLocation(source, angle, length);
        }

        public static Location GetNewLocation(this MapObject source, MapObject target, float angleOffset, int length)
        {
            float angle = source.GetAngle(target) + angleOffset;
            return source.GetNewLocation(angle, length);
        }

        //if checkForPortal is false, we check for mana fountain
        private static bool CanBuildInLocation(MapObject location, bool checkForPortal)
        {
            if (checkForPortal)
            {
                return Constants.Game.CanBuildPortalAt(location.GetLocation());
            }
            else
            {
                return Constants.Game.CanBuildManaFountainAt(location.GetLocation());
            }
        }

        public static Location GetClosestBuildableLocation(this MapObject location, System.Predicate<Location> isLocationValid, bool checkForPortal = true, MapObject towards = null, List<Building> uncheckedBuildings = null)
        {
            if (CanBuildInLocation(location, checkForPortal) && uncheckedBuildings == null)
            {
                //if we can build here and we are not checking a building, we can just return the current location
                return location.GetLocation();
            }
            else //we can't build here, time to look for an available spot
            {
                if (uncheckedBuildings == null) //if the list of checked buildings is null (meaning we were given an arbitrery location)
                {
                    uncheckedBuildings = Constants.Game.GetAllBuildings().ToList(); //we create a new list with all buildings
                    //System.Console.WriteLine("All unchecked buildings:");
                    //Utilities.DebugArray(uncheckedBuildings.ToArray());
                    //System.Console.WriteLine($"location: {location}");
                    //System.Console.WriteLine($"closest unchecked building to location: {uncheckedBuildings.ToArray().GetClosest(location)}");
                    //start recursion on the closest building to the arbitrery location
                    return uncheckedBuildings.ToArray().GetClosest(location, true).GetClosestBuildableLocation(isLocationValid, checkForPortal, towards, uncheckedBuildings);
                }

                int startAngle = 0;
                if(towards != null)
                {
                    startAngle = Mathf.RoundToInt(location.GetAngle(towards));
                }

                for (int i = startAngle; i < startAngle + 360; i++) //check all angles, this can probably be optimized but it's not a problem so fuck it
                {
                    Location newLocation = location.GetNewLocation(i, ((Building)location).Size + Constants.Game.PortalSize + 10 * 3); //get the new location

                    if (newLocation == null) continue;

                    if (isLocationValid != null && !isLocationValid.Invoke(newLocation)) continue; //if the delegate/predicate/lambda however you want to call it returns false, this newlocation is invalid and we move on

                    if (CanBuildInLocation(newLocation, checkForPortal)) //if this location is suitable to build at
                    {
                        return newLocation; //return this location
                    }
                }

                //remove this building since we just checked it and we don't want to check it again
                uncheckedBuildings.Remove((Building)location);

                return uncheckedBuildings.ToArray().GetClosest(location).GetClosestBuildableLocation(isLocationValid, checkForPortal, towards, uncheckedBuildings); //we didn't find an available location near our given building, start recursion on next closest building
            }
        }

        public static Location GetClosestBuildableLocation(this MapObject location, MapObject towards = null, bool checkForPortal = true, List<Building> uncheckedBuildings = null)
        {
            return GetClosestBuildableLocation(location, null, checkForPortal, towards, uncheckedBuildings);
        }

        /// <summary>
        /// Returns a location from a source to a target with an offset angle and an offset 'base'
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="angle"></param>
        /// <param name="size">How big is the 'base' of the T? Or rather, how LONG the T is</param>
        /// <param name="width">How far away is the T-location from the original line/angle? (or rather, or wide the T is)</param>
        /// <returns></returns>
        public static Location GetAngledLocation(this MapObject source, MapObject target, float angle, float size, int width)
        {
            return Utilities.GetAngledLocation(source, target, angle, size, width);
        }

        /// <summary>
        /// Returns a location resembling a 't-shape' given an angle between two positions
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="leftSide">Is the location on the left side or the right side?</param>
        /// <param name="width">How big is the 'base' of the T? Or rather, how LONG the T is</param>
        /// <param name="distance">How far away is the T-location from the original line/angle? (or rather, or wide the T is)</param>
        /// <returns>New T-junction location</returns>
        public static Location GetTLocation(this MapObject source, MapObject target, bool leftSide, int size, int width)
        {
            return Utilities.GetTLocation(source, target, leftSide, size, width);
        }

        public class CompareDistance : IComparer<MapObject>
        {
            private MapObject source;

            public CompareDistance(MapObject source)
            {
                this.source = source;
            }

            int IComparer<MapObject>.Compare(MapObject x, MapObject y)
            {
                int xDistanceToSource = x.GetLocation().Distance(source);
                int yDistanceToSource = y.GetLocation().Distance(source);
                return xDistanceToSource.CompareTo(yDistanceToSource);
            }
        }

        public static List<IceTroll> GetEnemyIceTrollsInArea(this MapObject source, int range)
        {
            List<IceTroll> list = new List<IceTroll>();

            foreach (IceTroll troll in Constants.GameCaching.GetEnemyIceTrolls())
            {
                if(troll.InRange(source, range))
                {
                    list.Add(troll);
                }
            }

            return list;
        }

        public static int PredictLavaGiantDamageDoneToCastleIfNotHitByEnemy(this MapObject source, Castle target, int health)
        {
            int timeToArrival = Mathf.CeilToInt((source.DistanceF(target) - Constants.Game.LavaGiantAttackRange - target.GetSize()) / Constants.Game.LavaGiantMaxSpeed);
            int timeToSuffocation = health / Constants.Game.LavaGiantSuffocationPerTurn;

            //if we will arrive before we die
            if (timeToArrival < timeToSuffocation)
            {
                int timeSpentAtTarget = timeToSuffocation - timeToArrival;
                int damageDone = timeSpentAtTarget * Constants.Game.LavaGiantAttackMultiplier;
                return damageDone;
            }
            else
            {
                return 0;
            }
        }

        public static int PredictLavaGiantDamageDoneToCastleIfNotHitByEnemy(this MapObject source, Castle target)
        {
            return source.PredictLavaGiantDamageDoneToCastleIfNotHitByEnemy(target, Constants.Game.LavaGiantMaxHealth);
        }

        public static List<Elf> GetEnemyLivingElvesInArea(this MapObject source, float area)
        {
            List<Elf> elves = new List<Elf>();

            foreach(Elf elf in Constants.GameCaching.GetEnemyLivingElves())
            {
                if (elf.Invisible) continue;

                if (elf.InRange(source, Mathf.RoundToInt(area))) elves.Add(elf);
            }

            return elves;
        }

        public static List<Elf> GetMyLivingElvesInAreaBasedOnFutureLocation(this MapObject source, VirtualGame virtualGame, float area)
        {
            List<Elf> elves = new List<Elf>();

            foreach (Elf elf in Constants.GameCaching.GetMyLivingElves())
            {
                FutureLocation futureLocation = virtualGame.GetFutureLocation(elf);
                if (futureLocation.GetFutureLocation().InRange(source, Mathf.RoundToInt(area))) elves.Add(elf);
            }

            return elves;
        }
    }
}
