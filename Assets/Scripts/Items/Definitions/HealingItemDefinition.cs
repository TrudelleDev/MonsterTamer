using MonsterTamer.Items.Models;
using MonsterTamer.Monsters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Items.Definitions
{
    /// <summary>
    /// Restores a fixed amount of HP to a Monster when used.
    /// </summary>
    [CreateAssetMenu(menuName = "MonsterTamer/Items/Healing Item Definition")]
    internal sealed class HealingItemDefinition : ItemDefinition
    {
        [SerializeField, Required] private int healingAmount;

        internal override ItemUseResult Use(Monster target)
        {
            if (target == null)
            {
                return new ItemUseResult(false, FailMessage);
            }

            int restored = target.Health.RestoreHealth(healingAmount);

            if (restored > 0)
            {
                string message = $"{target.Definition.DisplayName}'s HP was restored\nby {restored} points.";
                return new ItemUseResult(true, message);
            }

            return new ItemUseResult(false, NoEffectMessage);
        }
    }
}
