using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MonsterTamer.SceneManagement
{
    /// <summary>
    /// The entry point for the game, initializing core scenes and global systems.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class Bootstrap : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Scenes (Editor Only)")]

        [SerializeField, Required]
        [Tooltip("Main menu scene shown after boot.")]
        private SceneAsset mainMenuScene;

        [SerializeField, Required]
        [Tooltip("Optional transition scene (fade/black screen).")]
        private SceneAsset transitionScene;

        [SerializeField, Required]
        [Tooltip("Scene containing global audio manager and mixers.")]
        private SceneAsset audioScene;
#endif

        // Persisted scene names for runtime (these are serialized so they exist in builds)
        [SerializeField, HideInInspector]
        private string mainMenuSceneName;

        [SerializeField, HideInInspector]
        private string transitionSceneName;

        [SerializeField, HideInInspector]
        private string audioSceneName;


#if UNITY_EDITOR
        private void OnValidate()
        {
            mainMenuSceneName = mainMenuScene ? mainMenuScene.name : string.Empty;
            transitionSceneName = transitionScene ? transitionScene.name : string.Empty;
            audioSceneName = audioScene ? audioScene.name : string.Empty;
        }
#endif

        private async void Start()
        {
            // Initialization sequence
            await SceneLoader.LoadAdditiveAsync(audioSceneName);
            await SceneLoader.LoadAdditiveAsync(transitionSceneName);
            await SceneLoader.LoadAdditiveAsync(mainMenuSceneName);

            SceneLoader.SetActive(mainMenuSceneName);

            // Dispose of bootstrap scene once core is loaded
            await SceneLoader.UnloadAsync(gameObject.scene.name);
        }
    }
}
