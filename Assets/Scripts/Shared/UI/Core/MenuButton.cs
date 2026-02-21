using System;
using TMPro;
using UnityEngine;

namespace MonsterTamer.Shared.UI.Core
{
    /// <summary>
    /// Provides the base logic for selectable menu elements, managing interaction states and visual updates.
    /// </summary>
    internal abstract class MenuButton : MonoBehaviour
    {
        [SerializeField, Tooltip("Whether this button starts as interactable.")]
        private bool interactable = true;

        [SerializeField, Tooltip("Optional Text label displayed on the button.")]
        private TextMeshProUGUI label;

        private bool lockSelectSprite;

        internal event Action Selected;
        internal event Action Focused;

        internal bool IsInteractable => interactable;
        internal bool IsFocused { get; private set; }

        internal bool LockSelectSprite
        {
            get => lockSelectSprite;
            set => SetLockSelectSprite(value);
        }

        internal void SetLabel(string text)
        {
            label.text = text;
        }

        internal void SetFocused(bool active)
        {
            if (IsFocused == active) return;

            IsFocused = active;
            UpdateVisualState();

            if (active)
            {
                Focused?.Invoke();
            }
        }

        internal void SetInteractable(bool value)
        {
            if (interactable == value) return;

            interactable = value;
            UpdateVisualState();
        }

        internal void SetLockSelectSprite(bool value)
        {
            if (lockSelectSprite == value) return;

            lockSelectSprite = value;
            UpdateVisualState();
        }

        internal void OnSelected()
        {
            if (IsInteractable)
            {
                Selected?.Invoke();
            }
        }

        protected abstract void UpdateVisualState();
    }
}