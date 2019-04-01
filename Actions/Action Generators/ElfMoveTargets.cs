using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ
{
    public class ElfMoveTargets
    {
        public class HeuristicTemporaryMoveTarget
        {
            public Circle area;
            public int turnsToExpire;

            public HeuristicTemporaryMoveTarget(Circle area, int turnsToExpire)
            {
                this.area = area;
                this.turnsToExpire = turnsToExpire;
            }
        }

        public static List<Location> PermanentMoveLocations = new List<Location>();

        public List<Location> moveLocations = new List<Location>();

        //className, ElfTemporaryMoveTarget
        private static Dictionary<string, HeuristicTemporaryMoveTarget> temporaryMoveTargets = new Dictionary<string, HeuristicTemporaryMoveTarget>();

        public ElfMoveTargets()
        {
            foreach (Portal portal in Constants.GameCaching.GetEnemyPortals())
            {
                moveLocations.Add(portal.GetLocation());
            }

            foreach (Elf elf in Constants.GameCaching.GetEnemyLivingElves())
            {
                if (elf.Invisible) continue;
                moveLocations.Add(elf.GetLocation());
            }
        }

        public static void ClearOutdatedTargets()
        {
            //creating two lists is probably not a very good performant/effecient way to handle this, but I couldn't find another way
            //only other way not to do this is if we were to use List<ElfMoveTarget> instead of a dictionary
            List<string> keysToRemove = new List<string>();
            keysToRemove.Capacity = temporaryMoveTargets.Count;

            foreach (var pair in temporaryMoveTargets)
            {
                if(pair.Value.turnsToExpire <= 0)
                {
                    keysToRemove.Add(pair.Key);
                }
                pair.Value.turnsToExpire--;
            }

            foreach (string removeKey in keysToRemove)
            {
                temporaryMoveTargets.Remove(removeKey);
            }
        }

        public static void AddHeuristicCircleWithTurnLimit(string heuristicUniqueId, Circle area, int turnsToExpire)
        {
            temporaryMoveTargets[heuristicUniqueId] = new HeuristicTemporaryMoveTarget(area, turnsToExpire);
        }

        public static void RemoveHeuristicCircle(string heuristicUniqueId)
        {
            temporaryMoveTargets.Remove(heuristicUniqueId);
        }
    }
}
