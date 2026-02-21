namespace MonsterTamer.Pause
{
    /// <summary>
    /// Global pause manager for the game.
    /// </summary>
    internal static class PauseManager
    {
        internal static bool IsPaused { get; private set; } = false;

        internal static void SetPaused(bool paused)
        {
            if (IsPaused == paused)
            {
                return;
            }

            IsPaused = paused;
        }
    }
}
