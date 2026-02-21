using System;
using MonsterTamer.Shared.Interfaces;
using MonsterTamer.Shared.UI.Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MonsterTamer.Items.UI
{
    /// <summary>
    /// Defines the visual representation and interaction logic for a single item in the inventory UI.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class ItemUI : MonoBehaviour
    {
        [SerializeField, Required] private TextMeshProUGUI nameText;
        [SerializeField, Required] private TextMeshProUGUI quantityText;

        private MenuButton button;

        internal event Action<IDisplayable> ItemSelected;
        internal event Action<IDisplayable> ItemFocused;

        internal Item BoundItem { get; private set; }

        private void Awake() => button = GetComponent<MenuButton>();

        private void OnEnable()
        {
            button.Selected += OnItemSelected;
            button.Focused += OnItemFocused;
        }

        private void OnDisable()
        {
            button.Selected -= OnItemSelected;
            button.Focused -= OnItemFocused;
        }

        internal void Bind(Item item)
        {
            if (item == null || item.Definition == null)
            {
                Unbind();
                return;
            }

            BoundItem = item;
            nameText.text = item.Definition.DisplayName;
            quantityText.text = item.Quantity.ToString();
        }

        internal void Unbind()
        {
            BoundItem = null;
            nameText.text = string.Empty;
            quantityText.text = string.Empty;
        }

        private void OnItemSelected()
        {
            if (BoundItem?.Definition != null)
            {
                ItemSelected?.Invoke(BoundItem.Definition);
            }
        }

        private void OnItemFocused()
        {
            if (BoundItem?.Definition != null)
            {
                ItemFocused?.Invoke(BoundItem.Definition);
            }
        }
    }
}
