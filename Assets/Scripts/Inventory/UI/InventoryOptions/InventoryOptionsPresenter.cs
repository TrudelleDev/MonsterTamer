using System;
using System.Linq;
using MonsterTamer.Characters.Core;
using MonsterTamer.Dialogue;
using MonsterTamer.Items.Definitions;
using MonsterTamer.Items.Models;
using MonsterTamer.Monsters;
using MonsterTamer.Party.Enums;
using MonsterTamer.Party.UI;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Inventory.UI.InventoryOptions
{
    /// <summary>
    /// Handles item usage by responding to view input and coordinating with the party presenter.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class InventoryOptionsPresenter : MonoBehaviour
    {
        [SerializeField, Required] private InventoryOptionsView inventoryOptionsView;
        [SerializeField, Required] private PartyMenuPresenter partyMenuPresenter;
        [SerializeField, Required] private Character player;

        private ItemDefinition currentItem;
        private bool lastItemUseSucceeded;

        internal event Action<bool> ItemUsed;

        private void OnEnable()
        {
            inventoryOptionsView.UseRequested += OnUseRequested;
            inventoryOptionsView.BackRequested += OnBackRequested;
        }

        private void OnDisable()
        {
            inventoryOptionsView.UseRequested -= OnUseRequested;
            inventoryOptionsView.BackRequested -= OnBackRequested;

            partyMenuPresenter.ItemTargetRequested -= OnItemTargetSelected;
        }

        internal void Initialize(ItemDefinition item) => currentItem = item;

        private void OnUseRequested()
        {
            if (currentItem == null) return;

            // Start item selection flow
            partyMenuPresenter.SetMode(PartySelectionMode.UseItem);
            partyMenuPresenter.ItemTargetRequested += OnItemTargetSelected;

            ViewManager.Instance.Show<PartyMenuView>();
        }

        private void OnItemTargetSelected(Monster monster)
        {
            // Unsubscribe immediately
            partyMenuPresenter.ItemTargetRequested -= OnItemTargetSelected;

            var itemInstance = player.Inventory.Items.FirstOrDefault(i => i.Definition == currentItem);
            if (itemInstance == null) return;

            // Execute item logic
            ItemUseResult result = currentItem.Use(monster);
            lastItemUseSucceeded = result.IsUsed;

            if (result.IsUsed)
            {
                player.Inventory.Remove(itemInstance);
            }

            // Display results
            DialogueBoxOverworld.Instance.Dialogue.DisplayWithInput(result.Messages);
            DialogueBoxOverworld.Instance.Dialogue.DialogueFinished += OnDialogueFinished;
        }

        private void OnDialogueFinished()
        {
            DialogueBoxOverworld.Instance.Dialogue.DialogueFinished -= OnDialogueFinished;

            ViewManager.Instance.Close<PartyMenuView>();
            ItemUsed?.Invoke(lastItemUseSucceeded);
        }

        private void OnBackRequested() => ViewManager.Instance.Close<InventoryOptionsView>();
    }
}