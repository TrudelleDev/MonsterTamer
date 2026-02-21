using System.Collections.Generic;
using System.Linq;
using MonsterTamer.Moves;
using MonsterTamer.Moves.Definitions;

namespace MonsterTamer.Monsters.Components
{
    /// <summary>
    /// Manages the 4-slot moveset known by a monster instance.
    /// </summary>
    internal sealed class MovesComponent
    {
        internal const int MaxMoves = 4;

        /// <summary>
        ///  The 4 fixed move slots (includes nulls).
        /// </summary>
        internal Move[] MoveSet { get; } = new Move[MaxMoves];

        /// <summary>
        /// Only the moves that are currently learned. 
        /// </summary>
        internal IEnumerable<Move> ActiveMoves => MoveSet.Where(m => m != null);

        internal MovesComponent(MoveDefinition[] moveDefinitions)
        {
            for (int i = 0; i < MoveSet.Length; i++)
            {
                if (i < moveDefinitions.Length && moveDefinitions[i] != null)
                {
                    MoveSet[i] = new Move(moveDefinitions[i]);
                }
            }
        }
    }
}
