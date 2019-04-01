using ElfKingdom;

namespace SkillZ.Bots
{
    public static class BotFactory
    {
        private static Bot Instance;

        public static Bot GetInstance()
        {
            if (Instance == null)
            {
                Instance = new MainBot();
            }

            return Instance;
        }
    }
}
