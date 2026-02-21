using UnityEngine;

namespace MonsterTamer
{
    /// <summary>
    /// Defines global key bindings for player input.
    /// </summary>
    internal static class KeyBinds
    {
        // Movement
        internal const KeyCode Up = KeyCode.W;
        internal const KeyCode Down = KeyCode.S;
        internal const KeyCode Left = KeyCode.A;
        internal const KeyCode Right = KeyCode.D;

        // Actions
        internal const KeyCode Interact = KeyCode.Z;
        internal const KeyCode Back = KeyCode.X;
        internal const KeyCode Menu = KeyCode.Return;
    }
}
