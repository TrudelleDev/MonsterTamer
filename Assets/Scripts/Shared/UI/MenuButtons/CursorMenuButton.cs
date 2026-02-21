using MonsterTamer.Shared.UI.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Shared.UI.MenuButtons
{
    /// <summary>
    /// A menu button that displays a cursor arrow sprite.
    /// </summary>
    internal sealed class CursorMenuButton : MenuButton
    {
        [SerializeField, Required] private Image cursorArrow;

        private void Awake() => cursorArrow.enabled = false;

        protected override void UpdateVisualState()
        {
            if (cursorArrow == null)
            {
                return;
            }

            // Only show the arrow if interactable AND selected
            cursorArrow.enabled = IsInteractable && IsFocused;
        }
    }
}