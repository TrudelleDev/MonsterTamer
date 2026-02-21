namespace MonsterTamer.Characters.Interfaces
{
    /// <summary>
    /// Defines a character state lifecycle.
    /// </summary>
    internal interface ICharacterState
    {
        void Enter();
        void Update();
        void Exit();
    }
}
