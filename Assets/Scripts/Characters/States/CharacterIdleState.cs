using MonsterTamer.Characters.Core;
using MonsterTamer.Characters.Directions;
using MonsterTamer.Characters.Interfaces;

namespace MonsterTamer.Characters.States
{
    /// <summary>
    /// Idle state: character stands still.
    /// Evaluates input and transitions to refacing, walking, or collision.
    /// </summary>
    internal sealed class CharacterIdleState : ICharacterState
    {
        private readonly CharacterStateController controller;

        internal CharacterIdleState(CharacterStateController controller) => this.controller = controller;

        public void Enter() => controller.AnimatorController.PlayIdle(controller.FacingDirection);

        public void Update()
        {
            InputDirection input = controller.Input.CurrentDirection;

            if (input == InputDirection.None)
            {
                return;
            }

            FacingDirection facing = input.ToFacingDirection();

            // Trigger interactions first
            if (controller.TriggerHandler?.TryTrigger(input.ToVector2Int()) == true)
            {
                return;
            }

            // Reface if facing differs
            if (controller.FacingDirection != facing)
            {
                controller.FacingDirection = facing;
                controller.SetState(controller.RefacingState);
                return;
            }

            // Move or collide
            controller.SetState(
                controller.TileMover.CanMoveInDirection(facing)
                    ? controller.WalkingState
                    : controller.CollisionState
            );
        }

        public void Exit() { }
    }
}
