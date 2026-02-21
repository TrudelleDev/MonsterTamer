using System;
using UnityEngine;

namespace MonsterTamer.Monsters.Components
{
    /// <summary>
    /// Manages health state and applies damage or healing.
    /// </summary>
    internal sealed class HealthComponent
    {
        internal int MaxHealth { get; }
        internal int CurrentHealth { get; private set; }

        /// <summary>
        /// Invoked when health changes.
        /// Parameters: previous HP, new HP.
        /// </summary>
        internal event Action<int, int> HealthChanged;

        internal HealthComponent(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        internal int ApplyDamage(int amount)
        {
            if (amount <= 0 || CurrentHealth <= 0)
            {
                return 0;
            }

            int oldHp = CurrentHealth;
            CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);

            HealthChanged?.Invoke(oldHp, CurrentHealth);
            return oldHp - CurrentHealth;
        }

        internal int RestoreHealth(int amount)
        {
            if (amount <= 0 || CurrentHealth >= MaxHealth)
            {
                return 0;
            }

            int oldHp = CurrentHealth;
            CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);

            HealthChanged?.Invoke(oldHp, CurrentHealth);
            return CurrentHealth - oldHp;
        }

        internal void RestoreFullHealth()
        {
            if (CurrentHealth == MaxHealth)
            {
                return;
            }

            int oldHp = CurrentHealth;
            CurrentHealth = MaxHealth;

            HealthChanged?.Invoke(oldHp, CurrentHealth);
        }
    }
}
