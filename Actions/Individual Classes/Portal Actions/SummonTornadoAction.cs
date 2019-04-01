using ElfKingdom;

namespace SkillZ
{
    class SummonTornadoAction : Action
    {
        public SummonTornadoAction(GameObject gameObject) : base(gameObject) { }

        public override bool DoActionInNextTurn(VirtualGame virtualGame)
        {
            return virtualGame.AddFutureTornado((Portal)gameObject);
        }

        public override void RemoveActionInNextTurn(VirtualGame virtualGame)
        {
            virtualGame.RemoveFutureTornado((Portal)gameObject);
        }

        public override void DoGameAction()
        {
            Portal portal = (Portal)gameObject;

            portal.SummonTornado();
        }
    }
}
