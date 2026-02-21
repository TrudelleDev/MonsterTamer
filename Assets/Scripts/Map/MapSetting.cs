using MonsterTamer.Audio;
using MonsterTamer.Battle;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Map
{
    /// <summary>
    /// Configures map-specific audio and dialogue.
    /// Plays the map's BGM on load and reapplies settings after battles.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class MapSetting : MonoBehaviour
    {
        [SerializeField, Required, LabelText("Background Music")]
        private AudioClip bgmClip;

        private BattleView battleView;

        private void Start()
        {
            ApplyMapSetting();

            battleView = ViewManager.Instance.Get<BattleView>();
            if (battleView != null)
            {
                battleView.OnBattleViewClose += ApplyMapSetting;
            }
        }

        private void OnDestroy()
        {
            if (battleView != null)
            {
                battleView.OnBattleViewClose -= ApplyMapSetting;
            }
        }

        private void ApplyMapSetting()
        {
            if (AudioManager.Instance != null && bgmClip != null)
            {
                AudioManager.Instance.PlayBGM(bgmClip);
            }
        }
    }
}
