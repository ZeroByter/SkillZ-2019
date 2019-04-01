using ElfKingdom;

namespace SkillZ
{
    public class CastInvisibility : Action
    {
        public CastInvisibility(GameObject gameObject) : base(gameObject)
        {
        }

        public override bool DoActionInNextTurn(VirtualGame virtualGame)
        {
            return virtualGame.SetFutureInvisibility((Elf)gameObject);
        }

        public override void RemoveActionInNextTurn(VirtualGame virtualGame)
        {
            virtualGame.RemoveFutureInvisibility((Elf)gameObject);
        }

        public override void DoGameAction()
        {
            Elf elf = (Elf)gameObject;

            elf.CastInvisibility();
        }
    }
}
