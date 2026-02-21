using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace MonsterTamer.Audio
{
    /// <summary>
    /// Manages all game audio (BGM, SFX, UI).
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class AudioManager : Singleton<AudioManager>
    {
        [SerializeField, Required] private AudioMixerGroup bgmMixerGroup;
        [SerializeField, Required] private AudioMixerGroup sfxMixerGroup;
        [SerializeField, Required] private AudioMixerGroup uiMixerGroup;

        private AudioSource bgmSource;
        private AudioSource sfxSource;
        private AudioSource uiSource;

        protected override void Awake()
        {
            base.Awake();
            CreateAudioSources();
        }

        internal void PlayBGM(AudioClip clip)
        {
            if (clip == null) return;
            if (bgmSource.clip == clip && bgmSource.isPlaying) return;

            bgmSource.clip = clip;
            double startTime = AudioSettings.dspTime;
            bgmSource.PlayScheduled(startTime);
        }

        internal void PlaySFX(AudioClip clip)
        {
            if (clip == null) return;

            sfxSource.PlayOneShot(clip);
        }

        internal void PlayUISFX(AudioClip clip)
        {
            if (clip == null) return;

            uiSource.PlayOneShot(clip);
        }

        private void CreateAudioSources()
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.outputAudioMixerGroup = bgmMixerGroup;

            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = sfxMixerGroup;

            uiSource = gameObject.AddComponent<AudioSource>();
            uiSource.outputAudioMixerGroup = uiMixerGroup;
        }
    }
}
