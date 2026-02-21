using System;
using System.Collections.Generic;
using MonsterTamer.Items;
using MonsterTamer.Items.UI;
using MonsterTamer.Shared.Interfaces;
using MonsterTamer.Shared.UI.MenuButtons;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Inventory.UI
{
    /// <summary>
    /// Shows the player's inventory, raises events when items are focused or selected,
    /// and handles closing the view with the Back action.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class InventoryView : View
    {
        [Title("Inventory View Settings")]
        [SerializeField, Required] private ItemUI inventoryItemPrefab;
        [SerializeField, Required] private CancelMenuButton cancelButtonPrefab;
        [SerializeField, Required] private Transform itemsContainer;

        internal event Action<IDisplayable> OptionFocused;
        internal event Action<IDisplayable> OptionSelected;

        /// <summary>
        /// Populates the inventory UI with the given items.
        /// Automatically adds a Cancel button at the bottom.
        /// </summary>
        internal void PopulateItems(IReadOnlyList<Item> items)
        {
            ClearItems();

            foreach (Item item in items)
            {
                CreateItem(item);
            }

            CreateCancelButton();
            menuController.Rebuild();
        }

        private void CreateItem(Item item)
        {
            ItemUI itemUI = Instantiate(inventoryItemPrefab, itemsContainer);

            itemUI.Bind(item);
            itemUI.ItemFocused += OnOptionFocused;
            itemUI.ItemSelected += OnOptionSelected;
        }

        private void ClearItems()
        {
            for (int i = itemsContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(itemsContainer.GetChild(i).gameObject);
            }
        }

        private void CreateCancelButton()
        {
            CancelMenuButton button = Instantiate(cancelButtonPrefab, itemsContainer);

            button.transform.SetAsLastSibling();
            button.Focused += OnOptionFocused;
            button.Selected += OnBackRequested;
        }

        private void OnOptionSelected(IDisplayable menuOption) => OptionSelected?.Invoke(menuOption);
        private void OnOptionFocused(IDisplayable menuOption) => OptionFocused?.Invoke(menuOption);
        private void OnBackRequested() => CloseRequest(playSound: false);
    }
}