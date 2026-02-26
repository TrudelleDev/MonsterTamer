using MonsterTamer.Battle.States.Core;
using MonsterTamer.Battle.UI;
using MonsterTamer.Moves;
using MonsterTamer.Views;

namespace MonsterTamer.Battle.States.Player
{
    /// <summary>
    /// Handles the move selection flow, including UI binding and player input.
    /// </summary>
    internal sealed class PlayerMoveSelectState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private BattleMoveSelectionView moveSelectionView;
        private BattleView Battle => machine.BattleView;

        internal PlayerMoveSelectState(BattleStateMachine machine)
        {
            this.machine = machine;
        }

        public void Enter() => OpenMoveSelection();

        public void Update() { }

        public void Exit() => CloseMoveSelection();

        private void OpenMoveSelection()
        {
            moveSelectionView = ViewManager.Instance.Show<BattleMoveSelectionView>();
            moveSelectionView.BindMoves(Battle.PlayerActiveMonster.Moves.MoveSet);

            moveSelectionView.MoveConfirmed += HandleMoveConfirmed;
            moveSelectionView.BackRequested += OnBackRequested;
        }

        private void CloseMoveSelection()
        {
            if (moveSelectionView == null) return;

            moveSelectionView.MoveConfirmed -= HandleMoveConfirmed;
            moveSelectionView.BackRequested -= OnBackRequested;

            ViewManager.Instance.Close<BattleMoveSelectionView>();
        }

        private void OnBackRequested() => machine.SetState(new PlayerActionMenuState(machine));
        private void HandleMoveConfirmed(Move move) => machine.SetState(new BattleSpeedCheckState(machine, move));
    }
}
