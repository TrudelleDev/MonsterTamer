namespace MonsterTamer.Types
{
    /// <summary>
    /// Converts combat effectiveness enums into numerical damage multipliers.
    /// </summary>
    internal static class TypeEffectivenessExtensions
    {
        internal static float ToMultiplier(this TypeEffectiveness effectiveness)
        {
            return effectiveness switch
            {
                TypeEffectiveness.SuperEffective    => 2f,
                TypeEffectiveness.NotVeryEffective  => 0.5f,
                TypeEffectiveness.Immune            => 0f,
                _                                   => 1f
            };
        }
    }
}
