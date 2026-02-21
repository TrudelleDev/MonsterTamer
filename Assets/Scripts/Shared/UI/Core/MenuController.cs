using System;
using System.Collections;
using System.Collections.Generic;
using MonsterTamer.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Shared.UI.Core
{
    /// <summary>
    /// Base class for menu controllers.
    /// Manages button focus, selection, and audio feedback for a collection of menu buttons.
    /// </summary>
    internal abstract class MenuController : MonoBehaviour
    {
        [SerializeField, Required] protected UIAudioSettings audioSettings;

        protected readonly List<MenuButton> buttons = new();
        protected int currentIndex;
        protected int previousIndex;

        internal event Action<MenuButton> Focused;
        internal event Action<MenuButton> Selected;

        internal MenuButton CurrentButton { get; private set; }
        protected int ItemCount => buttons.Count;

        private void Awake()
        {
            RefreshButtonList();
            StartCoroutine(InitializeFocus());
        }

        internal void Rebuild()
        {
            StartCoroutine(RebuildCoroutine());
        }

        private IEnumerator RebuildCoroutine()
        {
            yield return null;
            RefreshButtonList();

            if (buttons.Count > 0)
            {
                SetFocus(0);
            }
        }


        internal IEnumerator ResetToFirstCoroutine()
        {
            yield return null;

            if (buttons.Count > 0)
            {
                SetFocus(0);
            }
        }

        internal virtual void ResetToFirst()
        {
            if (buttons.Count > 0)
            {
                RefreshButtonList();
                SetFocus(0);
            }
        }

        protected abstract void ApplySelection(int previousIndex, int currentIndex);

        protected void SelectButton(int index)
        {
            SetFocus(index);
            PlaySelectSfx();
        }

        protected void ConfirmCurrentButton()
        {
            if (CurrentButton == null) return;

            CurrentButton.OnSelected();
            PlayConfirmSfx();
            Selected?.Invoke(CurrentButton);
        }

        private void SetFocus(int index)
        {
            if (index < 0 || index >= ItemCount) return;

            previousIndex = currentIndex;
            currentIndex = index;
            CurrentButton = buttons[currentIndex];

            ApplySelection(previousIndex, currentIndex);
            Focused?.Invoke(CurrentButton);
        }

        private IEnumerator InitializeFocus()
        {
            yield return null;
            SetFocus(0);
        }

        private void RefreshButtonList()
        {
            buttons.Clear();
            buttons.AddRange(GetComponentsInChildren<MenuButton>(true));
            buttons.RemoveAll(button => button == null); // extra safety
        }

        private void PlaySelectSfx()
        {
            if (audioSettings == null) return;

            AudioManager.Instance.PlayUISFX(audioSettings.SelectSfx);
        }

        private void PlayConfirmSfx()
        {
            if (audioSettings == null) return;

            AudioManager.Instance.PlayUISFX(audioSettings.ConfirmSfx);
        }
    }
}
