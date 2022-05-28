using Runner.Scripts.Inputter;
using Runner.Scripts.Manager;
using Runner.Scripts.Service;
using Runner.Scripts.View;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Runner.Scripts.Controller.UI
{
    public class SettingsMenuController : MonoBehaviour, IInputListener
    {
        [field: Header("References")]
        [field: SerializeField]
        private SettingsMenuView SettingsMenu { get; set; }

        public InputListenerPriority Priority => InputListenerPriority.Overlay;
        public Action OnClickBack { get; set; }

        public void Setup(Action onClickBack)
        {
            OnClickBack = onClickBack;

            SettingsMenu.Setup(
                SoundManager.Instance.MainMenuMusicVolume,
                SoundManager.Instance.GameplayMusicVolume,
                SoundManager.Instance.SoundFXVolume,
                ConfigurationService.GetSavedShowCooldown(),
                OnBackPress,
                OnSoundConfigurationsChanged,
                OnCooldownConfigurationsChanged);
            SetupLanguagesDropdown();

            InputManager.Instance.RegisterInputListener(this);
        }

        private void SaveConfigurations(VisualConfigurations configurations)
        {
            SoundManager.Instance.SaveSoundConfigurations(configurations.MainMenuMusicVolume, configurations.GameplayMusicVolume, configurations.SoundFXVolume);
        }

        private void OnSoundConfigurationsChanged(VisualConfigurations configurations)
        {
            SoundManager.Instance.OnSoundConfigurationsChanged(configurations.MainMenuMusicVolume, configurations.GameplayMusicVolume, configurations.SoundFXVolume);
        }

        private void OnCooldownConfigurationsChanged(VisualConfigurations configurations)
        {
            ConfigurationService.SaveShowCooldown(configurations.ShowCooldown);
        }

        public void ToggleActiveState(bool active)
        {
            SettingsMenu.ToggleActiveState(active);
        }

        // (14/05/2022) Following unity tutorial, available at: https://docs.unity3d.com/Packages/com.unity.localization@0.4/manual/index.html
        private void SetupLanguagesDropdown()
        {
            SettingsMenu.SetupLanguagesDropdown(
                LocalizationSettings.AvailableLocales.Locales.Select(locale => locale.LocaleName).ToList(),
                LocalizationSettings.SelectedLocale.LocaleName,
                (selectedIndex) => LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selectedIndex]);
        }

        public bool ConsumeInput(InputAction inputAction)
        {
            if (inputAction != InputAction.BackOrPause || this == null || gameObject == null || !gameObject.activeInHierarchy)
                return false;

            OnBackPress();
            return true;
        }

        private void OnBackPress()
        {
            SaveConfigurations(SettingsMenu.GetConfigurations());
            OnClickBack?.Invoke();
        }

        private void OnDestroy()
        {
            InputManager.Instance.DeregisterInputListener(this);
        }
    }
}
