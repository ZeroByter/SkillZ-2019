using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfBuildAttackPortal : Heuristic
    {
        private float innerRadius;
        private float outerRadius;
        private float densityRadius;

        public ElfBuildAttackPortal(float weight, float innerRadius, float outerRadius, float densityRadius) : base(weight)
        {
            if(innerRadius >= outerRadius)
            {
                throw new System.Exception($"innerRadius can not be bigger than outerRadius (innerRadius = {innerRadius} outerRadius = {outerRadius})");
            }
            if(innerRadius < Constants.Game.CastleSize + Constants.Game.PortalSize)
            {
                throw new System.Exception($"innerRadius can not be smaller than CastleSize + PortalSize (innerRadius = {innerRadius})");
            }
            if(densityRadius < 2 * Constants.Game.PortalSize)
            {
                throw new System.Exception($"densityRadius can not be smaller than PortalSize * 2 (densityRadius = {densityRadius})");
            }

            this.innerRadius = innerRadius;
            this.outerRadius = outerRadius;
            this.densityRadius = densityRadius;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            Castle enemyCastle = Constants.Game.GetEnemyCastle();

            int score = 0;

            foreach(KeyValuePair<int, VirtualPortal> pair in virtualGame.futurePortals)
            {
                Location location = pair.Value.location;

                int distanceToEnemyCastle = location.Distance(enemyCastle);

                if (distanceToEnemyCastle >= innerRadius && distanceToEnemyCastle <= outerRadius)
                {
                    if (virtualGame.CountAllPortalsInArea(location, densityRadius) == 1)
                    {
                        score++;
                    }
                }
            }

            return score;
        }
    }
}

/*- ElfBuildAttackPortal
  My elves building offensive attack portals. implementation:
  members: 
  innerRadius - the inner distance/circle to enemy castle we should build portals. we dont build portals closer than this
  outerRadius - the outer distance/circle to enemy castle we should build portals. we dont build portals further from the enemy castle than this
  densityRadius - how dense should the portals be, or in other words, how spreadout should they be
  score: 
    for each elf:
	  simply loop through futurePortals and based on distance to enemy castle and density, add +1 or not*/
