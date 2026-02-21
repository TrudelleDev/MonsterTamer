namespace MonsterTamer.Battle.States.Core
{
    /// <summary>
    /// Manages battle flow by transitioning and updating <see cref="IBattleState"/> instances.
    /// </summary>
    internal sealed class BattleStateMachine
    {
        internal IBattleState CurrentState { get; private set; }
        internal BattleView BattleView { get; }

        internal BattleStateMachine(BattleView battleView)
        {
            BattleView = battleView;
        }

        internal void SetState(IBattleState nextState)
        {
            if (nextState is null || nextState == CurrentState) return;

            CurrentState?.Exit();
            CurrentState = nextState;
            CurrentState?.Enter();
        }

        internal void Update() => CurrentState?.Update();
    }
}