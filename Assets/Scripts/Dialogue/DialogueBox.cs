using System;
using System.Collections;
using System.Collections.Generic;
using MonsterTamer.Audio;
using MonsterTamer.Pause;
using MonsterTamer.Utilities;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Dialogue
{
    /// <summary>
    /// Manages the display of dialogue text in the game.
    /// Supports instant display, typewriter effect, and optional waiting for player input.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class DialogueBox : MonoBehaviour
    {
        [Title("Visual")]
        [SerializeField, Required] private TextMeshProUGUI dialogueText;
        [SerializeField, Required] private GameObject content;
        [SerializeField, Required] private Image cursor;

        [Title("Settings")]
        [SerializeField] private bool autoClose = true;
        [SerializeField, MinValue(0.01f)] private float characterDelay = 0.05f;
        [SerializeField, Required] private UIAudioSettings audioSetting;

        private string[] pages;
        private int pageIndex;
        private Coroutine dialogueCoroutine;
        private bool instantMode;
        private bool waitForInput;

        public event Action LineTypingCompleted;
        public event Action DialogueFinished;

        private void Awake()
        {
            Clear();

            if (autoClose)
            {
                content.SetActive(false);
            }
        }

        /// <summary>
        /// Displays text instantly with no typewriter effect or input wait.
        /// </summary>
        internal void DisplayInstant(string text)
        {
            StartDialogue(text, instant: true, waitForInput: false);
        }

        /// <summary>
        /// Displays text with typewriter effect and waits for player input.
        /// </summary>
        internal void DisplayWithInput(string text)
        {
            StartDialogue(text, instant: false, waitForInput: true);
        }

        /// <summary>
        /// Displays dialogue in battle context; yields until typing and player advance are complete.
        /// </summary>
        internal IEnumerator DisplayBattleDialogue(string text)
        {
            StartDialogue(text, instant: false, waitForInput: true, forceCursor: true);
            yield return WaitForTyping();
            yield return WaitForAdvance();
        }

        /// <summary>
        /// Displays text with typing effect and yields until typing finishes.
        /// No player input required.
        /// </summary>
        internal IEnumerator DisplayAndWaitTyping(string text)
        {
            StartDialogue(text, instant: false, waitForInput: false);
            yield return WaitForTyping();
        }
        internal void Clear()
        {
            dialogueText.text = " ";
            dialogueText.ForceMeshUpdate();
            cursor.gameObject.SetActive(false);
        }

        private void StartDialogue(string text, bool instant, bool waitForInput, bool forceCursor = false)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                Log.Warning(nameof(DialogueBox), "Tried to show empty dialogue.");
                return;
            }

            pages = SplitIntoPages(text, 2);
            pageIndex = 0;
            instantMode = instant;
            this.waitForInput = waitForInput;

            content.SetActive(true);
            PauseManager.SetPaused(true);

            Clear();
            RestartCoroutine(ref dialogueCoroutine, RunDialogueSequence(forceCursor));
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

        private IEnumerator RunDialogueSequence(bool forceCursor = false)
        {
            while (pageIndex < pages.Length)
            {
                yield return TypeLine(pages[pageIndex]);
                LineTypingCompleted?.Invoke();

                bool hasNextPage = pageIndex < pages.Length - 1;

                // Show cursor if there's more pages or forceCursor is true
                cursor.gameObject.SetActive(forceCursor || hasNextPage);

                if (waitForInput)
                {
                    yield return WaitForAdvance();
                }

                cursor.gameObject.SetActive(false);
                pageIndex++;
            }

            FinishDialogue();
        }

        private IEnumerator TypeLine(string page)
        {
            dialogueText.text = "";

            if (instantMode)
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

        private void FinishDialogue()
        {
            DialogueFinished?.Invoke();

            if (!autoClose) return;

            Clear();
            content.SetActive(false);
            PauseManager.SetPaused(false);
        }

        private void RestartCoroutine(ref Coroutine routine, IEnumerator sequence)
        {
            if (routine != null)
            {
                StopCoroutine(routine);
            }

            routine = StartCoroutine(sequence);
        }

        private string[] SplitIntoPages(string text, int linesPerPage)
        {
            // Handles all common types of newlines: Windows (\r\n), Unix/Linux (\n), and old Mac (\r)
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
