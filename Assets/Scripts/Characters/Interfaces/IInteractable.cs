using MonsterTamer.Characters.Core;

namespace MonsterTamer.Characters.Interfaces
{
    /// <summary>
    /// Defines an object that can be interacted with by a character.
    /// </summary>
    internal interface IInteractable
    {
        void Interact(Character initiator);
    }
}
