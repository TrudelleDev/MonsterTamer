using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonsterTamer.Pause;
using MonsterTamer.Transitions;
using MonsterTamer.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Views
{
    /// <summary>
    /// Manages a stacked overlay system for View instances.
    /// </summary>
    internal sealed class ViewManager : Singleton<ViewManager>
    {
        [SerializeField] private float blackScreenHoldDuration = 1f;
        [SerializeField] private bool enableDebugLogs = false;
        [SerializeField, Required] private View[] views;

        private readonly List<View> activeViews = new(); // Stack of active overlay views (bottom → top)

        internal bool IsTransitioning { get; private set; }
        internal View CurrentView => activeViews.Count > 0 ? activeViews[^1] : null;
        internal bool HasActiveView => activeViews.Count > 0;

        protected override void Awake()
        {
            base.Awake();

            foreach (View view in views)
            {
                view.Preload();

                if (view.gameObject.activeInHierarchy)
                {
                    view.Hide();
                }
            }
        }

        /// <summary>
        /// Finds and returns the instance of the specified view type from the registry.
        /// </summary>
        internal T Get<T>() where T : View
        {
            foreach (View view in views)
            {
                if (view is T target)
                {
                    return target;
                }
            }

            return null;
        }

        /// <summary>
        /// Opens the specified view as the new top-most overlay and freezes the current view.
        /// </summary>
        internal T Show<T>() where T : View
        {
            if (IsTransitioning) return null;

            foreach (View view in views)
            {
                if (view is not T target)
                {
                    continue;
                }

                View previous = CurrentView;

                if (previous != null)
                {
                    previous.Freeze();
                }

                StartCoroutine(ShowAsOverlay(target));
                return target;
            }

            return null;
        }

        /// <summary>
        /// Closes the specified view and restores focus to the view immediately below it in the stack.
        /// </summary>
        internal void Close<T>() where T : View
        {
            if (IsTransitioning) return;

            // Find the instance in active views
            View target = activeViews.FirstOrDefault(v => v is T);

            if (target != null)
            {
                StartCoroutine(CloseSpecificViewCoroutine(target));
            }        
        }


        private IEnumerator CloseSpecificViewCoroutine(View target)
        {
            IsTransitioning = true;
            UpdatePauseState();

            Transition transition = TransitionLibrary.Instance.Resolve(target.CloseTransition);

            // Run close transition
            if (transition != null)
            {
                yield return transition.FadeInCoroutine();
                target.Hide();

                if (blackScreenHoldDuration > 0f)
                {
                    yield return new WaitForSecondsRealtime(blackScreenHoldDuration);
                }
                    
                yield return transition.FadeOutCoroutine();
            }
            else
            {
                target.Hide();
            }

            activeViews.Remove(target);

            // Unfreeze the new top
            if (CurrentView != null)
            {
                CurrentView.Unfreeze();
            }

            IsTransitioning = false;
            UpdatePauseState();
            DebugHistory();
        }

        private IEnumerator ShowAsOverlay(View target)
        {
            IsTransitioning = true;
            UpdatePauseState();

            Transition transition = TransitionLibrary.Instance.Resolve(target.OpenTransition);

            if (transition != null)
            {
                yield return transition.FadeInCoroutine();
                target.Show();

                if (blackScreenHoldDuration > 0f)
                {
                    yield return new WaitForSecondsRealtime(blackScreenHoldDuration);
                }

                yield return transition.FadeOutCoroutine();
            }
            else
            {
                target.Show();
            }

            activeViews.Add(target);
            IsTransitioning = false;
            UpdatePauseState();
            DebugHistory();
        }

        /// <summary>
        /// Immediately removes the specified view from the stack without transitions.
        /// </summary>
        internal void InstantClose<T>() where T : View
        {
            // Find the specific instance in our active stack
            View target = activeViews.FirstOrDefault(v => v is T);

            if (target != null)
            {
                target.Hide();
                activeViews.Remove(target);

                // Unfreeze the new top view now that this one is gone
                if(CurrentView != null)
                {
                    CurrentView.Unfreeze();
                }

                UpdatePauseState();
                DebugHistory();
            }
        }

        private void UpdatePauseState()
        {
            bool shouldPause = HasActiveView || IsTransitioning;
            PauseManager.SetPaused(shouldPause);
        }

        private void DebugHistory()
        {
            if (!enableDebugLogs)
                return;

            string stack = activeViews.Count > 0
                ? string.Join(" -> ", activeViews.Select(v => v.GetType().Name))
                : "Empty";

            Log.Info(nameof(ViewManager), $"View Stack: {stack}");
        }
    }
}
