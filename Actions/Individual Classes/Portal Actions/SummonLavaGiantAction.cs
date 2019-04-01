using ElfKingdom;

namespace SkillZ
{
    class SummonLavaGiantAction : Action
    {
        public SummonLavaGiantAction(GameObject gameObject) : base(gameObject) { }

        public override bool DoActionInNextTurn(VirtualGame virtualGame)
        {
            return virtualGame.AddFutureLavaGiant((Portal)gameObject);
        }

        public override void RemoveActionInNextTurn(VirtualGame virtualGame)
        {
            virtualGame.RemoveFutureLavaGiant((Portal)gameObject);
        }

        public override void DoGameAction()
        {
            Portal portal = (Portal)gameObject;

            portal.SummonLavaGiant();
        }
    }
}
