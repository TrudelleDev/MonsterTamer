using System;
using MonsterTamer.Shared.UI.Core;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Inventory.UI.InventoryOptions
{
    /// <summary>
    /// Inventory options view that raises user input events.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class InventoryOptionsView : View
    {
        [Title("Inventory Option View Settings")]
        [SerializeField, Required] private MenuButton useButton;
        [SerializeField, Required] private MenuButton backButton;

        internal event Action UseRequested;

        private void OnEnable()
        {
            useButton.Selected += OnUseRequested;
            backButton.Selected += OnBackRequested;
        }

        private void OnDisable()
        {
            useButton.Selected -= OnUseRequested;
            backButton.Selected -= OnBackRequested;
        }

        private void OnUseRequested() => UseRequested?.Invoke();
        private void OnBackRequested() => CloseRequest(playSound: false);
    }
}
