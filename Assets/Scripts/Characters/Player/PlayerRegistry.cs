using MonsterTamer.Characters.Core;
using MonsterTamer.Utilities;

namespace MonsterTamer.Characters.Player
{
    /// <summary>
    /// Holds a global reference to the active player character.
    /// </summary>
    internal static class PlayerRegistry
    {
        internal static Character Player { get; private set; }

        internal static void Register(Character player)
        {
            if (Player != null)
            {
                Log.Warning(nameof(PlayerRegistry), "Player already registered. Overwriting.");
            }
             
            Player = player;
        }

        internal static void Unregister(Character player)
        {
            if (Player == player)
            {
                Player = null;
            }               
        }
    }
}
