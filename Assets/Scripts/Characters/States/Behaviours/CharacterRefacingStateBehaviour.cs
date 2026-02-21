using MonsterTamer.Characters.Core;
using MonsterTamer.Utilities;
using UnityEngine;

namespace MonsterTamer.Characters.States.Behaviours
{
    /// <summary>
    /// Notifies the character controller when a refacing animation ends.
    /// </summary>
    internal sealed class CharacterRefacingStateBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.TryGetComponent<CharacterStateController>(out var controller))
            {
                Log.Warning(nameof(CharacterRefacingStateBehaviour), "Missing CharacterStateController.");
                return;
            }

            if (controller.CurrentState is CharacterRefacingState refacingState)
            {
                refacingState.OnRefacingComplete();
            }
        }
    }
}
