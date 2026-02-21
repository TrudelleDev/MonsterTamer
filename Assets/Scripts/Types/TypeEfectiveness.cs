namespace MonsterTamer.Types
{
    /// <summary>
    /// Represents the damage multiplier categories for type-based combat.
    /// </summary>
    internal enum TypeEffectiveness
    {
        None = 0,            // Use for uninitialized/invalid states
        Immune,              // 0x multiplier
        NotVeryEffective,    // 0.5x multiplier
        Normal,              // 1x multiplier
        SuperEffective       // 2x multiplier
    }
}
