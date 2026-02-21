using System;
using MonsterTamer.Moves;
using MonsterTamer.Shared.UI.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Battle.UI
{
    /// <summary>
    /// Displays the player's available moves as interactive buttons.
    /// Handles binding, highlighting, and confirming selection.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class BattleMoveSelectionPanel : MonoBehaviour
    {
        [SerializeField, Required] private MenuButton firstMoveButton;
        [SerializeField, Required] private MenuButton secondMoveButton;
        [SerializeField, Required] private MenuButton thirdMoveButton;
        [SerializeField, Required] private MenuButton fourthMoveButton;

        private MenuButton[] buttons;
        private readonly Action[] clickHandlers = new Action[4];
        private readonly Action[] selectHandlers = new Action[4];

        internal event Action<Move> MoveSelected;
        internal event Action<Move> MoveFocused;

        private void Awake()
        {
            buttons = new[] { firstMoveButton, secondMoveButton, thirdMoveButton, fourthMoveButton };
        }

        /// <summary>
        /// Assigns moves to buttons and hooks up events.
        /// </summary>
        /// <param name="moves">Array of 1–4 moves for the active Monster.</param>
        internal void BindMoves(Move[] moves)
        {
            UnbindMoves();

            if (moves == null) return;

            for (int i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];

                if (i < moves.Length && moves[i] != null)
                {
                    var move = moves[i];

                    button.SetLabel(move.Definition.DisplayName);
                    button.SetInteractable(true);

                    clickHandlers[i] = () => MoveSelected?.Invoke(move);
                    button.Selected += clickHandlers[i];

                    selectHandlers[i] = () => MoveFocused?.Invoke(move);
                    button.Focused += selectHandlers[i];
                }
                else
                {
                    button.SetLabel("-");
                    button.SetInteractable(false);
                }
            }
        }

        /// <summary>
        /// Clears all button bindings and disables interaction.
        /// </summary>
        internal void UnbindMoves()
        {
            if (buttons == null) return;

            for (int i = 0; i < buttons.Length; i++)
            {
                if (clickHandlers[i] != null)
                {
                    buttons[i].Selected -= clickHandlers[i];
                    clickHandlers[i] = null;
                }

                if (selectHandlers[i] != null)
                {
                    buttons[i].Focused -= selectHandlers[i];
                    selectHandlers[i] = null;
                }

                buttons[i].SetLabel("-");
                buttons[i].SetInteractable(false);
            }
        }
    }
}
