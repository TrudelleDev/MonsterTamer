using MonsterTamer.Shared.UI.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Shared.UI.MenuButtons
{
    /// <summary>
    ///  Updates a target image sprite based on the button's focus and interaction state.
    /// </summary>
    internal sealed class SpriteSwapMenuButton : MenuButton
    {
        [SerializeField, Required, Tooltip("The Image component to update.")]
        private Image targetImage;

        [Title("Sprites")]
        [SerializeField, Required, Tooltip("Normal state sprite.")]
        private Sprite normalSprite;

        [SerializeField, Required, Tooltip("Focused/Selected state sprite.")]
        private Sprite selectedSprite;

        [SerializeField, Required, Tooltip("Disabled state sprite.")]
        private Sprite disabledSprite;

        protected override void UpdateVisualState()
        {
            if (targetImage == null) return;

            // Priority 1: Disabled State
            if (!IsInteractable)
            {
                if (disabledSprite != null) targetImage.sprite = disabledSprite;
                return;
            }

            // Priority 2: Forced Selection/Focus State
            if (LockSelectSprite || IsFocused)
            {
                targetImage.sprite = selectedSprite;
                return;
            }

            // Priority 3: Default State
            targetImage.sprite = normalSprite;
        }
    }
}