using MonsterTamer.Characters.Core;
using MonsterTamer.Characters.Interfaces;

namespace MonsterTamer.Characters.States
{
    /// <summary>
    /// Turns the character to a new facing without moving.
    /// Plays a refacing animation, then returns to idle.
    /// </summary>
    internal sealed class CharacterRefacingState : ICharacterState
    {
        private readonly CharacterStateController controller;

        internal CharacterRefacingState(CharacterStateController controller) => this.controller = controller;

        public void Enter()
        {
            if (controller.AnimatorController != null)
            {
                controller.AnimatorController.PlayRefacing(controller.FacingDirection);
            }
            else
            {
                controller.SetState(controller.IdleState);
            }
        }

        public void Update() { }

        public void Exit() { }

        internal void OnRefacingComplete()
        {
            controller.SetState(controller.IdleState);
        }
    }
}
