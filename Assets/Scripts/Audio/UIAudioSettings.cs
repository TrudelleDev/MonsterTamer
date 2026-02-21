using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Audio
{
    /// <summary>
    /// Holds UI sound effects for selection, confirmation, and back actions.
    /// </summary>
    [CreateAssetMenu(menuName = "MonsterTamer/Settings/UI Audio Settings")]
    internal sealed class UIAudioSettings : ScriptableObject
    {
        [SerializeField, Required] private AudioClip selectSfx;
        [SerializeField, Required] private AudioClip confirmSfx;
        [SerializeField, Required] private AudioClip backSfx;

        internal AudioClip SelectSfx => selectSfx;
        internal AudioClip ConfirmSfx => confirmSfx;
        internal AudioClip BackSfx => backSfx;
    }
}
