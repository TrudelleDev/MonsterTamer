using System.Collections;
using MonsterTamer.Audio;
using MonsterTamer.Characters.Core;
using MonsterTamer.Characters.Directions;
using MonsterTamer.Characters.Player;
using MonsterTamer.Raycasting;
using MonsterTamer.Tile;
using MonsterTamer.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Characters.Trainers
{
    /// <summary>
    /// Detects the player using line-of-sight and triggers the trainer challenge sequence.
    /// Handles exclamation icon, movement toward player, facing, and interaction.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TrainerInput), typeof(TrainerInteractable))]
    internal sealed class TrainerVision : MonoBehaviour
    {
        private const float ExclamationDuration = 0.5f;

        [SerializeField, Required]
        [Tooltip("Reference to the trainer's state controller to manage facing and movement.")]
        private CharacterStateController controller;

        [SerializeField, Required]
        [Tooltip("Sprite shown above the trainer when the player is spotted.")]
        private SpriteRenderer exclamationIcon;

        [SerializeField, Required]
        [Tooltip("Settings for raycasting used to detect the player.")]
        private RaycastSettings raycastSettings;

        [SerializeField, Tooltip("Maximum distance the trainer can see the player.")]
        private float viewDistance = 5f;

        [SerializeField, Required]
        [Tooltip("Audio clip played when the trainer spots the player.")]
        private AudioClip trainerTriggerClip;

        private readonly Vector2 visionBoxSize = new(0.9f, 0.9f);

        private TrainerInput trainerInput;
        private TrainerInteractable trainerInteractable;
        private TileMover playerTileMover;
        private bool spotted;

        private void Awake()
        {
            trainerInput = GetComponent<TrainerInput>();
            trainerInteractable = GetComponent<TrainerInteractable>();
        }

        private void OnEnable()
        {
            if (PlayerRegistry.Player == null) return;

            playerTileMover = PlayerRegistry.Player.GetComponent<TileMover>();
            playerTileMover.MoveCompleted += OnPlayerMoveCompleted;
        }

        private void OnDisable()
        {
            if (playerTileMover != null)
            {
                playerTileMover.MoveCompleted -= OnPlayerMoveCompleted;
            }
        }

        private void OnPlayerMoveCompleted()
        {
            if (spotted) return;

            if (TryDetectPlayer(out Character player))
            {
                var interactable = GetComponent<TrainerInteractable>();

                if (interactable != null && interactable.HasBattled) return;

                spotted = true;
                StartCoroutine(ChallengeSequence(player));
            }
        }

        private bool TryDetectPlayer(out Character player)
        {
            player = null;

            Vector2 origin = (Vector2)transform.position + raycastSettings.RaycastOffset;
            Vector2 direction = controller.FacingDirection.ToVector2Int();

            RaycastHit2D hit = Physics2D.BoxCast(origin, visionBoxSize, 0f, direction, viewDistance, raycastSettings.InteractionMask);

            if (hit.collider == null) return false;

            Character hitCharacter = hit.collider.GetComponent<Character>();

            if (hitCharacter != PlayerRegistry.Player) return false;

            player = hit.collider.GetComponent<Character>();

            return player != null;
        }

        private IEnumerator ChallengeSequence(Character player)
        {
            var playerController = player.GetComponent<CharacterStateController>();

            // Reset player to idle to avoid accidental movement this frame
            playerController.CancelToIdle();
            playerController.LockMovement();

            AudioManager.Instance.PlayBGM(trainerTriggerClip);

            // Show exclamation
            exclamationIcon.gameObject.SetActive(true);
            yield return new WaitForSeconds(ExclamationDuration);
            exclamationIcon.gameObject.SetActive(false);

            // Move toward player
            Vector2Int targetTile = Vector2Int.RoundToInt(player.transform.position) - controller.FacingDirection.ToVector2Int();
            InputDirection moveDir = controller.FacingDirection.ToInputDirection();

            while (Vector2Int.RoundToInt(transform.position) != targetTile && controller.TileMover.CanMoveInDirection(controller.FacingDirection))
            {
                trainerInput.ForcedDirection = moveDir;
                yield return null;
            }

            trainerInput.ForcedDirection = InputDirection.None;
            yield return new WaitUntil(() => !controller.TileMover.IsMoving);

            // Face player & trigger interaction
            playerController?.Reface(controller.FacingDirection.Opposite());
            trainerInteractable.Interact(player);
        }

        private void OnDrawGizmos()
        {
            if (!controller || raycastSettings == null) return;

            Vector2 origin = (Vector2)transform.position + raycastSettings.RaycastOffset;
            Vector2 direction = controller.FacingDirection.ToVector2Int();

            VisionGizmoDrawer.DrawBoxCast(origin, direction, viewDistance, visionBoxSize, spotted ? Color.red : Color.yellow);
        }
    }
}
