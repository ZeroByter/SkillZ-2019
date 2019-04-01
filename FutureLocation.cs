using ElfKingdom;

namespace SkillZ
{
    public class FutureLocation
    {
        private readonly Elf elf;
        private readonly Location futureLocation;

        public FutureLocation(Elf elf, Location futureLocation)
        {
            this.elf = elf;
            this.futureLocation = futureLocation;
        }

        public Elf GetElf()
        {
            return elf;
        }

        public Location GetFutureLocation()
        {
            return futureLocation;
        }
    }
}
