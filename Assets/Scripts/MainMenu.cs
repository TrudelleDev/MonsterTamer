using UnityEngine;
using Sirenix.OdinInspector;
using MonsterTamer.SceneManagement;
using MonsterTamer.Shared.UI.Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MonsterTamer
{
    /// <summary>
    /// Binds main menu UI buttons. (New Game, Exit)
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GameStarter))]
    internal sealed class MainMenu : MonoBehaviour
    {
        [SerializeField, Required] private MenuButton newGameButton;
        [SerializeField, Required] private MenuButton exitButton;

        private GameStarter mainMenuLoader;

        private void Awake()
        {
            mainMenuLoader = GetComponent<GameStarter>();

            newGameButton.Selected += OnNewGame;
            exitButton.Selected += OnExit;
        }

        private void OnDestroy()
        {
            newGameButton.Selected -= OnNewGame;
            exitButton.Selected -= OnExit;
        }

        private void OnNewGame()
        {
            mainMenuLoader.StartNewGame();
        }

        private void OnExit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
