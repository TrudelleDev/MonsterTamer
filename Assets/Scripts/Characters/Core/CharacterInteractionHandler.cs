using MonsterTamer.Characters.Directions;
using MonsterTamer.Characters.Interfaces;
using MonsterTamer.Pause;
using MonsterTamer.Raycasting;
using UnityEngine;

namespace MonsterTamer.Characters.Core
{
    /// <summary>
    /// Handles character interactions with objects in front of them (NPCs, items).
    /// </summary>
    internal sealed class CharacterInteractionHandler
    {
        private readonly Character character;
        private readonly RaycastSettings raycastSettings;
        private readonly Transform transform;

        internal CharacterInteractionHandler(Character character, RaycastSettings raycastSettings)
        {
            this.character = character;
            this.raycastSettings = raycastSettings;
            this.transform = character.transform;
        }

        /// <summary>
        /// Called each frame to check for interaction input and trigger interactables.
        /// </summary>
        internal void Tick()
        {
            if (PauseManager.IsPaused || !character.StateController.Input.InteractPressed)
                return;

            Vector2 direction = character.StateController.FacingDirection.ToVector2Int();

            if (TryInteract(direction))
            {
                character.StateController.CancelToIdle();
            }
        }

        private bool TryInteract(Vector2 direction)
        {
            bool interacted = false;

            RaycastUtility.RaycastAndCall<IInteractable>(direction, raycastSettings, transform, interactable =>
            {
                interactable.Interact(character);
                interacted = true;
                return false; // continue checking other interactables
            });

            return interacted;
        }
    }
}
