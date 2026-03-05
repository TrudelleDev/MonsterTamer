using System;
using System.Collections;
using System.Collections.Generic;
using MonsterTamer.Audio;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Dialogue
{
    /// <summary>
    /// Manages the display of dialogue text.
    /// Supports instant, timed, and input-based dialogue flows.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class DialogueView : View
    {
        [Title("Visual")]
        [SerializeField, Required] private TextMeshProUGUI dialogueText;
        [SerializeField, Required] private Image cursor;

        [Title("Settings")]
        [SerializeField, MinValue(0.01f)] private float characterDelay = 0.05f;
        [SerializeField, Required] private UIAudioSettings audioSetting;

        private string[] pages;
        private int pageIndex;
        private Coroutine dialogueCoroutine;

        public event Action LineTypingCompleted;
        public event Action DialogueFinished;

        private void Awake()
        {
            Clear();
        }

        /// <summary>
        /// Instant popup. No typing, no input. Does not auto close.
        /// </summary>
        internal void ShowInstant(string text)
        {
            StartDialogue(text, instant: true, waitForInput: false, autoClose: false);
        }

        /// <summary>
        /// Standard dialogue. Types and waits for input. Auto closes.
        /// </summary>
        internal void ShowConversational(string text)
        {
            StartDialogue(text, instant: false, waitForInput: true, autoClose: true);
        }

        /// <summary>
        /// Battle dialogue. Types and waits for input.
        /// Does not auto close (battle state controls closing).
        /// </summary>
        internal IEnumerator ShowBattleSequence(string text)
        {
            StartDialogue(text, instant: false, waitForInput: true, forceCursor: true, autoClose: false);

            yield return WaitForTyping();
            yield return WaitForAdvance();
        }

        /// <summary>
        /// Types text and finishes when typing ends.
        /// </summary>
        internal IEnumerator ShowTimedMessage(string text)
        {
            StartDialogue(text, instant: false, waitForInput: false, autoClose: false);
            yield return WaitForTyping();
        }

        /// <summary>
        /// Clears the dialogue text and hides the cursor.
        /// </summary>
        internal void Clear()
        {
            dialogueText.text = string.Empty;
            dialogueText.ForceMeshUpdate();
            cursor.gameObject.SetActive(false);
        }

        private void StartDialogue(string text, bool instant, bool waitForInput, bool forceCursor = false, bool autoClose = true)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            StopCurrentDialogue();

            if (!gameObject.activeInHierarchy)
            {
                ViewManager.Instance.Show<DialogueView>();
            }

            pages = SplitIntoPages(text, 2);
            pageIndex = 0;

            dialogueCoroutine = StartCoroutine(
                RunDialogueSequence(instant, waitForInput, forceCursor, autoClose)
            );
        }

        private IEnumerator RunDialogueSequence(bool instant, bool waitForInput, bool forceCursor, bool autoClose)
        {
            while (pageIndex < pages.Length)
            {
                yield return TypeLine(pages[pageIndex], instant);

                LineTypingCompleted?.Invoke();

                bool hasNextPage = pageIndex < pages.Length - 1;

                cursor.gameObject.SetActive(forceCursor || hasNextPage);

                if (waitForInput)
                {
                    yield return WaitForAdvance();
                }

                cursor.gameObject.SetActive(false);

                pageIndex++;
            }

            if (autoClose)
            {
                Clear();
                ViewManager.Instance.Close<DialogueView>();
            }

            DialogueFinished?.Invoke();
        }

        private IEnumerator WaitForTyping()
        {
            bool done = false;

            void OnTypingComplete()
            {
                done = true;
                LineTypingCompleted -= OnTypingComplete;
            }

            LineTypingCompleted += OnTypingComplete;

            yield return new WaitUntil(() => done);
        }

        private IEnumerator WaitForAdvance()
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyBinds.Interact));

            if (audioSetting != null)
            {
                AudioManager.Instance.PlayUISFX(audioSetting.ConfirmSfx);
            }
        }

        private IEnumerator TypeLine(string page, bool instant)
        {
            dialogueText.text = string.Empty;

            if (instant)
            {
                dialogueText.text = page;
                yield break;
            }

            WaitForSecondsRealtime delay = new(characterDelay);

            foreach (char character in page)
            {
                dialogueText.text += character;
                yield return delay;
            }
        }

        private void StopCurrentDialogue()
        {
            if (dialogueCoroutine != null)
            {
                StopCoroutine(dialogueCoroutine);
                dialogueCoroutine = null;
            }
        }

        private string[] SplitIntoPages(string text, int linesPerPage)
        {
            string[] lines = text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            List<string> result = new();

            for (int i = 0; i < lines.Length; i += linesPerPage)
            {
                int count = Mathf.Min(linesPerPage, lines.Length - i);
                result.Add(string.Join("\n", lines, i, count));
            }

            return result.ToArray();
        }
    }
}