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

        private TrainerVision vision;

        internal bool HasBattled { get; private set; }

        private void Awake()
        {
            trainer = GetComponent<Character>();
            trainerStateController = GetComponent<CharacterStateController>();
            vision = GetComponent<TrainerVision>();
        }

        /// <summary>
        /// Triggered when the player interacts with the trainer.
        /// </summary>
        public void Interact(Character player)
        {
            this.player = player;
            playerStateController = player.GetComponent<CharacterStateController>();

            FacePlayer();

            if (TryTriggerVision()) return;

            if (HasBattled)
            {
                ShowPostBattleDialogue();
                return;
            }

            StartPreBattleDialogue();
        }

        private void FacePlayer()
        {
            trainerStateController.Reface(playerStateController.FacingDirection.Opposite());
        }

        private bool TryTriggerVision()
        {
            if (HasBattled) return false;
            if (vision == null || vision.IsSpotted) return false;

            vision.TriggerManualChallenge(player);
            return true;
        }

        private void ShowPostBattleDialogue()
        {
            var dialogueView = ViewManager.Instance.Get<DialogueView>();
            dialogueView.ShowConversational(trainer.Definition.PostEventDialogue);
        }

        private void StartPreBattleDialogue()
        {
            playerStateController.LockMovement();

            var dialogueView = ViewManager.Instance.Get<DialogueView>();
            dialogueView.DialogueFinished += OnPreBattleDialogueFinished;
            dialogueView.ShowConversational(trainer.Definition.DefaultInteractionDialogue);
        }

        private void OnPreBattleDialogueFinished()
        {
            var dialogueView = ViewManager.Instance.Get<DialogueView>();
            dialogueView.DialogueFinished -= OnPreBattleDialogueFinished;

            BattleView battle = ViewManager.Instance.Show<BattleView>();
            battle.BattleViewClose += OnBattleFinished;
            battle.InitializeTrainerBattle(player, trainer);

            HasBattled = true;
        }

        private void OnBattleFinished() => playerStateController.UnlockMovement();
    }
}