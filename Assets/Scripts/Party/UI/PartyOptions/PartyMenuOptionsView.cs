using System;
using MonsterTamer.Shared.UI.Core;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Party.UI.PartyOptions
{
    /// <summary>
    /// Displays party options and raises events when the player selects Info or Swap.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class PartyMenuOptionsView : View
    {
        [Title("Party Option View Settings")]
        [SerializeField, Required] private MenuButton infoButton;
        [SerializeField, Required] private MenuButton swapButton;

        internal event Action SwapRequested;
        internal event Action InfoRequested;

        private void OnEnable()
        {
            infoButton.Selected += OnInfoRequested;
            swapButton.Selected += OnSwapRequested;
        }

        private void OnDisable()
        {
            infoButton.Selected -= OnInfoRequested;
            swapButton.Selected -= OnSwapRequested;
        }

        private void OnInfoRequested() => InfoRequested?.Invoke();
        private void OnSwapRequested() => SwapRequested?.Invoke();
    }
}
