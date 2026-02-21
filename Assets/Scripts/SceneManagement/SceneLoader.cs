using System.Threading.Tasks;
using MonsterTamer.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterTamer.SceneManagement
{
    /// <summary>
    /// Provides asynchronous wrappers and safety checks for Unity's SceneManager.
    /// </summary>
    [DisallowMultipleComponent]
    internal static class SceneLoader
    {   
        internal static async Task LoadAdditiveAsync(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Log.Warning(nameof(SceneLoader), "LoadAdditiveAsync called with null or empty scene name.");
                return;
            }

            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                return;
            }

            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!loadSceneOperation.isDone)
            {
                await Task.Yield();
            }
        }

        internal static async Task UnloadAsync(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Log.Warning(nameof(SceneLoader), "UnloadAsync called with null or empty scene name.");
                return;
            }

            if (!SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                return;
            }

            AsyncOperation unloadSceneOperation = SceneManager.UnloadSceneAsync(sceneName);

            while (!unloadSceneOperation.isDone)
            {
                await Task.Yield();
            }
        }

        internal static void SetActive(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Log.Warning(nameof(SceneLoader), "SetActive called with null or empty scene name.");
                return;
            }

            Scene scene = SceneManager.GetSceneByName(sceneName);

            if (scene.IsValid() && scene.isLoaded)
            {
                SceneManager.SetActiveScene(scene);
            }
            else
            {
                Log.Warning(nameof(SceneLoader), $"Cannot set active: {sceneName} not loaded.");
            }
        }
    }
}
