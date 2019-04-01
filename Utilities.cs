using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ
{
    public static class Utilities
    {
        public static void DebugArray<T>(T[] array)
        {
            foreach (object obj in array)
            {
                System.Console.WriteLine(obj.ToString());
            }
        }

        public static MapObject GetClosest(MapObject source, MapObject[] locations, bool includeSelf = false)
        {
            if (locations.Length == 0 || source == null)
            {
                return null;
            }
            else
            {
                MapObject closest = null;

                foreach (MapObject location in locations)
                {
                    if (location == null) continue;

                    int distanceToSource = location.Distance(source);

                    if (!includeSelf && location.Equals(source)) continue;

                    if (closest == null || distanceToSource < closest.Distance(source))
                    {
                        closest = location;
                    }
                }

                return closest;
            }
        }

        public static MapObject GetClosest(MapObject source, MapObject[] locations, System.Predicate<MapObject> predicate)
        {
            if (locations.Length == 0 || source == null)
            {
                return null;
            }
            else
            {
                MapObject closest = null;

                foreach (MapObject location in locations)
                {
                    int distanceToSource = location.Distance(source);

                    if (location.Equals(source)) continue;

                    if (!predicate.Invoke(location)) continue;

                    if (closest == null || distanceToSource < closest.Distance(source))
                    {
                        closest = location;
                    }
                }

                return closest;
            }
        }

        public static Location AverageLocation(MapObject[] locations)
        {
            int rows = 0;
            int cols = 0;

            foreach (MapObject location in locations)
            {
                rows += location.GetLocation().Row;
                cols += location.GetLocation().Col;
            }

            if (locations.Length == 0)
            {
                return null;
            }
            else
            {
                return new Location(rows / locations.Length, cols / locations.Length);
            }
        }

        /// <summary>
        /// Tries to attack gameObject, if we can't we move to it's intercept location
        /// </summary>
        /// <param name="elf"></param>
        /// <param name="target"></param>
        public static void TryAttack(Elf elf, GameObject target)
        {
            if (target == null) return;

            if (elf.InAttackRange(target))
            {
                elf.Attack(target);
            }
            else
            {
                elf.MoveToIntercept(target);
            }
        }

        public static Location GetAngledLocation(MapObject source, MapObject target, float angle, float size, int width)
        {
            float sourceToTargetAngle = Mathf.GetAngle(target, source);
            float tAngle = sourceToTargetAngle + angle;

            Location intersectionLocation = Mathf.GetNewLocationFromLocation(source, sourceToTargetAngle, size);
            Location tLocation = Mathf.GetNewLocationFromLocation(intersectionLocation, tAngle, width);
            return tLocation;
        }

        /// <summary>
        /// Returns a location resembling a 't-shape' given an angle between two positions
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="leftSide">Is the location on the left side or the right side?</param>
        /// <param name="width">How big is the 'base' of the T? Or rather, how LONG the T is</param>
        /// <param name="distance">How far away is the T-location from the original line/angle? (or rather, or wide the T is)</param>
        /// <returns></returns>
        public static Location GetTLocation(MapObject source, MapObject target, bool leftSide, int size, int width)
        {
            float sourceToTargetAngle = Mathf.GetAngle(target, source);
            float tAngle = 0;
            if (leftSide)
            {
                tAngle += 90;
            }
            else
            {
                tAngle -= 90;
            }

            return GetAngledLocation(source, target, tAngle, size, width);
        }

        /// <summary>
        /// Given a list of portals, this returns a list of all portals threatened by enemy elfs
        /// </summary>
        /// <param name="portals"></param>
        /// <returns></returns>
        public static List<Portal> GetThreatenedPortals(List<Portal> portals)
        {
            List<Portal> threatenedPortals = new List<Portal>();

            foreach (Portal portal in portals)
            {
                Elf closestElf = (Elf)Utilities.GetClosest(portal, Constants.GameCaching.GetEnemyLivingElves());
                if (closestElf != null)
                { //if the enemy has any closest elf
                    if (closestElf.InRange(portal, Constants.Game.ElfAttackRange * 4))
                    { //if the closest elf is in the threat distance
                        if (closestElf.InAttackRange(portal))
                        { //if the closestElf is already in attack range of 
                            threatenedPortals.Add(portal); //this portal is threatened
                        }
                        else
                        { //if the elf is not in attack range of this portal
                            bool isHeadingTowards = closestElf.IsHeadingTowards(portal);
                            if (isHeadingTowards)
                            { //the closestElf is heading towards this portal
                                threatenedPortals.Add(portal); //this portal is threatened
                            }
                        }
                    }
                }
            }

            return threatenedPortals;
        }

        public static int PredictFutureDamage(MapObject source, Building target, int attackRange, int maxSpeed, int attackMultiplier, int health, int suffocation)
        {
            float distance = source.DistanceF(target) - target.Size - attackRange;
            float timeToArrive = distance / maxSpeed;
            float timeToDie = health / suffocation;

            if (timeToArrive < timeToDie)
            {
                float timeAtTarget = timeToDie - timeToArrive;
                return Mathf.RoundToInt(timeAtTarget * attackMultiplier);
            }

            return 0;
        }

        public static void CreateHeuristicsForBuildingSidePortals(
            MapObject source,
            MapObject target,
            float angle,
            float size,
            float width,
            float sidesMoveWeight,
            float sidesBuildWeight,
            List<Circle> circleToFill)
        {
            Location location = source.GetAngledLocation(target, angle, size, Mathf.FloorToInt(width));

            if (Constants.Game.CanBuildPortalAt(location))
            {
                Circle centerObjective = new Circle(location, Constants.Game.ElfMaxSpeed);
                circleToFill.Add(centerObjective);
                Heuristics.AddHeuristic(new IndividualHeuristics.ElfBuildPortal(sidesBuildWeight, centerObjective));
            }
        }

        public static void CreateHeuristicsForBuildingSideManaFountains(
            MapObject source,
            MapObject target,
            float angle,
            float size,
            float width,
            float sidesMoveWeight,
            float sidesBuildWeight,
            List<Circle> circleToFill)
        {
            Location location = source.GetAngledLocation(target, angle, size, Mathf.FloorToInt(width));

            if (Constants.Game.CanBuildManaFountainAt(location))
            {
                Circle centerObjective = new Circle(location, Constants.Game.ElfMaxSpeed);
                circleToFill.Add(centerObjective);
                Heuristics.AddHeuristic(new IndividualHeuristics.ElfBuildManaFountainInCircle(sidesBuildWeight, centerObjective));
            }
        }

        public static void CreateHeuristicsForBuildingPortalsAroundPivotLocation(
            MapObject source,
            MapObject target,
            float angle,
            float size,
            float widthStep,
            float centerMoveWeight,
            float centerBuildWeight,
            float sidesMoveWeight,
            float sidesBuildWeight,
            bool isBuildCenter,
            int numberOfSidePortals,
            float maxDistanceToCircle)
        {
            List<Circle> circles = new List<Circle>();
            if (isBuildCenter)
            {
                Location centerLocation = source.Lerp(target, size / source.DistanceF(target));

                if (Constants.Game.canBuildInLocation(Constants.Game.PortalSize, centerLocation))
                {
                    Circle centerObjective = new Circle(centerLocation, Constants.Game.ElfMaxSpeed);
                    circles.Add(centerObjective);
                    Heuristics.AddHeuristic(new IndividualHeuristics.ElfBuildPortal(centerBuildWeight, centerObjective));
                }
            }

            for (int i = 1; i <= numberOfSidePortals; i++)
            {
                CreateHeuristicsForBuildingSidePortals(source, target, angle, size, widthStep * i, sidesMoveWeight, sidesBuildWeight, circles);
                CreateHeuristicsForBuildingSidePortals(source, target, -angle, size, widthStep * i, sidesMoveWeight, sidesBuildWeight, circles);
            }

            if (circles.Count > 0)
            {
                Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleMoveToBestBuildPortalArea(sidesMoveWeight, circles, maxDistanceToCircle));
            }
        }

        //this file is getting kind of long, perhaps it's time to create multiple utility files in some 'Utility' folder and divide up these functions

        public static float GetManaFountainRatio(float enemyManaFountainModifier = 1.25f, float myManaFountainModifier = 0.25f)
        {
            int numberOfEnemyManaFountains = Constants.GameCaching.GetEnemyManaFountains().Length;
            numberOfEnemyManaFountains += Constants.GameCaching.GetEnemyElvesCurrentlyBuildingManaFountains().Count;
            int numberOfMyManaFountains = Constants.GameCaching.GetMyManaFountains().Length;
            numberOfMyManaFountains += Constants.GameCaching.GetMyCurrentlyBuildingManaFountainsElves().Count;

            return (numberOfEnemyManaFountains + enemyManaFountainModifier) / (numberOfMyManaFountains + myManaFountainModifier);
        }

        public static float GetDot(MapObject start, MapObject end, MapObject target)
        {
            LocationF startF = start.GetLocation().GetFloatLocation();
            LocationF endF = end.GetLocation().GetFloatLocation();
            
            LocationF direction = (endF - startF).Normalized();
            return (target.GetLocation().GetFloatLocation() - startF).Normalized().Dot(direction);
            //return target.GetLocation().Subtract(start.GetLocation()).Normalized().Dot(direction);
        }

        public static bool IsLocationInCone(MapObject coneStart, MapObject coneEnd, MapObject target, float coneSize)
        {
            return GetDot(coneStart, coneEnd, target) >= coneSize;
        }
    }
}
