using MonsterTamer.Characters.Core;
using MonsterTamer.Utilities;
using UnityEngine;

namespace MonsterTamer.Characters.States.Behaviours
{
    /// <summary>
    /// Notifies the character controller when a collision animation ends.
    /// </summary>
    internal sealed class CharacterCollisionStateBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.TryGetComponent<CharacterStateController>(out var controller))
            {
                Log.Warning(nameof(CharacterCollisionStateBehaviour), "Missing CharacterStateController.");
                return;
            }

            if (controller.CurrentState is CharacterCollisionState collisionState)
            {
                collisionState.OnCollisionComplete();
            }
        }
    }
}
