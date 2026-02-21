using System.Threading.Tasks;
using MonsterTamer.Map;
using MonsterTamer.Pause;
using MonsterTamer.Transitions;
using MonsterTamer.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterTamer.SceneManagement
{
    /// <summary>
    /// Coordinates the sequence of fading, scene loading, and player relocation during gameplay transitions.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class SceneTransitionManager : Singleton<SceneTransitionManager>
    {
        [SerializeField] private float blackScreenHoldDuration = 1f;

        internal bool IsTransitioning { get; private set; }

        internal void StartTransition(string[] scenesToLoad, MapEntryID spawnLocationId, TransitionType transitionType)
        {
            if (IsTransitioning)
            {
                return;
            }

            if (scenesToLoad == null || scenesToLoad.Length == 0)
            {
                Log.Warning(nameof(SceneTransitionManager), "No scenes provided for transition.");
                return;
            }

            IsTransitioning = true;
            Transition transition = TransitionLibrary.Instance.Resolve(transitionType);

            // Fire and forget the async task
            _ = RunTransitionMulti(scenesToLoad, spawnLocationId, transition);
        }

        private async Task RunTransitionMulti(string[] scenesToLoad, MapEntryID spawnLocationId, Transition transition)
        {
            Scene sourceScene = SceneManager.GetActiveScene();

            PauseManager.SetPaused(true);
            MapEntryRegistry.SetNextEntry(spawnLocationId);

            if (transition != null)
            {
                await transition.FadeInAsync();
            }

            foreach (string sceneName in scenesToLoad)
            {
                await SceneLoader.LoadAdditiveAsync(sceneName);
            }

            // Target the final scene in the array as the active environment
            string lastSceneName = scenesToLoad[scenesToLoad.Length - 1];
            SceneLoader.SetActive(lastSceneName);

            await SceneLoader.UnloadAsync(sourceScene.name);

            if (transition != null)
            {
                // Ensure a minimum hold time for visual polish/loading buffer
                await Task.Delay((int)(blackScreenHoldDuration * 1000));
                await transition.FadeOutAsync();
            }

            IsTransitioning = false;
            PauseManager.SetPaused(false);
        }
    }
}