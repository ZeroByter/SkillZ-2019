using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ
{
    public static class LastPosition
    {
        public static Dictionary<int, Location> LastPositions = new Dictionary<int, Location>();

        /// <summary>
        /// Call this before ships make their decisions
        /// </summary>
        public static void UpdateLastPositions()
        {
            Dictionary<int, Location> temp = new Dictionary<int, Location>();

            foreach (GameObject gameObject in Constants.GameCaching.GetAllGameObjects().Values)
            {
                if (gameObject is Building) continue;

                if (gameObject is Elf && ((Elf)gameObject).Invisible)
                {
                    temp[gameObject.UniqueId] = LastPositions[gameObject.UniqueId];
                }
                else
                {
                    temp[gameObject.UniqueId] = gameObject.GetLocation();
                }
            }

            LastPositions = temp;
        }

        /// <summary>
        /// Gets the provided ship's last position
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static Location GetLastLocation(GameObject gameObject)
        {
            if (gameObject is Building) return gameObject.GetLocation();

            Location lastLocation;
            if(LastPositions.TryGetValue(gameObject.UniqueId, out lastLocation))
            {
                return lastLocation;
            }

            return null;
        }

        /// <summary>
        /// Gets the direction this gameObject is heading
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static LocationF GetDirection(GameObject gameObject)
        {
            LocationF velocity = GetVelocity(gameObject);
            if (velocity.Magnitude() == 0) return new LocationF();
            return velocity.Normalized();
        }

        /// <summary>
        /// Returns the velocity location (meaning speed+direction) of the given gameObject
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static LocationF GetVelocity(GameObject gameObject)
        {
            LocationF lastLocation = GetLastLocation(gameObject).GetFloatLocation();
            if (lastLocation == null) return new LocationF();
            return gameObject.GetLocation().GetFloatLocation() - lastLocation;
        }

        /// <summary>
        /// Is a given object heading approximately towards this target location?
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsHeadingTowards(GameObject gameObject, MapObject target, float certainty)
        {
            LocationF direction = GetDirection(gameObject);
            if (direction == null) return false;
            if (direction.Magnitude() == 0) return false; //gameObject is standing still
            float dot = direction.Dot(target.GetLocation().Subtract(gameObject.GetLocation()).Normalized());
            return dot >= certainty;
        }

        /// <summary>
        /// Is a given object heading approximately towards this target location?
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsHeadingTowards(GameObject gameObject, MapObject target)
        {
            return IsHeadingTowards(gameObject, target, 0.9f);
        }

        public static bool IsAnyHeadingTowards(GameObject[] gameObjects, MapObject target, float certainty)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (IsHeadingTowards(gameObject, target)) return true;
            }

            return false;
        }

        public static bool IsAnyHeadingTowards(GameObject[] gameObjects, MapObject target)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (IsHeadingTowards(gameObject, target, 0.9f)) return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the potential/maximum speed of this gameObject
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static int GetMaximumSpeed(GameObject gameObject)
        {
            if (Constants.Game == null) return 1;

            switch (gameObject.Type)
            {
                case "Elf":
                    return Constants.Game.ElfMaxSpeed;
                case "IceTroll":
                    return Constants.Game.IceTrollMaxSpeed;
                case "LavaGiant":
                    return Constants.Game.LavaGiantMaxSpeed;
            }

            return 0;
        }

        /// <summary>
        /// I have tried to do this the smart/elegant/good way using a simple equation, but it doesnt fucking work.
        /// So here it is, hacked and done really weirdly and maybe even badly but it should work more reliably
        /// Also possibly more intensive too
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="timeToImpact"></param>
        /// <returns></returns>
        private static Location GetStupidInterceptLocation(GameObject source, GameObject target, out int timeToImpact)
        {
            int sourceVelocity = GetMaximumSpeed(source);
            LocationF targetVelocity = target.GetVelocity();

            //Before we do any calculations, we do some simple, non-intensive checks to make sure we can actually intercept the target before getting the location to intercept

            //We can not move! Interception is impossible
            if(sourceVelocity == 0)
            {
                timeToImpact = -1;
                return target.GetLocation();
            }

            if (targetVelocity.Magnitude() == 0) //target is not moving, we will never get him
            {
                timeToImpact = source.Distance(target) / sourceVelocity;
                return target.GetLocation();
            }

            if (!IsHeadingTowards(target, source.GetLocation(), 0f)) //if the target isn't heading towards us
            {
                timeToImpact = source.Distance(target) / sourceVelocity;
                return target.GetLocation();
            }
            
            for(int i = 0; i < 4000; i++)
            {
                Location possibleLocation = target.GetLocation().Add(targetVelocity.GetIntLocation().Multiply(i));

                float sourceTimeToLocation = source.Distance(possibleLocation) / sourceVelocity;
                float targetTimeToLocation = target.Distance(possibleLocation) / targetVelocity.Magnitude();

                if(Mathf.Abs(sourceTimeToLocation - targetTimeToLocation) < 1)
                {
                    timeToImpact = Mathf.RoundToInt(sourceTimeToLocation);
                    return possibleLocation;
                }
            }

            //Failed to find intercept point, just head to target
            timeToImpact = source.Distance(target) / sourceVelocity;
            return target.GetLocation();
        }

        /// <summary>
        /// Given a source and target gameObject, returns the position needed for `source` to move to in order to intercept `target`. Also returns the turn at which this will happen.
        /// </summary>
        /// <param name="source">Our gameObject</param>
        /// <param name="target">Target gameObject we want to intercept</param>
        /// <param name="timeToImpact">The turn number at which we will intercept the target</param>
        /// <returns></returns>
        public static Location GetInterceptLocation(GameObject source, GameObject target, out int timeToImpact)
        {
            return GetStupidInterceptLocation(source, target, out timeToImpact);

            /*
            //Ripped all this math from one of my own Unity's game... Which is in 3d, but this should be the same (hopefully) - Rom
            Location dir = source.GetLocation().Subtract(target.GetLocation());
            int sourceVelocity = GetMaximumSpeed(source);
            Location targetVelocity = GetVelocity(target);

            if (targetVelocity.Magnitude() == 0 || sourceVelocity == 0)
            { //if we haven't determined yet the objects' velocities, simply return target location
                timeToImpact = source.Distance(target) / sourceVelocity;
                return target.GetLocation();
            }

            if (!IsHeadingTowards(target, source.GetLocation(), 0f))
            { //if the target isn't heading towards us
                timeToImpact = source.Distance(target) / sourceVelocity;
                return target.GetLocation();
            }

            float a = targetVelocity.Dot(targetVelocity) - (sourceVelocity * sourceVelocity);
            float b = 2 * targetVelocity.Dot(dir);
            float c = dir.Dot(dir);

            float p = -b / (2 * a);
            float q = Mathf.Sqrt((b * b) - 4 * a * c) / (2 * a);

            float t1 = p - q;
            float t2 = p + q;
            float t;

            if (t1 <= 0 && t2 <= 0)
            {
                timeToImpact = source.Distance(target) / sourceVelocity;
                return target.GetLocation();
            }

            if (t1 > t2 && t2 > 0) {
                t = t2;
            } else {
                t = t2 * -1;
            }

            if (t != t) //IsNaN check
            {
                timeToImpact = source.Distance(target) / sourceVelocity;
                return target.GetLocation();
            }

            /*System.Console.WriteLine(source);
            System.Console.WriteLine($"sourceVelocity = {sourceVelocity}");
            System.Console.WriteLine($"targetVelocity = {targetVelocity}");
            System.Console.WriteLine($"a = {a} - b = {b} - c = {c}");
            System.Console.WriteLine($"p = {p} - q = {q}");
            System.Console.WriteLine($"t1 = {t1} - t2 = {t2} - t = {t}");

            Location interceptPosition = target.GetLocation().Add(targetVelocity.Multiply(t));
            Location path = interceptPosition.Subtract(source.GetLocation());
            float timeToImpactF = path.Magnitude() / sourceVelocity;
            timeToImpact = Mathf.RoundToInt(timeToImpactF);

            //System.Console.WriteLine(string.Format("a = {0} - b = {1} - c = {2} - t1 = {3} - t2 = {4} - t = {5} - tVelocity = {6} - tVelocity.mag = {7}", a, b, c, t1, t2, t, targetVelocity, targetVelocity.Magnitude()));

            return interceptPosition;*/
        }

        /// <summary>
        /// Another variation of GetInterceptLocation except this also doesnt return the turn at the gameObjects will hit, use this if you dont care at what turn the will hit.
        /// </summary>
        /// <param name="source">Our gameObject</param>
        /// <param name="target">Target gameObject we want to intercept</param>
        /// <returns></returns>
        public static Location GetInterceptLocation(GameObject source, GameObject target)
        {
            return GetInterceptLocation(source, target, out int timeToImpact);
        }

        /// <summary>
        /// Returns GetInterceptLocation, but adds an offset incase we want to account for 'attackRange' or something
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Location GetInterceptLocation(GameObject source, GameObject target, int offset)
        {
            Location interceptLocation = GetInterceptLocation(source, target, out int timeToImpact);
            if (interceptLocation == target.GetLocation())
            {
                return interceptLocation;
            }
            else
            {
                return interceptLocation.Add((GetDirection(target) * offset).GetIntLocation());
            }
        }

        /// <summary>
        /// Returns GetInterceptLocation, but adds an offset incase we want to account for 'attackRange' or something. Also returns timeToImpact
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Location GetInterceptLocation(GameObject source, GameObject target, int offset, out int timeToImpact)
        {
            Location interceptLocation = GetInterceptLocation(source, target, out timeToImpact);
            if (interceptLocation == target.GetLocation())
            {
                return interceptLocation;
            }
            else
            {
                return interceptLocation.Add((GetDirection(target) * offset).GetIntLocation());
            }
        }
    }
}
