using MonsterTamer.Moves.Effects;
using MonsterTamer.Moves.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Moves.Definitions
{
    /// <summary>
    /// Defines the static data and core properties for a monster move.
    /// </summary>
    [CreateAssetMenu(menuName = "MonsterTamer/Move/Move Definition")]
    internal sealed class MoveDefinition : ScriptableObject
    {
        [SerializeField, Required, Tooltip("Display name of the move.")]
        private string displayName;

        [SerializeField, Tooltip("Base stats of the move: Power, Accuracy, and PP.")]
        private MoveInfo moveInfo;

        [SerializeField, Tooltip("Classification of the move: Type and Category.")]
        private MoveClassification classification;

        [SerializeField, Tooltip("The effect this move applies (damage, status, etc.).")]
        private MoveEffect moveEffect;

        [SerializeField, Required, TextArea(5, 10)]
        [Tooltip("Description or effect text shown to the player.")]
        private string effect;

        [SerializeField, Tooltip("Sound played when the move is used.")]
        private AudioClip sound;

        internal string DisplayName => displayName;
        internal MoveInfo MoveInfo => moveInfo;
        internal MoveClassification Classification => classification;
        internal MoveEffect MoveEffect => moveEffect;
        internal string Effect => effect;
        internal AudioClip Sound => sound;
    }
}
