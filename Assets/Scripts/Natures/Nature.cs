namespace MonsterTamer.Natures

{
    /// <summary>
    /// Represents a runtime instance of a Monster nature.
    /// </summary>
    internal sealed class Nature
    {
        internal NatureDefinition Definition { get; private set; }

        internal Nature(NatureDefinition definition)
        {
            Definition = definition;
        }
    }
}
