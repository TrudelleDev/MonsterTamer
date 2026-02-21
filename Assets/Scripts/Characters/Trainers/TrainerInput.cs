using MonsterTamer.Characters.Core;
using MonsterTamer.Characters.Directions;
using UnityEngine;

namespace MonsterTamer.Characters.Trainers
{
    /// <summary>
    /// Provides externally forced movement input for trainers (vision, scripts, cutscenes).
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class TrainerInput : CharacterInput
    {
        /// <summary>
        /// Direction forced by scripts or AI. Defaults to none.
        /// </summary>
        internal InputDirection ForcedDirection { get; set; } = InputDirection.None;

        protected override void ReadInput()
        {
            CurrentDirection = ForcedDirection;
        }
    }
}
