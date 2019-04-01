using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfInvisibilitySpellToKeepDistanceFromEnemyElves : Heuristic
    {
        private float radius;
        private float radiusWhenDontHaveMuchHealth;
        private int whatIsNotMuchHealth;

        public ElfInvisibilitySpellToKeepDistanceFromEnemyElves(float weight, float radius, float radiusWhenDontHaveMuchHealth, int whatIsNotMuchHealth) : base(weight)
        {
            this.radius = radius;
            this.radiusWhenDontHaveMuchHealth = radiusWhenDontHaveMuchHealth;
            this.whatIsNotMuchHealth = whatIsNotMuchHealth;
        }

        private float GetMyElfScore(VirtualGame virtualGame, VirtualInvisibility virtualInvisibility)
        {
            Elf myElf = (Elf)virtualInvisibility.realGameObject;
            float useRadius = radius;
            if (myElf.CurrentHealth < whatIsNotMuchHealth) useRadius = radiusWhenDontHaveMuchHealth;

            List<Elf> enemyElves = Constants.GameCaching.GetEnemyElvesInArea(new Circle(myElf.GetLocation(), useRadius));
            if (enemyElves.Count == 0) return 0;

            Dictionary<int, GameObject> myElves = new Dictionary<int, GameObject>();

            int enemyCombinedHealth = 0;

            foreach (Elf enemyElf in enemyElves)
            {
                enemyCombinedHealth += enemyElf.CurrentHealth;
                foreach (Elf elf in Constants.GameCaching.GetMyElvesInArea(new Circle(enemyElf.GetLocation(), useRadius)))
                {
                    myElves[elf.UniqueId] = elf;
                }
            }

            float ourCombinedHealth = 0;

            foreach (KeyValuePair<int, GameObject> pair in myElves)
            {
                ourCombinedHealth += pair.Value.CurrentHealth;
            }

            if (ourCombinedHealth == enemyCombinedHealth)
            {
                foreach (GameObject enemyElf in enemyElves)
                {
                    if (enemyElf.OnSameSideAsCastle()) //if enemy elf is our side of the map
                    {
                        return 0;
                    }
                }
            }
            else if (ourCombinedHealth > enemyCombinedHealth)
            {
                return 0;
            }

            ourCombinedHealth = Mathf.Max(1f, myElf.CurrentHealth - 2);
            return (enemyCombinedHealth / ourCombinedHealth);
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (VirtualInvisibility virtualInvisibility in virtualGame.futureInvisibilitySpells.Values)
            {
                score += GetMyElfScore(virtualGame, virtualInvisibility);
            }

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}
