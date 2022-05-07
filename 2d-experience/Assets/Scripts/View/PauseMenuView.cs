﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Scripts.View
{
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
        private Button BackButton { get; set; }

        public void Setup(
            float startMusicVolume,
            float startSoundFXVolume,
            Action onClickContinue,
            Action onClickQuit,
            Action onClickConfigurations,
            Action onClickBack,
            Action<Configurations> onSoundConfigurationsChanged)
        {
            ContinueButton.onClick.AddListener(() => onClickContinue());
            ConfigurationsButton.onClick.AddListener(() => onClickConfigurations());
            QuitButton.onClick.AddListener(() => onClickQuit());

            MusicSlider.value = startMusicVolume;
            SoundFXSlider.value = startSoundFXVolume;

            MusicSlider.onValueChanged.AddListener((value) => onSoundConfigurationsChanged(GetConfigurations()));
            SoundFXSlider.onValueChanged.AddListener((value) => onSoundConfigurationsChanged(GetConfigurations()));

            BackButton.onClick.AddListener(() => onClickBack());
        }

        public Configurations GetConfigurations()
        {
            return new Configurations() { MusicVolume = MusicSlider.normalizedValue, SoundFXVolume = SoundFXSlider.normalizedValue };
        }

        public void ShowScreen(bool defaultMenu)
        {
            DefaultMenuContainer.SetActive(defaultMenu);
            ConfigurationsMenuContainer.SetActive(!defaultMenu);
        }
    }
}
