using MonsterTamer.Characters.Core;
using MonsterTamer.Characters.Directions;
using UnityEngine;

namespace MonsterTamer.Characters.Player
{
    /// <summary>
    /// Reads player input for movement and interaction.
    /// Supports WASD or arrow keys.
    /// </summary>
    internal sealed class PlayerInput : CharacterInput
    {
        protected override void ReadInput()
        {
            CurrentDirection = GetMovementDirection();
            InteractPressed = Input.GetKeyDown(KeyBinds.Interact);
        }

        private InputDirection GetMovementDirection()
        {
            if (Input.GetKey(KeyBinds.Up))
            {
                return InputDirection.Up;
            }

            if (Input.GetKey(KeyBinds.Down))
            {
                return InputDirection.Down;
            }

            if (Input.GetKey(KeyBinds.Left))
            {
                return InputDirection.Left;
            }

            if (Input.GetKey(KeyBinds.Right))
            {
                return InputDirection.Right;
            }

            return InputDirection.None;
        }
    }
}
