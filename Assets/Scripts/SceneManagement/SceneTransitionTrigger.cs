using MonsterTamer.Audio;
using MonsterTamer.Characters.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using MonsterTamer.Transitions;
using MonsterTamer.Characters.Directions;
using MonsterTamer.Characters.Core;
using MonsterTamer.Map;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MonsterTamer.SceneManagement
{
    /// <summary>
    /// Executes a scene transition when a character interacts with this object while facing the required direction.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class SceneTransitionTrigger : MonoBehaviour, ITriggerable
    {
#if UNITY_EDITOR
        [Title("Scenes (Editor Only)")]
        [SerializeField, Required, Tooltip("Scene to load on trigger.")]
        private SceneAsset targetScene;
#endif

        [Title("Requirements")]
        [SerializeField, Required, Tooltip("Player must face this direction to activate the trigger.")]
        private FacingDirection requiredFacing;

        [Title("Spawn Location")]
        [SerializeField, Required, Tooltip("Spawn location ID in the target scene.")]
        private MapEntryID spawnLocationId;

        [Title("Transition")]
        [SerializeField, Required, Tooltip("Transition effect for scene change.")]
        private TransitionType sceneTransition;

        [Title("Audio")]
        [SerializeField, Tooltip("Optional sound effect played during activation.")]
        private AudioClip transitionSfx;

        [SerializeField, HideInInspector] private string targetSceneName;

#if UNITY_EDITOR
        private void OnValidate()
        {
            targetSceneName = targetScene ? targetScene.name : string.Empty;
        }
#endif

        public void Trigger(Character character)
        {
            var transitionController = SceneTransitionManager.Instance;

            if (transitionController == null || transitionController.IsTransitioning)
            {
                return;
            }

            // Ensure player is facing the transition exit (e.g., facing 'Up' to exit via a northern door)
            if (character.StateController.FacingDirection != requiredFacing)
            {
                character.StateController.Reface(requiredFacing);
                return;
            }

            if (transitionSfx != null)
            {
                AudioManager.Instance.PlaySFX(transitionSfx);
            }

            transitionController.StartTransition(new[] { targetSceneName }, spawnLocationId, sceneTransition);
        }
    }
}