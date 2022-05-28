using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Scripts.View
{
    public class SettingsMenuView : MonoBehaviour
    {
        [field: SerializeField]
        private GameObject ConfigurationsMenuContainer { get; set; }

        [field: SerializeField]
        private Slider MainMenuMusicSlider { get; set; }

        [field: SerializeField]
        private Slider GameplayMusicSlider { get; set; }

        [field: SerializeField]
        private Slider SoundFXSlider { get; set; }

        [field: SerializeField]
        private Toggle ShowCooldownToggle { get; set; }

        [field: SerializeField]
        private TMP_Dropdown Dropdown { get; set; }

        [field: SerializeField]
        private Button BackButton { get; set; }

        public void Setup(
            float startMainMenuVolume,
            float startGameplayMusicVolume,
            float startSoundFXVolume,
            bool showCooldownValue,
            Action onClickBack,
            Action<VisualConfigurations> onSoundConfigurationsChanged,
            Action<VisualConfigurations> onCooldownConfigurationsChanged)
        {
            MainMenuMusicSlider.value = startMainMenuVolume;
            GameplayMusicSlider.value = startGameplayMusicVolume;
            SoundFXSlider.value = startSoundFXVolume;
            ShowCooldownToggle.isOn = showCooldownValue;

            MainMenuMusicSlider.onValueChanged.AddListener((value) => onSoundConfigurationsChanged(GetConfigurations()));
            GameplayMusicSlider.onValueChanged.AddListener((value) => onSoundConfigurationsChanged(GetConfigurations()));
            SoundFXSlider.onValueChanged.AddListener((value) => onSoundConfigurationsChanged(GetConfigurations()));
            ShowCooldownToggle.onValueChanged.AddListener((value) => onCooldownConfigurationsChanged(GetConfigurations()));

            BackButton.onClick.AddListener(() => onClickBack());
        }

        public VisualConfigurations GetConfigurations()
        {
            return new VisualConfigurations()
            {
                MainMenuMusicVolume = MainMenuMusicSlider.normalizedValue,
                GameplayMusicVolume = GameplayMusicSlider.normalizedValue,
                SoundFXVolume = SoundFXSlider.normalizedValue,
                ShowCooldown = ShowCooldownToggle.isOn,
            };
        }

        public void ToggleActiveState(bool active)
        {
            ConfigurationsMenuContainer.SetActive(active);
        }

        // (14/05/2022) Following unity tutorial, available at: https://docs.unity3d.com/Packages/com.unity.localization@0.4/manual/index.html
        public void SetupLanguagesDropdown(List<string> localeNames, string selectedLocaleName, Action<int> onLocaleSelected)
        {
            // Generate list of available Locales
            var options = new List<TMP_Dropdown.OptionData>();
            int selected = 0;
            for (int i = 0; i < localeNames.Count; ++i)
            {
                var locale = localeNames[i];
                if (selectedLocaleName == locale)
                    selected = i;

                options.Add(new TMP_Dropdown.OptionData(locale));
            }
            Dropdown.options = options;

            Dropdown.value = selected;
            Dropdown.onValueChanged.AddListener((index) => onLocaleSelected(index));
        }

        private void OnDestroy()
        {
            if (Dropdown != null)
                Dropdown.onValueChanged.RemoveAllListeners();
        }
    }
}
