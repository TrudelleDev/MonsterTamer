using UnityEngine;

namespace MonsterTamer.Characters.Directions
{
    /// <summary>
    /// Converts InputDirection to vectors and facing directions.
    /// </summary>
    internal static class InputDirectionExtensions
    {
        private const string InvalidMessage = "Invalid InputDirection value.";

        internal static Vector2Int ToVector2Int(this InputDirection direction)
        {
            return direction switch
            {
                InputDirection.Up => Vector2Int.up,
                InputDirection.Down => Vector2Int.down,
                InputDirection.Left => Vector2Int.left,
                InputDirection.Right => Vector2Int.right,
                InputDirection.None => Vector2Int.zero,
                _ => throw new System.ArgumentOutOfRangeException(nameof(direction), direction, InvalidMessage)
            };
        }

        internal static FacingDirection ToFacingDirection(this InputDirection input)
        {
            return input switch
            {
                InputDirection.Up => FacingDirection.North,
                InputDirection.Down => FacingDirection.South,
                InputDirection.Left => FacingDirection.West,
                InputDirection.Right => FacingDirection.East,
                _ => throw new System.ArgumentOutOfRangeException(nameof(input), input, InvalidMessage)
            };
        }
    }
}
