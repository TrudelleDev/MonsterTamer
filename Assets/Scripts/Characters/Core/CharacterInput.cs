using MonsterTamer.Characters.Directions;
using UnityEngine;

namespace MonsterTamer.Characters.Core
{
    /// <summary>
    /// Base class for handling character input.
    /// </summary>
    internal abstract class CharacterInput : MonoBehaviour
    {
        internal InputDirection CurrentDirection { get; set; }
        internal bool InteractPressed { get; set; }

        protected virtual void Update() => ReadInput();

        /// <summary>
        /// Reads input values each frame. Must be implemented by subclasses.
        /// </summary>

        protected abstract void ReadInput();
    }
}
