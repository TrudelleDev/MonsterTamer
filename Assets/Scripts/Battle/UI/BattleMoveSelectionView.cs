using System;
using MonsterTamer.Moves;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Battle.UI
{
    /// <summary>
    /// Display the player's moves, updates move info for the focused move,
    /// and raises an event when a move is selected.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class BattleMoveSelectionView : View
    {
        [SerializeField, Required] private BattleMoveSelectionPanel moveSelectionPanel;
        [SerializeField, Required] private BattleMoveInfoPanel moveSelectionDetail;

        internal event Action<Move> MoveConfirmed;

        private void OnEnable()
        {
            moveSelectionPanel.MoveSelected += OnMoveSelected;
            moveSelectionPanel.MoveFocused += OnMoveFocused;
        }

        private void OnDisable()
        {
            moveSelectionPanel.MoveSelected -= OnMoveSelected;
            moveSelectionPanel.MoveFocused -= OnMoveFocused;

            //ResetMenuController();
        }

        internal void BindMoves(Move[] moves)
        {
            if (moves == null || moves.Length == 0) return;

            moveSelectionPanel.BindMoves(moves);

            // Focus the first move by default
            OnMoveFocused(moves[0]);
        }

        private void OnMoveFocused(Move move) => moveSelectionDetail.Bind(move);
        private void OnMoveSelected(Move move) => MoveConfirmed?.Invoke(move);
    }
}
