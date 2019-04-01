using ElfKingdom;

namespace SkillZ
{
    public static class ArrayExtensions
    {
        public static MapObject GetClosest(this MapObject[] mapObjects, MapObject source, bool includeSelf = false)
        {
            return Utilities.GetClosest(source, mapObjects, includeSelf);
        }

        public static MapObject GetClosest(this MapObject[] mapObjects, MapObject source, System.Predicate<MapObject> predicate)
        {
            return Utilities.GetClosest(source, mapObjects, predicate);
        }

        public static MapObject GetAverageLocation(this MapObject[] mapObjects)
        {
            return Utilities.AverageLocation(mapObjects);
        }
    }
}
