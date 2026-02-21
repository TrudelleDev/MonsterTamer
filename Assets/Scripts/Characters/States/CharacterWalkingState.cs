using System.Collections;
using MonsterTamer.Characters.Core;
using MonsterTamer.Characters.Directions;
using MonsterTamer.Characters.Interfaces;
using MonsterTamer.Map;
using MonsterTamer.Utilities;
using UnityEngine;

namespace MonsterTamer.Characters.States
{
    /// <summary>
    /// Walking state: moves the character one tile in the current facing direction.
    /// Checks triggers, collisions, and executes movement via coroutine.
    /// </summary>
    internal sealed class CharacterWalkingState : ICharacterState
    {
        private readonly CharacterStateController controller;
        private Coroutine walkRoutine;

        internal CharacterWalkingState(CharacterStateController controller) => this.controller = controller;

        public void Enter()
        {
            var input = controller.Input.CurrentDirection;
            if (input == InputDirection.None)
            {
                controller.SetState(controller.IdleState);
                return;
            }

            var facing = input.ToFacingDirection();
            controller.FacingDirection = facing;

            // Check triggers
            if (controller.TriggerHandler?.TryTrigger(input.ToVector2Int()) == true)
            {
                controller.SetState(controller.IdleState);
                return;
            }

            // Check collisions
            if (!controller.TileMover.CanMoveInDirection(facing))
            {
                controller.SetState(controller.CollisionState);
                return;
            }

            // Start walking coroutine
            walkRoutine = controller.StartCoroutine(Walk(facing));
        }

        public void Update() { }

        public void Exit()
        {
            if (walkRoutine != null)
            {
                controller.StopCoroutine(walkRoutine);
                walkRoutine = null;
            }
        }

        private IEnumerator Walk(FacingDirection direction)
        {
            var move = direction.ToVector2Int();
            var destination = controller.transform.position +
                              new Vector3(move.x * MapInfo.CellSize.x, move.y * MapInfo.CellSize.y, 0f);

            controller.AnimatorController.UpdateDirection(direction);
            controller.AnimatorController.PlayWalkStep();

            yield return controller.TileMover.MoveToTile(destination, controller.WalkDuration);

            // Decide next state after movement
            controller.SetState(controller.Input.CurrentDirection == InputDirection.None
                ? controller.IdleState
                : controller.WalkingState);
        }
    }
}
