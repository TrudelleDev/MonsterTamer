using MonsterTamer.Shared.UI.Core;
using MonsterTamer.Views;
using UnityEngine;

namespace MonsterTamer.Shared.UI.Navigation
{
    /// <summary>
    /// Manages vertical list navigation, supporting automatic skipping of non-interactable buttons.
    /// </summary>
    internal sealed class VerticalMenuController : MenuController
    {
        private void Update()
        { 
            // Wait for UI
            if (ViewManager.Instance != null && ViewManager.Instance.IsTransitioning) return;

            if (Input.GetKeyDown(KeyBinds.Down))
            {
                SelectNext();
            }
            else if (Input.GetKeyDown(KeyBinds.Up))
            {
                SelectPrevious();
            }
            else if (Input.GetKeyDown(KeyBinds.Interact))
            {
                ConfirmCurrentButton();
            }
        }

        protected override void ApplySelection(int previousIndex, int currentIndex)
        {
            if (previousIndex >= 0 && previousIndex < ItemCount)
            {
                buttons[previousIndex].SetFocused(false);
            }

            if (currentIndex >= 0 && currentIndex < ItemCount)
            {
                buttons[currentIndex].SetFocused(true);
            }
        }

        private void SelectNext()
        {
            for (int i = currentIndex + 1; i < ItemCount; i++)
            {
                if (buttons[i].IsInteractable)
                {
                    SelectButton(i);
                    return;
                }
            }
        }

        private void SelectPrevious()
        {
            for (int i = currentIndex - 1; i >= 0; i--)
            {
                if (buttons[i].IsInteractable)
                {
                    SelectButton(i);
                    return;
                }
            }
        }
    }
}
