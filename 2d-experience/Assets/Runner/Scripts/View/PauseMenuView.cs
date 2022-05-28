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
        private GameObject CooldownScreenContainer { get; set; }

        [field: SerializeField]
        private TextMeshProUGUI CooldownText { get; set; }

        public void Setup(
            Action onClickContinue,
            Action onClickQuit,
            Action onClickConfigurations)
        {
            ContinueButton.onClick.AddListener(() => onClickContinue());
            ConfigurationsButton.onClick.AddListener(() => onClickConfigurations());
            QuitButton.onClick.AddListener(() => onClickQuit());
        }

        public void ShowScreen(PauseMenuVisualState visualState)
        {
            DefaultMenuContainer.SetActive(visualState == PauseMenuVisualState.Default);
            CooldownScreenContainer.SetActive(visualState == PauseMenuVisualState.Cooldown);
        }

        public void SetCooldownText(string text)
        {
            CooldownText.text = text;
        }
    }
}
