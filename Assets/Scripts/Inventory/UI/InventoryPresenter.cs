using System;
using MonsterTamer.Characters.Core;
using MonsterTamer.Inventory.UI.InventoryOptions;
using MonsterTamer.Items.Definitions;
using MonsterTamer.Shared.Interfaces;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Inventory.UI
{
    /// <summary>
    /// Handles inventory view events, updates the item detail panel,
    /// and coordinates item usage and view navigation.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class InventoryPresenter : MonoBehaviour
    {
        [SerializeField, Required] private InventoryView inventoryView;
        [SerializeField, Required] private InventoryItemDetailPanel itemDetailPanel;
        [SerializeField, Required] private Character player;

        internal event Action<bool> ItemUsed;

        private void OnEnable()
        {
            inventoryView.OptionSelected += OnItemSelected;
            inventoryView.OptionFocused += OnItemFocused;
            inventoryView.BackRequested += OnBackRequested;

            player.Inventory.ItemsChanged += OnItemChanged;
            OnItemChanged();
        }

        private void OnDisable()
        {
            inventoryView.OptionSelected -= OnItemSelected;
            inventoryView.OptionFocused -= OnItemFocused;
            inventoryView.BackRequested -= OnBackRequested;

            player.Inventory.ItemsChanged -= OnItemChanged;
        }

        private void OnItemFocused(IDisplayable displayable)
        {
            if (displayable == null)
            {
                itemDetailPanel.Unbind();
            }
            else
            {
                itemDetailPanel.Bind(displayable);
            }
        }

        private void OnItemSelected(IDisplayable displayable)
        {
            if (displayable is not ItemDefinition itemDefinition)
            {
                return;
            }

            // Show the options view (UI)
            var optionsView = ViewManager.Instance.Show<InventoryOptionsView>();

            if (optionsView.TryGetComponent<InventoryOptionsPresenter>(out var optionsPresenter))
            {
                optionsPresenter.Initialize(itemDefinition);

                // Subscribe to item used event
                optionsPresenter.ItemUsed += result =>
                {
                    ItemUsed?.Invoke(result);
                };
            }
        }

        private void OnItemChanged() => inventoryView.PopulateItems(player.Inventory.Items);
        private void OnBackRequested() => ViewManager.Instance.Close<InventoryView>();
    }
}
