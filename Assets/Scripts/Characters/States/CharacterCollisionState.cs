using MonsterTamer.Audio;
using MonsterTamer.Characters.Core;
using MonsterTamer.Characters.Directions;
using MonsterTamer.Characters.Interfaces;

namespace MonsterTamer.Characters.States
{
    /// <summary>
    /// Handles collision when movement is blocked.
    /// </summary>
    internal sealed class CharacterCollisionState : ICharacterState
    {
        private readonly CharacterStateController controller;

        internal CharacterCollisionState(CharacterStateController controller) => this.controller = controller;

        public void Enter()
        {
            controller.AnimatorController.PlayCollisionStep();
            AudioManager.Instance.PlaySFX(controller.CollisionAudioClip);
        }

        public void Update() { }
        public void Exit() { }

        /// <summary>
        /// Called when the collision animation completes.
        /// </summary>
        public void OnCollisionComplete()
        {
            var input = controller.Input.CurrentDirection;

            if (input == InputDirection.None)
            {
                controller.SetState(controller.IdleState);
                return;
            }

            var facing = input.ToFacingDirection();
            controller.FacingDirection = facing;

            bool canMove = controller.TileMover.CanMoveInDirection(facing);

            controller.SetState(canMove
                ? controller.WalkingState
                : controller.CollisionState);
        }
    }
}
