using ElfKingdom;

namespace SkillZ
{
    public class CastSpeedUp : Action
    {
        public CastSpeedUp(GameObject gameObject) : base(gameObject)
        {
        }

        public override bool DoActionInNextTurn(VirtualGame virtualGame)
        {
            return virtualGame.SetFutureSpeedUp((Elf)gameObject);
        }

        public override void RemoveActionInNextTurn(VirtualGame virtualGame)
        {
            virtualGame.RemoveFutureSpeedUp((Elf)gameObject);
        }

        public override void DoGameAction()
        {
            Elf elf = (Elf)gameObject;

            elf.CastSpeedUp();
        }
    }
}
