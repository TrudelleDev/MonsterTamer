using MonsterTamer.Monsters;
using MonsterTamer.Moves;
using MonsterTamer.Moves.Enums;
using MonsterTamer.Types;
using UnityEngine;

namespace MonsterTamer.Battle
{
    /// <summary>
    /// Calculates damage for Monster moves, accounting for type effectiveness, STAB, 
    /// stats, level, and random variation.
    /// </summary>
    public static class DamageCalculator
    {
        private const float STABMultiplier = 1.5f;
        private const float RandomDamageMin = 0.85f;
        private const float RandomDamageMax = 1f;
        private const float LevelMultiplier = 2f;
        private const float LevelDamageDivider = 250f;
        private const float BaseDamageBonus = 2f;

        /// <summary>
        /// Calculates the damage a move deals from a user to a target Monster.
        /// Considers move category, stats, type effectiveness, STAB, and random variation.
        /// Automatically consumes 1 PP from the move.
        /// </summary>
        internal static int CalculateDamage(Monster user, Monster target, Move move)
        {
            bool isPhysical = move.Definition.Classification.Category == MoveCategory.Physical;

            int attack = isPhysical ? user.Stats.Core.Attack : user.Stats.Core.SpecialAttack;
            int defense = isPhysical ? target.Stats.Core.Defense : target.Stats.Core.SpecialDefense;

            float baseDamage =
                (((LevelMultiplier * user.Experience.Level + 10f) / LevelDamageDivider)
                * (attack / (float)defense)
                * move.Definition.MoveInfo.Power
                + BaseDamageBonus);

            float typeModifier = CalculateTypeModifier(move, target);
            float stabModifier = CalculateSTAB(user, move);
            float randomModifier = Random.Range(RandomDamageMin, RandomDamageMax);

            float finalDamage = baseDamage * typeModifier * stabModifier * randomModifier;

            return Mathf.Max(1, Mathf.RoundToInt(finalDamage));
        }

        /// <summary>
        /// Returns the type effectiveness multiplier of a move against a target Monster.
        /// </summary>
        private static float CalculateTypeModifier(Move move, Monster target)
        {
            float multiplier = 1f;
            var moveType = move.Definition.Classification.TypeDefinition;
            var types = target.Definition.Typing;

            multiplier *= moveType.EffectivenessGroups.GetEffectiveness(types.FirstType).ToMultiplier();

            if (types.SecondType != null)
            {
                multiplier *= moveType.EffectivenessGroups.GetEffectiveness(types.SecondType).ToMultiplier();
            }

            return multiplier;
        }

        /// <summary>
        /// Returns the STAB (Same-Type Attack Bonus) multiplier for a move.
        /// </summary>
        private static float CalculateSTAB(Monster user, Move move)
        {
            var moveType = move.Definition.Classification.TypeDefinition;
            var types = user.Definition.Typing;

            return (types.FirstType == moveType || types.SecondType == moveType) ? STABMultiplier : 1f;
        }
    }
}
