using MonsterTamer.Shared.Interfaces;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Inventory.UI
{
    /// <summary>
    /// Displays detailed information for a selected inventory item.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class InventoryItemDetailPanel : MonoBehaviour
    {
        [SerializeField, Required] private TextMeshProUGUI descriptionText;
        [SerializeField, Required] private Image iconImage;

        internal void Bind(IDisplayable item)
        {
            if (item == null)
            {
                Unbind();
                return;
            }

            descriptionText.text = item.Description;
            iconImage.sprite = item.Icon;
            iconImage.enabled = true;
        }

        internal void Unbind()
        {
            descriptionText.text = string.Empty;
            iconImage.enabled = false;
        }
    }
}
