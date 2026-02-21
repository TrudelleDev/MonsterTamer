namespace MonsterTamer.Battle.States.Core
{
    /// <summary>
    /// Defines the lifecycle contract for a battle state within the State Machine.
    /// </summary>
    internal interface IBattleState
    {
        void Enter();
        void Update();
        void Exit();
    }
}