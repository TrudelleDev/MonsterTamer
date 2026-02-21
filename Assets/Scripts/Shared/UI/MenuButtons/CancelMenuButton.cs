using System;
using MonsterTamer.Shared.Interfaces;
using MonsterTamer.Shared.UI.Core;
using MonsterTamer.Shared.UI.Definitions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Shared.UI.MenuButtons
{
    /// <summary>
    /// Represents a cancel option in a menu.
    /// Behaves like a regular selectable entry while exposing
    /// display data for detail panels.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MenuButton))]
    internal sealed class CancelMenuButton : MonoBehaviour, IDisplayable
    {
        [SerializeField, Required]
        private CancelMenuOptionDefinition definition;

        private MenuButton button;

        public string DisplayName => definition.DisplayName;
        public string Description => definition.Description;
        public Sprite Icon => definition.Icon;

        internal event Action<IDisplayable> Focused;
        internal event Action Selected;

        private void Awake() => button = GetComponent<MenuButton>();

        private void OnEnable()
        {
            button.Focused += OnFocused;
            button.Selected += OnSelected;
        }

        private void OnDisable()
        {
            button.Focused -= OnFocused;
            button.Selected -= OnSelected;
        }

        private void OnFocused() => Focused?.Invoke(this);
        private void OnSelected() => Selected?.Invoke();
    }
}
