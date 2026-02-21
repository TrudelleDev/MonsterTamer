namespace MonsterTamer.Items.Models
{
    /// <summary>
    /// Result of using an item, including success and display messages.
    /// </summary>
    internal readonly struct ItemUseResult
    {
        internal bool IsUsed { get; }
        internal string Messages { get; }

        internal ItemUseResult(bool used, string messages)
        {
            IsUsed = used;
            Messages = messages;
        }
    }
}
