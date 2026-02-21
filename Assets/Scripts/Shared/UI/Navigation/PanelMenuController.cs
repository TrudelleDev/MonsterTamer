using MonsterTamer.Shared.UI.Core;
using MonsterTamer.Views;
using UnityEngine;

namespace MonsterTamer.Shared.UI.Navigation
{
    /// <summary>
    /// Manages navigation between sibling UI panels.
    /// Inherits from MenuController to be compatible with View.menuController slot.
    /// </summary>
    internal sealed class PanelMenuController : MenuController
    {
        private void Update()
        {
            if (ViewManager.Instance != null && ViewManager.Instance.IsTransitioning) return;

            int childCount = transform.childCount;

            if (Input.GetKeyDown(KeyBinds.Right) && currentIndex < childCount - 1)
            {
                Navigate(1);
            }
            else if (Input.GetKeyDown(KeyBinds.Left) && currentIndex > 0)
            {
                Navigate(-1);
            }
        }

        private void Navigate(int direction)
        {
            previousIndex = currentIndex;
            currentIndex += direction;

            ApplySelection(previousIndex, currentIndex);
        }

        internal override void ResetToFirst()
        {
            // Ensure indices are reset
            currentIndex = 0;
            previousIndex = 0;

            // Visual reset: deactivate all children, then activate the first
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(i == 0);
            }
        }

        protected override void ApplySelection(int previous, int current)
        {
            int childCount = transform.childCount;

            // Hide previous panel
            if (previous >= 0 && previous < childCount)
            {
                transform.GetChild(previous).gameObject.SetActive(false);
            }

            // Show current panel
            if (current >= 0 && current < childCount)
            {
                transform.GetChild(current).gameObject.SetActive(true);
            }
        }
    }
}