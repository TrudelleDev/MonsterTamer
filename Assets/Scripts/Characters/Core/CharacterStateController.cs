using MonsterTamer.Characters.Directions;
using MonsterTamer.Characters.Interfaces;
using MonsterTamer.Characters.States;
using MonsterTamer.Pause;
using MonsterTamer.Raycasting;
using MonsterTamer.Tile;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Characters.Core
{
    /// <summary>
    /// Base controller for all characters (Player, NPC).
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class CharacterStateController : MonoBehaviour
    {
        [SerializeField, Required]private FacingDirection startingDirection;
        [SerializeField, Required]private TileMover tileMover;
        [SerializeField, Required] private AnimationClip walkAnimationClip;
        [SerializeField, Required]private RaycastSettings raycastSettings;
        [SerializeField] private AudioClip collisionAudioClip;

        private FacingDirection facingDirection;
        private bool isLocked;

        internal AudioClip CollisionAudioClip => collisionAudioClip;
        internal float WalkDuration => walkAnimationClip.length;
        internal TileMover TileMover => tileMover;

        internal ICharacterState CurrentState { get; private set; }
        internal CharacterIdleState IdleState { get; private set; }
        internal CharacterRefacingState RefacingState { get; private set; }
        internal CharacterWalkingState WalkingState { get; private set; }
        internal CharacterCollisionState CollisionState { get; private set; }
        internal CharacterAnimatorController AnimatorController { get; private set; }
        internal CharacterTriggerHandler TriggerHandler { get; private set; }
        internal CharacterInteractionHandler InteractionHandler { get; private set; }
        internal CharacterInput Input { get; private set; }

        internal FacingDirection FacingDirection
        {
            get => facingDirection;
            set
            {
                facingDirection = value;
                AnimatorController?.UpdateDirection(facingDirection);
            }
        }

        private void Awake()
        {
            var character = GetComponent<Character>();
            var animator = GetComponent<Animator>();
            Input = GetComponent<CharacterInput>();

            AnimatorController = new CharacterAnimatorController(animator);
            TriggerHandler = new CharacterTriggerHandler(character, raycastSettings);
            InteractionHandler = new CharacterInteractionHandler(character, raycastSettings);

            IdleState = new CharacterIdleState(this);
            RefacingState = new CharacterRefacingState(this);
            WalkingState = new CharacterWalkingState(this);
            CollisionState = new CharacterCollisionState(this);
        }

        private void Start()
        {
            FacingDirection = startingDirection;
            SetState(IdleState);
        }

        private void Update()
        {
            if (isLocked || PauseManager.IsPaused)
            {
                if (CurrentState != IdleState)
                    SetState(IdleState);

                return;
            }

            InteractionHandler.Tick();
            CurrentState?.Update();
        }

        internal void SetState(ICharacterState newState)
        {
            if (newState == null) return;

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        internal void LockMovement() => isLocked = true;

        internal void UnlockMovement() => isLocked = false;

        internal void CancelToIdle() => SetState(IdleState);

        internal void Reface(FacingDirection newFacing)
        {
            if (FacingDirection != newFacing)
            {
                FacingDirection = newFacing;
                AnimatorController.PlayRefacing(newFacing);
            }
        }
    }
}
