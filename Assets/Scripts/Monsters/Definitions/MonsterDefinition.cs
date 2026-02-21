using MonsterTamer.Monsters.Models;
using MonsterTamer.Moves.Models;
using MonsterTamer.Natures;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Monsters.Definitions
{
    /// <summary>
    /// Static data container describing a Monster's identity, typing,
    /// base stats, available natures, learnset, and visual assets.
    /// </summary>
    [CreateAssetMenu(menuName = "MonsterTamer/Monster/Monster Definition")]
    internal sealed class MonsterDefinition : ScriptableObject
    {
        [BoxGroup("Identity")]
        [SerializeField, Required]
        [Tooltip("Name displayed in UI.")]
        private string displayName;

        [BoxGroup("Identity")]
        [SerializeField, Required, Range(0, 50)]
        [Tooltip("Unique codex identifier.")]
        private int codexNumber;

        [BoxGroup("Attributes")]
        [SerializeField]
        [Tooltip("Primary and optional secondary elemental typing.")]
        private MonsterType typing;

        [BoxGroup("Attributes")]
        [SerializeField]
        [Tooltip("Base stats used to compute runtime stats.")]
        private MonsterStats baseStats;

        [BoxGroup("Natures")]
        [SerializeField, Required]
        [Tooltip("Pool of possible natures.")]
        private NatureDatabase possibleNatures;

        [BoxGroup("Moves")]
        [SerializeField]
        [Tooltip("Moves learned through level progression.")]
        private LevelUpMove[] levelUpMoves;

        [BoxGroup("Visuals")]
        [SerializeField]
        [Tooltip("Sprites used in battle and UI.")]
        private MonsterSprites sprites;

        internal string DisplayName => displayName;
        internal int CodexNumber => codexNumber;
        internal MonsterType Typing => typing;
        internal MonsterStats BaseStats => baseStats;
        internal NatureDatabase PossibleNatures => possibleNatures;
        internal LevelUpMove[] LevelUpMoves => levelUpMoves;
        internal MonsterSprites Sprites => sprites;
    }
}
