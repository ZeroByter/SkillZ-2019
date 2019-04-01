using ElfKingdom;

namespace SkillZ.Bots
{
    public abstract class Bot
    {
        public abstract void DoTurn(Game game);

        public virtual void PreDoTurn(Game game)
        {
            ElfMoveTargets.ClearOutdatedTargets();
        }

        /// <summary>
        /// You most likely don't need to override this. Is is only for some very special cases
        /// 
        /// I made this virtual so that it can be overwritten. There are some specific challenge bots we shouldn't/don't need to call this on.
        /// </summary>
        /// <param name="game"></param>
        public virtual void PostDoTurn(Game game)
        {
            LastHealth.UpdateLastHealths();
            LastPosition.UpdateLastPositions();
            TrackPortalCreations.UpdateEnemyPortalCreations();
        }
    }
}
