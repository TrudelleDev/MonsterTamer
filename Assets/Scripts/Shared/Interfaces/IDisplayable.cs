using UnityEngine;

namespace MonsterTamer.Shared.Interfaces
{
    /// <summary>
    /// Represents something that can be displayed in the UI with an icon and description.
    /// </summary>
    internal interface IDisplayable
    {   
        string DisplayName { get; }
        string Description { get; }
        Sprite Icon { get; }
    }
}
