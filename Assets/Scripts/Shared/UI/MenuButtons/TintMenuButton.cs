using MonsterTamer.Shared.UI.Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Shared.UI.MenuButtons
{
    /// <summary>
    /// Tints text and graphic components based on the button's focus and interaction state.
    /// </summary>
    internal sealed class TintMenuButton : MenuButton
    {
        [Title("Targets")]
        [SerializeField, Tooltip("Text to tint when focused.")]
        private TextMeshProUGUI targetText;

        [SerializeField, Tooltip("Image to tint when focused.")]
        private Graphic targetImage;

        [Title("Colors")]
        [SerializeField, ColorUsage(false), Tooltip("Default state color.")]
        private Color normalColor = Color.white;

        [SerializeField, ColorUsage(false), Tooltip("Focused state color.")]
        private Color highlightedColor = Color.white;

        [SerializeField, ColorUsage(false), Tooltip("Disabled state color.")]
        private Color disabledColor = Color.white;

        private void Awake() => ApplyColor(normalColor);

        protected override void UpdateVisualState()
        {
            if (!IsInteractable)
            {
                ApplyColor(disabledColor);
                return;
            }

            ApplyColor(IsFocused ? highlightedColor : normalColor);
        }

        private void ApplyColor(Color color)
        {
            if (targetText != null) targetText.color = color;
            if (targetImage != null) targetImage.color = color;
        }
    }
}