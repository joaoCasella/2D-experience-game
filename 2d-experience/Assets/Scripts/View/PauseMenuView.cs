using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Scripts.View
{
    public enum PauseMenuVisualState
    {
        Default = 0,
        Configurations = 1,
        Cooldown = 2,
    }

    public class PauseMenuView : MonoBehaviour
    {
        [field: SerializeField]
        private GameObject DefaultMenuContainer { get; set; }

        [field: SerializeField]
        private Button ContinueButton { get; set; }

        [field: SerializeField]
        private Button ConfigurationsButton { get; set; }

        [field: SerializeField]
        private Button QuitButton { get; set; }

        [field: SerializeField]
        private GameObject ConfigurationsMenuContainer { get; set; }

        [field: SerializeField]
        private Slider MusicSlider { get; set; }

        [field: SerializeField]
        private Slider SoundFXSlider { get; set; }

        [field: SerializeField]
        private Toggle ShowCooldownToggle { get; set; }

        [field: SerializeField]
        private Button BackButton { get; set; }

        [field: SerializeField]
        private GameObject CooldownScreenContainer { get; set; }

        [field: SerializeField]
        private TextMeshProUGUI CooldownText { get; set; }

        public void Setup(
            float startMusicVolume,
            float startSoundFXVolume,
            bool showCooldownValue,
            Action onClickContinue,
            Action onClickQuit,
            Action onClickConfigurations,
            Action onClickBack,
            Action<VisualConfigurations> onSoundConfigurationsChanged,
            Action<VisualConfigurations> onCooldownConfigurationsChanged)
        {
            ContinueButton.onClick.AddListener(() => onClickContinue());
            ConfigurationsButton.onClick.AddListener(() => onClickConfigurations());
            QuitButton.onClick.AddListener(() => onClickQuit());

            MusicSlider.value = startMusicVolume;
            SoundFXSlider.value = startSoundFXVolume;
            ShowCooldownToggle.isOn = showCooldownValue;

            MusicSlider.onValueChanged.AddListener((value) => onSoundConfigurationsChanged(GetConfigurations()));
            SoundFXSlider.onValueChanged.AddListener((value) => onSoundConfigurationsChanged(GetConfigurations()));
            ShowCooldownToggle.onValueChanged.AddListener((value) => onCooldownConfigurationsChanged(GetConfigurations()));

            BackButton.onClick.AddListener(() => onClickBack());
        }

        public VisualConfigurations GetConfigurations()
        {
            return new VisualConfigurations()
            {
                MusicVolume = MusicSlider.normalizedValue,
                SoundFXVolume = SoundFXSlider.normalizedValue,
                ShowCooldown = ShowCooldownToggle.isOn,
            };
        }

        public void ShowScreen(PauseMenuVisualState visualState)
        {
            DefaultMenuContainer.SetActive(visualState == PauseMenuVisualState.Default);
            ConfigurationsMenuContainer.SetActive(visualState == PauseMenuVisualState.Configurations);
            CooldownScreenContainer.SetActive(visualState == PauseMenuVisualState.Cooldown);
        }

        public void SetCooldownText(string text)
        {
            CooldownText.text = text;
        }
    }
}
