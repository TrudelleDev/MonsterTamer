using MonsterTamer.Battle.States.Core;
using MonsterTamer.Battle.States.Opponent;
using MonsterTamer.Inventory.UI;
using MonsterTamer.Views;

namespace MonsterTamer.Battle.States.Player
{
    /// <summary>
    /// Handles the player inventory UI during battle, allowing item usage and transitioning
    /// control to the opponent when an item is used.
    /// </summary>
    internal sealed class PlayerInventoryState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private InventoryView inventoryView;
        private InventoryPresenter inventoryPresenter;
        private BattleView Battle => machine.BattleView;

        internal PlayerInventoryState(BattleStateMachine machine)
        {
            this.machine = machine;
        }

        public void Enter()
        {
            inventoryView = ViewManager.Instance.Show<InventoryView>();
            inventoryPresenter = inventoryView.GetComponent<InventoryPresenter>();

            inventoryView.BackRequested += OnBackRequested;
            inventoryPresenter.ItemUsed += OnItemUsed;
        }

        public void Update() { }

        public void Exit()
        {
            inventoryView.BackRequested -= OnBackRequested;
            inventoryPresenter.ItemUsed -= OnItemUsed;
        }

        private void OnItemUsed(bool used)
        {
            if (!used) return;

            ViewManager.Instance.InstantClose<InventoryView>();

            // Opponent AI selects a move because the player's turn is consumed by the item
            var opponentMonster = Battle.OpponentActiveMonster;
            var selectedMove = opponentMonster.GetRandomMove();

            // Transition to Opponent Turn
            machine.SetState(new OpponentTurnState(machine, selectedMove, null, isActingFirst: false));
        }

        private void OnBackRequested() => machine.SetState(new PlayerActionMenuState(machine));
    }
}
