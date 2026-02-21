using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Transitions
{
    /// <summary>
    /// Executes a multi-stage battle transition sequence involving screen flashes
    /// followed by a shader-based mask reveal/hide. 
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class BattleIntroTransition : Transition
    {
        private static readonly int flashStrengthId = Shader.PropertyToID("_FlashStrength");
        private static readonly int cutoffId = Shader.PropertyToID("_Cutoff");
        private static readonly int flashColorId = Shader.PropertyToID("_Color");

        [Title("Settings")]
        [SerializeField, Range(1, 10)] private int flashCount = 3;
        [SerializeField, Min(0.01f)] private float flashDuration = 0.1f;
        [SerializeField, Min(0.05f)] private float maskDuration = 0.8f;
        [SerializeField, Min(0f)] private float holdDuration = 1f;
        [SerializeField] private Color flashColor = Color.gray;

        [Title("Images")]
        [SerializeField, Required] private Image flashImage;
        [SerializeField, Required] private Image maskImage;

        private Material flashMaterial;
        private Material maskMaterial;
        private Coroutine activeRoutine;

        private void Awake()
        {
            // Duplicate materials to avoid modifying shared assets
            flashMaterial = new Material(flashImage.material);
            maskMaterial = new Material(maskImage.material);

            flashImage.material = flashMaterial;
            maskImage.material = maskMaterial;

            flashMaterial.SetFloat(flashStrengthId, 0f);
            flashMaterial.SetColor(flashColorId, flashColor);
            maskMaterial.SetFloat(cutoffId, 1f);

            flashImage.enabled = false;
            maskImage.enabled = true;
        }

        private void OnDestroy()
        {
            if (flashMaterial != null)
                Destroy(flashMaterial);

            if (maskMaterial != null) 
                Destroy(maskMaterial);
        }

        protected override void FadeInInternal(Action onComplete)
        {
            if (activeRoutine != null)
                StopCoroutine(activeRoutine);

            activeRoutine = StartCoroutine(RunFadeIn(onComplete));
        }

        protected override void FadeOutInternal(Action onComplete)
        {
            if (activeRoutine != null)
                StopCoroutine(activeRoutine);

            activeRoutine = StartCoroutine(RunFadeOut(onComplete));
        }

        private IEnumerator RunFadeIn(Action onComplete)
        {
            flashImage.enabled = true;

            for (int i = 0; i < flashCount; i++)
            {
                yield return LerpMaterial(flashMaterial, flashStrengthId, 0f, 1f, flashDuration * 0.5f);
                yield return LerpMaterial(flashMaterial, flashStrengthId, 1f, 0f, flashDuration * 0.5f);
            }

            flashImage.enabled = false;
            yield return LerpMaterial(maskMaterial, cutoffId, 1f, 0f, maskDuration);

            onComplete?.Invoke();
        }

        private IEnumerator RunFadeOut(Action onComplete)
        {
            if (holdDuration > 0f)
                yield return new WaitForSecondsRealtime(holdDuration);

            maskMaterial.SetFloat(cutoffId, 1f);
            onComplete?.Invoke();
        }

        private IEnumerator LerpMaterial(Material mat, int propId, float from, float to, float duration)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                mat.SetFloat(propId, Mathf.Lerp(from, to, elapsed / duration));
                yield return null;
            }
            mat.SetFloat(propId, to);
        }
    }
}