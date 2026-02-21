using MonsterTamer.Moves.Definitions;

namespace MonsterTamer.Moves
{
    /// <summary>
    /// Represents an in-battle instance of a Monster move.
    /// </summary>
    internal sealed class Move
    {
        internal int PowerPointRemaining { get; private set; }
        internal MoveDefinition Definition { get; private set; }

        internal Move(MoveDefinition definition)
        {
            Definition = definition;
            PowerPointRemaining = definition.MoveInfo.PowerPoint;
        }

        internal void ConsumePowerPoint()
        {
            PowerPointRemaining--;
        }
    }
}
