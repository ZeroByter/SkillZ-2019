using ElfKingdom;

namespace SkillZ
{
    class SummonIceTrollAction : Action
    {
        public SummonIceTrollAction(GameObject gameObject) : base(gameObject) { }

        public override bool DoActionInNextTurn(VirtualGame virtualGame)
        {
            return virtualGame.AddFutureIceTroll((Portal)gameObject);
        }

        public override void RemoveActionInNextTurn(VirtualGame virtualGame)
        {
            virtualGame.RemoveFutureIceTroll((Portal)gameObject);
        }

        public override void DoGameAction()
        {
            Portal portal = (Portal)gameObject;

            portal.SummonIceTroll();
        }
    }
}
