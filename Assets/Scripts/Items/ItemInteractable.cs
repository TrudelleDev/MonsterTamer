using MonsterTamer.Audio;
using MonsterTamer.Characters.Core;
using MonsterTamer.Characters.Interfaces;
using MonsterTamer.Dialogue;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Items
{
    /// <summary>
    /// Interactable item pickup: grants an item stack to the interacting character.
    /// </summary>
    internal sealed class ItemInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField, Required] private Item item;
        [SerializeField, Required] private AudioClip receiveItemClip;

        private bool consumed;

        public void Interact(Character player)
        {
            if (consumed) return;
            consumed = true;

            // Build pickup message based on quantity
            string itemFoundLine = item.Quantity > 1
                ? $"You picked up {item.Quantity} × {item.Definition.DisplayName}!"
                : $"You picked up a {item.Definition.DisplayName}!";

            string putInBagLine = $"It's added to your inventory.";
            string fullDialogue = $"{itemFoundLine}\n{putInBagLine}";

            AudioManager.Instance.PlaySFX(receiveItemClip);
            DialogueBoxOverworld.Instance.Dialogue.DisplayWithInput(fullDialogue);

            player.Inventory.Add(item);

            // Remove item from overworld
            Destroy(gameObject);
        }
    }
}
