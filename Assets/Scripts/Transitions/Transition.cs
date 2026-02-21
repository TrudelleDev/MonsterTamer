using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace MonsterTamer.Transitions
{
    /// <summary>
    /// Base class for all screen transitions, providing a unified API for 
    /// Action callbacks, Coroutines, and Async/Await workflows.
    /// </summary>
    internal abstract class Transition : MonoBehaviour
    {
        internal static event Action FadeOutCompleted;

        protected abstract void FadeInInternal(Action onComplete);
        protected abstract void FadeOutInternal(Action onComplete);

        internal void FadeIn(Action onComplete = null) => FadeInInternal(onComplete);

        internal void FadeOut(Action onComplete = null)
        {
            FadeOutInternal(() =>
            {
                FadeOutCompleted?.Invoke();
                onComplete?.Invoke();
            });
        }

        internal IEnumerator FadeInCoroutine()
        {
            bool done = false;
            FadeIn(() => done = true);
            yield return new WaitUntil(() => done);
        }

        internal IEnumerator FadeOutCoroutine()
        {
            bool done = false;
            FadeOut(() => done = true);
            yield return new WaitUntil(() => done);
        }

        internal async Task FadeInAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            FadeIn(() => tcs.SetResult(true));
            await tcs.Task;
        }

        internal async Task FadeOutAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            FadeOut(() => tcs.SetResult(true));
            await tcs.Task;
        }
    }
}