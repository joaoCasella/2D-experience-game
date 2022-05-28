using Runner.Scripts.Domain;
using Runner.Scripts.Service;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Scripts.Manager
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;
        public static SoundManager Instance => _instance;

        public float MainMenuMusicVolume
        {
            get
            {
                return ConfigurationService.GetSavedMusicVolume(MusicType.MainMenu);
            }
            set
            {
                ConfigurationService.SaveMusicVolume(MusicType.MainMenu, Mathf.Clamp01(value));
            }
        }

        public float GameplayMusicVolume
        {
            get
            {
                return ConfigurationService.GetSavedMusicVolume(MusicType.Gameplay);
            }
            set
            {
                ConfigurationService.SaveMusicVolume(MusicType.Gameplay, Mathf.Clamp01(value));
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

        public List<AudioSource> MainMenuMusicAudioSources { get; } = new List<AudioSource>();
        public List<AudioSource> GameplayMusicAudioSources { get; } = new List<AudioSource>();
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

        public void RegisterMusicSource(MusicType musicType, AudioSource source)
        {
            if (musicType == MusicType.MainMenu)
            {
                MainMenuMusicAudioSources.Add(source);
                source.volume = MainMenuMusicVolume;
            }
            else if (musicType == MusicType.Gameplay)
            {
                GameplayMusicAudioSources.Add(source);
                source.volume = GameplayMusicVolume;
            }
        }

        public void DeregisterMusicSource(MusicType musicType, AudioSource source)
        {
            if (musicType == MusicType.MainMenu)
                MainMenuMusicAudioSources.Remove(source);
            else if (musicType == MusicType.Gameplay)
                GameplayMusicAudioSources.Remove(source);
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

        public void OnSoundConfigurationsChanged(float mainMenuVolume, float gameplayVolume, float soundFXVolume)
        {
            foreach (var musicAudioSource in MainMenuMusicAudioSources)
            {
                musicAudioSource.volume = mainMenuVolume;
            }

            foreach (var musicAudioSource in GameplayMusicAudioSources)
            {
                musicAudioSource.volume = gameplayVolume;
            }

            foreach (var soundFXAudioSource in SoundFXAudioSources)
            {
                soundFXAudioSource.volume = soundFXVolume;
            }
        }

        public void SaveSoundConfigurations(float mainMenuMusicVolume, float gameplayMusicVolume, float soundFXVolume)
        {
            MainMenuMusicVolume = mainMenuMusicVolume;
            GameplayMusicVolume = gameplayMusicVolume;
            SoundFXVolume = soundFXVolume;

            OnSoundConfigurationsChanged(MainMenuMusicVolume, GameplayMusicVolume, SoundFXVolume);
        }

        private void OnDestroy()
        {
            MainMenuMusicAudioSources.Clear();
            GameplayMusicAudioSources.Clear();
            SoundFXAudioSources.Clear();
        }
    }
}
