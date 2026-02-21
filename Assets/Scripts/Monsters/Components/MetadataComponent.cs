namespace MonsterTamer.Monsters.Components
{
    /// <summary>
    /// Stores runtime metadata for a monster instance.
    /// </summary>
    internal sealed class MetadataComponent
    {
        internal string OwnerName { get; set; }
        internal string EncounterLocation { get; set; }
  
        internal MetadataComponent() { }
    }
}
