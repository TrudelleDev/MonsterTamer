using Sirenix.OdinInspector;
using UnityEngine;
using MonsterTamer.Transitions;
using MonsterTamer.Map;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MonsterTamer.SceneManagement
{
    /// <summary>
    /// Manages the transition sequence from the main menu into a new or continued game session.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class GameStarter : MonoBehaviour
    {
#if UNITY_EDITOR
        [Title("Scenes (Editor Only)")]
        [SerializeField, Required, Tooltip("Gameplay core systems scene.")]
        private SceneAsset coreScene;

        [SerializeField, Required, Tooltip("Initial map scene to enter on New Game.")]
        private SceneAsset initialMapScene;
#endif

        [Title("Gameplay Settings")]
        [SerializeField, Required, Tooltip("Map entry where the player spawns on New Game.")]
        private MapEntryID spawnLocationId;

        [SerializeField, Required, Tooltip("Transition animation used when entering gameplay.")]
        private TransitionType startGameTransition;

        [SerializeField, HideInInspector] private string coreSceneName;
        [SerializeField, HideInInspector] private string initialMapSceneName;

#if UNITY_EDITOR
        private void OnValidate()
        {
            coreSceneName = coreScene ? coreScene.name : string.Empty;
            initialMapSceneName = initialMapScene ? initialMapScene.name : string.Empty;
        }
#endif

        internal void StartNewGame()
        {
            string[] sceneNames = { coreSceneName, initialMapSceneName };
            SceneTransitionManager.Instance.StartTransition(sceneNames, spawnLocationId, startGameTransition);
        }
    }
}