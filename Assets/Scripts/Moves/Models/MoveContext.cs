using MonsterTamer.Battle;
using MonsterTamer.Monsters;

namespace MonsterTamer.Moves.Models
{
    /// <summary>
    /// Contains all relevant data for executing a Monster move,
    /// including the battle, user, target, and move instance.
    /// </summary>
    internal readonly struct MoveContext
    {
        internal BattleView Battle { get; }
        internal Monster User { get; }
        internal Monster Target { get; }
        internal Move Move { get; }

        internal MoveContext(BattleView battle, Monster user, Monster target, Move move)
        {
            Battle = battle;
            User = user;
            Target = target;
            Move = move;
        }
    }
}
