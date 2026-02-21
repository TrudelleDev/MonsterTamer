using UnityEngine;

namespace MonsterTamer.Characters.Directions
{
    /// <summary>
    /// Converts FacingDirection to vectors, input, and opposites.
    /// </summary>
    internal static class FacingDirectionExtensions
    {
        private const string InvalidMessage = "Invalid FacingDirection value.";

        internal static Vector2Int ToVector2Int(this FacingDirection direction)
        {
            return direction switch
            {
                FacingDirection.North => Vector2Int.up,
                FacingDirection.South => Vector2Int.down,
                FacingDirection.West => Vector2Int.left,
                FacingDirection.East => Vector2Int.right,
                _ => throw new System.ArgumentOutOfRangeException(nameof(direction), direction, InvalidMessage)
            };
        }

        internal static InputDirection ToInputDirection(this FacingDirection facing)
        {
            return facing switch
            {
                FacingDirection.North => InputDirection.Up,
                FacingDirection.South => InputDirection.Down,
                FacingDirection.West => InputDirection.Left,
                FacingDirection.East => InputDirection.Right,
                _ => throw new System.ArgumentOutOfRangeException(nameof(facing), facing, InvalidMessage)
            };
        }

        internal static FacingDirection Opposite(this FacingDirection direction)
        {
            return direction switch
            {
                FacingDirection.North => FacingDirection.South,
                FacingDirection.South => FacingDirection.North,
                FacingDirection.West => FacingDirection.East,
                FacingDirection.East => FacingDirection.West,
                _ => throw new System.ArgumentOutOfRangeException(nameof(direction), direction, InvalidMessage)
            };
        }
    }
}
