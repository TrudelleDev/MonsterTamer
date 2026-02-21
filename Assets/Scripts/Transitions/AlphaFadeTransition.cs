using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Transitions
{
    /// <summary>
    /// A screen-space transition that drives the "_Alpha" property of a custom transition shader.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    internal sealed class AlphaFadeTransition : Transition
    {
        private static readonly int alphaPropId = Shader.PropertyToID("_Alpha");

        [SerializeField, Min(0.01f)] private float duration = 1f;

        private Image fadeImage;
        private Material fadeMaterial;
        private Coroutine fadeRoutine;

        private void Awake()
        {
            fadeImage = GetComponent<Image>();

            // Create a material instance so we don't leak changes to the project asset
            fadeMaterial = new Material(fadeImage.material);
            fadeImage.material = fadeMaterial;

            // Initialize state
            SetShaderAlpha(0);
            fadeImage.enabled = false;
        }

        private void OnDestroy()
        {
            // Clean up the instantiated material to prevent memory leaks
            if (fadeMaterial != null)
                Destroy(fadeMaterial);
        }

        protected override void FadeInInternal(Action onComplete) => StartFade(0, 1, onComplete);
        protected override void FadeOutInternal(Action onComplete) => StartFade(1, 0, onComplete);

        private void StartFade(float start, float target, Action onComplete)
        {
            if (fadeRoutine != null) StopCoroutine(fadeRoutine);

            fadeImage.enabled = true;
            fadeRoutine = StartCoroutine(FadeRoutine(start, target, onComplete));
        }

        private IEnumerator FadeRoutine(float start, float target, Action onComplete)
        {
            float elapsed = 0;
            SetShaderAlpha(start);

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                SetShaderAlpha(Mathf.Lerp(start, target, t));
                yield return null;
            }

            SetShaderAlpha(target);

            // Disable image after fade-out (reveal game) to allow UI interactions
            if (target <= 0) fadeImage.enabled = false;

            fadeRoutine = null;
            onComplete?.Invoke();
        }

        private void SetShaderAlpha(float alpha)
        {
            if (fadeMaterial != null)
                fadeMaterial.SetFloat(alphaPropId, alpha);
        }
    }
}