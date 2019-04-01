using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ
{
    static class Logger
    {
        private static LogLevel debug = new LogLevel(true, 220, 750);
        private static LogLevel info = new LogLevel(true, 0, 750);
        private static LogLevel warn = new LogLevel(true, 0, 750);
        private static LogLevel error = new LogLevel(true, 0, 750);
        public static List<string> lines = new List<string>();

        public static void Debug(string format, params object[] stringsToInsert)
        {
            if (stringsToInsert == null) return;

            debug.Println(format, stringsToInsert);
        }

        public static void Info(string format, params object[] stringsToInsert)
        {
            if (stringsToInsert == null) return;

            info.Println(format, stringsToInsert);
        }

        public static void Warning(string format, params object[] stringsToInsert)
        {
            if (stringsToInsert == null) return;

            string line = warn.Println(format, stringsToInsert);
            if (line != null) lines.Add(line);
        }

        public static void Error(string format, params object[] stringsToInsert)
        {
            if (stringsToInsert == null) return;

            string line = error.Println(format, stringsToInsert);
            if (line != null) lines.Add(line);
        }

        public static void EnableDebug(bool enable)
        {
            debug.isActive = enable;
        }

        /// <summary>
        /// This should be called in the last turn as the last thing that the bot is doing
        /// </summary>
        public static void PrintAllTheWarningAndErrors()
        {
            foreach(string line in lines)
            {
                System.Console.WriteLine(line);
            }

            lines.Clear();
        }

        private class LogLevel
        {
            internal bool isActive;
            private int startTurn;
            private int endTurn;

            public LogLevel(bool isActive, int startTurn, int endTurn)
            {
                this.isActive = isActive;
                this.startTurn = startTurn;
                this.endTurn = endTurn;
            }

            public string Println(string format, params object[] strings)
            {
                if (IsEnabled())
                {
                    string line = string.Format(format, strings);

                    System.Console.WriteLine(line);
                    return line;
                }
                else
                {
                    return null;
                }
            }

            private bool IsEnabled()
            {
                int turnCount = Constants.Game.Turn;

                return isActive && turnCount >= startTurn && turnCount < endTurn;
            }
        }
    }
}
