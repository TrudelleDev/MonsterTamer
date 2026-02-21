using System;
using MonsterTamer.Audio;
using MonsterTamer.Shared.UI.Core;
using MonsterTamer.Transitions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Views
{
    /// <summary>
    /// Base class for UI views. Handles transitions, back input, and menu controller lifecycle.
    /// </summary>
    internal abstract class View : MonoBehaviour
    {
        [Title("Base View Settings")]
        [SerializeField, Required] private TransitionType openTransition = TransitionType.None;
        [SerializeField, Required] private TransitionType closeTransition = TransitionType.None;
        [SerializeField, Required] protected AudioClip closeSound;
        [SerializeField, Required] protected MenuController menuController;

        internal event Action BackRequested;

        private bool isFrozen;

        internal TransitionType OpenTransition => openTransition;
        internal TransitionType CloseTransition => closeTransition;

        /// <summary>
        /// Executed exactly once by the ViewManager during its initialization.
        /// Use this for setup that must occur before the view is ever opened.
        /// </summary>
        internal virtual void Preload() { }

        internal virtual void Show()
        {
            if (menuController != null)
            {
                menuController.ResetToFirst();
            }

            gameObject.SetActive(true);
        }

        internal virtual void Hide() => gameObject.SetActive(false);

        /// <summary>
        /// Disables input for this view.
        /// </summary>
        internal virtual void Freeze()
        {
            if (menuController != null)
            {
                menuController.enabled = false;
            }

            isFrozen = true;
        }

        /// <summary>
        /// Re-enables input for this view.
        /// </summary>
        internal virtual void Unfreeze()
        {
            if (menuController != null)
            {
                menuController.enabled = true;
            }

            isFrozen = false;
        }

        /// <summary>
        /// Requests the view to close and optionally plays a sound.
        /// </summary>
        internal void CloseRequest(bool playSound = true)
        {
            if (playSound)
            {
                AudioManager.Instance.PlayUISFX(closeSound);
            }

            BackRequested?.Invoke();
        }

        protected virtual void Update()
        {
            if (isFrozen || ViewManager.Instance == null || ViewManager.Instance.IsTransitioning)
                return;

            if (Input.GetKeyDown(KeyBinds.Back))
            {
                CloseRequest();
            }
        }
    }
}
