using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ
{
    class PortalActionGenerator : ActionGenerator
    {
        public Portal portal;

        public List<Action> GenerateActions()
        {
            List<Action> actions = new List<Action>();

            if (portal.IsSummoning) return actions;

            //check if we can summon an icetroll
            if (portal.CanSummonIceTroll())
            {
                actions.Add(new SummonIceTrollAction(portal));
            }

            //check if we can summon a lavagiant
            if (portal.CanSummonLavaGiant())
            {
                actions.Add(new SummonLavaGiantAction(portal));
            }

            //check if we can summon a tornado
            if (portal.CanSummonTornado())
            {
                actions.Add(new SummonTornadoAction(portal));
            }

            return actions;
        }

        public PortalActionGenerator(Portal portal)
        {
            this.portal = portal;
        }
    }
}
