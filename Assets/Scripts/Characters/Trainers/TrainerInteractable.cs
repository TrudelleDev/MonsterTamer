using MonsterTamer.Battle;
using MonsterTamer.Characters.Core;
using MonsterTamer.Characters.Directions;
using MonsterTamer.Characters.Interfaces;
using MonsterTamer.Dialogue;
using MonsterTamer.Views;
using UnityEngine;

namespace MonsterTamer.Characters.Trainers
{
    /// <summary>
    /// Handles player interaction with a trainer, including dialogue and initiating battles.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class TrainerInteractable : MonoBehaviour, IInteractable
    {
        private Character player;
        private Character trainer;
        private CharacterStateController trainerStateController;
        private CharacterStateController playerStateController;

        internal bool HasBattled { get; private set; }

        private void Awake()
        {
            trainer = GetComponent<Character>();
            trainerStateController = GetComponent<CharacterStateController>();
        }

        /// <summary>
        /// Triggered when the player interacts with this trainer.
        /// Handles pre-battle dialogue, re-facing, and battle initiation.
        /// </summary>
        /// <param name="player">The player character interacting with the trainer.</param>
        public void Interact(Character player)
        {
            this.player = player;
            playerStateController = player.GetComponent<CharacterStateController>();
            trainerStateController.Reface(playerStateController.FacingDirection.Opposite());

            if (HasBattled)
            {
                DialogueBoxOverworld.Instance.Dialogue.DisplayWithInput(trainer.Definition.PostEventDialogue);
                return;
            }

            playerStateController?.LockMovement();

            // Show pre-battle dialogue
            var dialogue = DialogueBoxOverworld.Instance.Dialogue;
            dialogue.DialogueFinished += OnPreBattleDialogueFinished;
            DialogueBoxOverworld.Instance.Dialogue.DisplayWithInput(trainer.Definition.DefaultInteractionDialogue);
        }

        private void OnPreBattleDialogueFinished()
        {
            var dialogue = DialogueBoxOverworld.Instance.Dialogue;
            dialogue.DialogueFinished -= OnPreBattleDialogueFinished;

            BattleView battle = ViewManager.Instance.Show<BattleView>();
            battle.InitializeTrainerBattle(player, trainer);
            battle.OnBattleViewClose += OnBattleFinished;

            HasBattled = true;
        }

        private void OnBattleFinished()
        {
            playerStateController?.UnlockMovement();
        }
    }
}
