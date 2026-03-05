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

            // Get the reference without "Showing" it yet
            var dialogueView = ViewManager.Instance.Get<DialogueView>();

            if (HasBattled)
            {
                // This method now handles its own Open/Close internally
                dialogueView.ShowConversational(trainer.Definition.PostEventDialogue);
                return;
            }

            playerStateController?.LockMovement();

            // Setup the battle trigger
            dialogueView.DialogueFinished += OnPreBattleDialogueFinished;

            // We use the "ShowInteractive" because it auto-closes, 
            // which is exactly what we want right before a battle transition!
            dialogueView.ShowConversational(trainer.Definition.DefaultInteractionDialogue);
        }

        private void OnPreBattleDialogueFinished()
        {
            var dialogueView = ViewManager.Instance.Get<DialogueView>();
            dialogueView.DialogueFinished -= OnPreBattleDialogueFinished;

            BattleView battle = ViewManager.Instance.Show<BattleView>();
            battle.InitializeTrainerBattle(player, trainer);
            battle.BattleViewClose += OnBattleFinished;

            HasBattled = true;
        }

        private void OnBattleFinished()
        {
          //  ViewManager.Instance.InstantClose<DialogueView>();
            playerStateController?.UnlockMovement();
        }
    }
}
