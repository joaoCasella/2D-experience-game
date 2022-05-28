using Runner.Scripts.Domain;
using UnityEngine;

namespace Runner.Scripts.Service
{
    public static class ConfigurationService
    {
        private const string MainMenuMusicVolumeKey = "B5DA1137-759D-4C0E-938B-B0E7E68DA804";
        private const string GameplayMusicVolumeKey = "B388A825-3B22-4B95-9597-D260D892C3FE";
        private const string SoundFXVolumeKey = "3D9757E8-BECF-4C76-977E-2CBB7C9427FD";
        private const string ShowCooldownKey = "AE5FC30D-1437-4727-9959-BE8F29B80825";

        private static string MusicKey(MusicType musicType) => musicType == MusicType.MainMenu ? MainMenuMusicVolumeKey : GameplayMusicVolumeKey;

        public static float GetSavedMusicVolume(MusicType musicType)
        {
            return PlayerPrefs.GetFloat(MusicKey(musicType), 0.6f);
        }

        public static void SaveMusicVolume(MusicType musicType, float newMusicVolume)
        {
            PlayerPrefs.SetFloat(MusicKey(musicType), newMusicVolume);
        }

        public static float GetSavedSoundFXVolume()
        {
            return PlayerPrefs.GetFloat(SoundFXVolumeKey, 0.6f);
        }

        public static void SaveSoundFXVolume(float newSoundFXVolume)
        {
            PlayerPrefs.SetFloat(SoundFXVolumeKey, newSoundFXVolume);
        }

        public static bool GetSavedShowCooldown()
        {
            return PlayerPrefs.GetInt(ShowCooldownKey, 1) == 1;
        }

        public static void SaveShowCooldown(bool showCooldown)
        {
            PlayerPrefs.SetInt(ShowCooldownKey, showCooldown ? 1 : 0);
        }
    }
}
