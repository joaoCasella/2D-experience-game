using UnityEngine;

namespace Runner.Scripts.Service
{
    public static class ConfigurationService
    {
        private const string MusicVolumeKey = "B388A825-3B22-4B95-9597-D260D892C3FE";
        private const string SoundFXVolumeKey = "3D9757E8-BECF-4C76-977E-2CBB7C9427FD";
        private const string ShowCooldownKey = "AE5FC30D-1437-4727-9959-BE8F29B80825";

        public static float GetSavedMusicVolume()
        {
            return PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        }

        public static void SaveMusicVolume(float newMusicVolume)
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, newMusicVolume);
        }

        public static float GetSavedSoundFXVolume()
        {
            return PlayerPrefs.GetFloat(SoundFXVolumeKey, 1f);
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
