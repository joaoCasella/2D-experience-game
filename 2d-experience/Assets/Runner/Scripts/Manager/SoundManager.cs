using Runner.Scripts.Service;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Scripts.Manager
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;
        public static SoundManager Instance => _instance;

        public float MusicVolume
        {
            get
            {
                return ConfigurationService.GetSavedMusicVolume();
            }
            set
            {
                ConfigurationService.SaveMusicVolume(Mathf.Clamp01(value));
            }
        }

        public float SoundFXVolume
        {
            get
            {
                return ConfigurationService.GetSavedSoundFXVolume();
            }
            set
            {
                ConfigurationService.SaveSoundFXVolume(Mathf.Clamp01(value));
            }
        }

        public List<AudioSource> MusicAudioSources { get; } = new List<AudioSource>();
        public List<AudioSource> SoundFXAudioSources { get; } = new List<AudioSource>();

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void RegisterMusicSource(AudioSource source)
        {
            MusicAudioSources.Add(source);
            source.volume = MusicVolume;
        }

        public void DeregisterMusicSource(AudioSource source)
        {
            MusicAudioSources.Remove(source);
        }

        public void RegisterSoundFXSource(AudioSource source)
        {
            SoundFXAudioSources.Add(source);
            source.volume = SoundFXVolume;
        }

        public void DeregisterSoundFXSource(AudioSource source)
        {
            SoundFXAudioSources.Remove(source);
        }

        public void OnSoundConfigurationsChanged(float musicVolume, float soundFXVolume)
        {
            foreach (var musicAudioSource in MusicAudioSources)
            {
                musicAudioSource.volume = musicVolume;
            }

            foreach (var soundFXAudioSource in SoundFXAudioSources)
            {
                soundFXAudioSource.volume = soundFXVolume;
            }
        }

        public void SaveSoundConfigurations(float musicVolume, float soundFXVolume)
        {
            MusicVolume = musicVolume;
            SoundFXVolume = soundFXVolume;

            OnSoundConfigurationsChanged(MusicVolume, SoundFXVolume);
        }

        private void OnDestroy()
        {
            MusicAudioSources.Clear();
            SoundFXAudioSources.Clear();
        }
    }
}
