using MonsterTamer.Audio;
using MonsterTamer.Moves.Definitions;
using UnityEngine;

namespace MonsterTamer.Battle
{
    /// <summary>
    /// Handles playback of battle-related audio, such as move and event sounds.
    /// </summary>
    internal sealed class BattleAudio : MonoBehaviour
    {
        internal void PlayMoveSound(MoveDefinition move)
        {
            if (move != null && move.Sound != null)
            {
                AudioManager.Instance.PlaySFX(move.Sound);
            }
        }
    }
}
