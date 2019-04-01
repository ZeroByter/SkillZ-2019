using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ
{
    public static class LocationExtensions
    {
        public static Location Divide(this Location a, float b)
        {
            return new Location(Mathf.RoundToInt(a.Row / b), Mathf.RoundToInt(a.Col / b));
        }

        public static float Dot(this Location l1, Location l2)
        {
            return l1.Col * l2.Col + l1.Row * l2.Row;
        }

        public static float Magnitude(this Location a)
        {
            return Mathf.Sqrt(a.Row * a.Row + a.Col * a.Col);
        }

        public static LocationF Normalized(this Location a)
        {
            LocationF b = a.GetFloatLocation();
            float magnitude = b.Magnitude();
            if (magnitude > 0)
            {
                return b / magnitude;
            }
            else
            {
                return new LocationF();
            }
        }

        public static LocationF GetFloatLocation(this Location location)
        {
            if (location == null) return null;
            return new LocationF(location.Col, location.Row);
        }

        public static bool ContainsFriendlyBuilding(this Location location, int offset = 0)
        {
            Building building = Constants.Game.GetBuildingAtLocation(location, offset);

            if (building != null)
            {
                return building.Owner == Constants.Game.GetMyself();
            }
            else
            {
                return false;
            }
        }

        public static bool IsApproximately(this MapObject location, MapObject otherLocation)
        {
            return location.Distance(otherLocation) < 10;
        }

        /// <summary>
        /// Is any location in the array approximately equal the given location?
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="otherLocation"></param>
        /// <returns></returns>
        public static bool IsApproximately(this MapObject[] locations, MapObject otherLocation)
        {
            foreach (MapObject location in locations)
            {
                if (location.IsApproximately(otherLocation)) return true;
            }

            return false;
        }

        public static bool AreAnyElfsBuilding(this Location location, int newBuildingSize = 0)
        {
            foreach(Elf elf in Constants.Game.GetAllLivingElves())
            {
                if (elf.IsBuilding)
                {
                    int size = 0;
                    if (elf.CurrentlyBuilding == "Portal") size = Constants.Game.PortalSize;
                    if (location.InRange(elf, size + newBuildingSize)) return true;
                }
            }

            return false;
        }

        public static Building GetBuilding(this Location location, int ourSize)
        {
            return Constants.Game.GetBuildingAtLocation(location, ourSize);
        }

        /// <summary>
        /// Checks if there is already a building here or not. This DOES NOT take into account mana
        /// </summary>
        /// <returns>True if can build portal, false otherwise</returns>
        public static bool CanBuildPortal(this Location location, bool includePlannedPortals = true)
        {
            int portalSize = Constants.Game.PortalSize;

            if (!location.InMap()) return false;

            //here we check if the building is a castle, cant build on a castle!
            Building existingBuilding = location.GetBuilding(Constants.Game.PortalSize);
            if(existingBuilding != null)
            {
                //System.Console.WriteLine($"CanBuildPortal = existing building = {existingBuilding}");
                if(existingBuilding is Castle) return false;
            }

            if (includePlannedPortals)
            {
                //if (location.ContainsPlannedPortal(Constants.Game.PortalSize)) return false;
                return !location.ContainsFriendlyBuilding(portalSize) && !AreAnyElfsBuilding(location);
            }
            else
            {
                return !location.ContainsFriendlyBuilding(portalSize);
            }
        }

        public static Location KeepLocationInMapBounds(this Location location)
        {
            int padding = 10;
            int cols = Constants.Game.Cols;
            int rows = Constants.Game.Rows;

            if (location.Col < padding) location.Col = padding;
            if (location.Row < padding) location.Row = padding;
            if (location.Col > cols - padding) location.Col = cols - padding;
            if (location.Row > rows - padding) location.Row = rows - padding;

            return location;
        }
    }
}
