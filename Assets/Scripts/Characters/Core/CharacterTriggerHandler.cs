using MonsterTamer.Characters.Interfaces;
using MonsterTamer.Raycasting;
using UnityEngine;

namespace MonsterTamer.Characters.Core
{
    /// <summary>
    /// Handles detection and activation of triggers in front of a character,
    /// such as warp points, cutscenes, or scripted events.
    /// </summary>
    internal sealed class CharacterTriggerHandler
    {
        private readonly Character character;
        private readonly RaycastSettings raycastSettings;
        private readonly Transform transform;

        internal CharacterTriggerHandler(Character character, RaycastSettings raycastSettings)
        {
            this.character = character;
            this.raycastSettings = raycastSettings;
            this.transform = character.transform;
        }

        /// <summary>
        /// Attempts to trigger all triggerables in the given direction.
        /// </summary>
        internal bool TryTrigger(Vector2 direction)
        {
            bool triggered = false;

            RaycastUtility.RaycastAndCall<ITriggerable>(direction, raycastSettings, transform, triggerable =>
            {
                triggerable.Trigger(character);
                triggered = true;
                return false; // continue checking other triggers
            });

            return triggered;
        }
    }
}
