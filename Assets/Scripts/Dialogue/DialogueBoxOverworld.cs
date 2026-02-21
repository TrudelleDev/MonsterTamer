using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Dialogue
{
    /// <summary>
    /// Singleton wrapper for the overworld <see cref="DialogueBox"/>.
    /// </summary>
    internal sealed class DialogueBoxOverworld : Singleton<DialogueBoxOverworld>
    {
        [SerializeField, Required] private DialogueBox dialogueBox;

        internal DialogueBox Dialogue => dialogueBox;
    }
}
