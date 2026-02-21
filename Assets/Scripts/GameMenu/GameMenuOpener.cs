using MonsterTamer.Audio;
using MonsterTamer.Characters.Core;
using MonsterTamer.GameMenu;
using MonsterTamer.Pause;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer
{
    /// <summary>
    /// Opens the game menu when input is received and no other views are active.
    /// </summary>
    internal sealed class GameMenuOpener : MonoBehaviour
    {
        [SerializeField, Required] private CharacterStateController playerStateController;
        [SerializeField, Required] private AudioClip openSound;

        private void Update()
        {
            if (PauseManager.IsPaused || playerStateController.TileMover.IsMoving) return;

            if (Input.GetKeyDown(KeyBinds.Menu))
            {
                TryOpenMenu();
            }
        }

        private void TryOpenMenu()
        {
            if (!ViewManager.Instance.HasActiveView)
            {
                AudioManager.Instance.PlayUISFX(openSound);
                ViewManager.Instance.Show<GameMenuView>();
            }
        }
    }
}
