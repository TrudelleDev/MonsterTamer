using MonsterTamer.Shared.UI.Core;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Shared.UI.Navigation
{
    /// <summary>
    /// Manages grid-based navigation, supporting column layouts and skipping non-interactable buttons.
    /// </summary>
    internal sealed class GridMenuController : MenuController
    {
        [SerializeField, MinValue(1)]
        [Tooltip("Number of columns in this grid.")]
        private int columns = 2;

        private void Update()
        {
            int col = currentIndex % columns;

            if (ViewManager.Instance != null && ViewManager.Instance.IsTransitioning) return;

            if (Input.GetKeyDown(KeyBinds.Right) && col < columns - 1)
            {
                MoveRight();
            }
            else if (Input.GetKeyDown(KeyBinds.Left) && col > 0)
            {
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyBinds.Down) && currentIndex + columns < ItemCount)
            {
                MoveDown();
            }
            else if (Input.GetKeyDown(KeyBinds.Up) && currentIndex - columns >= 0)
            {
                MoveUp();
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

        private void MoveRight()
        {
            int index = currentIndex + 1;
            int rowEnd = (currentIndex / columns + 1) * columns;

            while (index < ItemCount && index < rowEnd)
            {
                if (buttons[index].IsInteractable)
                {
                    SelectButton(index);
                    return;
                }
                index++;
            }
        }

        private void MoveLeft()
        {
            int index = currentIndex - 1;
            int rowStart = (currentIndex / columns) * columns;

            while (index >= rowStart)
            {
                if (buttons[index].IsInteractable)
                {
                    SelectButton(index);
                    return;
                }
                index--;
            }
        }

        private void MoveDown()
        {
            int index = currentIndex + columns;

            while (index < ItemCount)
            {
                if (buttons[index].IsInteractable)
                {
                    SelectButton(index);
                    return;
                }
                index += columns;
            }
        }

        private void MoveUp()
        {
            int index = currentIndex - columns;

            while (index >= 0)
            {
                if (buttons[index].IsInteractable)
                {
                    SelectButton(index);
                    return;
                }
                index -= columns;
            }
        }
    }
}
