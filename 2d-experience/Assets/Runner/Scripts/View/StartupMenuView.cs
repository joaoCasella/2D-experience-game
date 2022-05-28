using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Runner.Scripts.View
{
    public class StartupMenuView : MonoBehaviour
    {
        [field: SerializeField]
        private GameObject ContentContainer { get; set; }

        [field: SerializeField]
        private TextMeshProUGUI HighestScoreText { get; set; }

        [field: SerializeField]
        private LocalizedString HighestScoreString { get; set; }

        [field: SerializeField]
        private Button StartGameButton { get; set; }

        [field: SerializeField]
        private Button SettingsMenuButton { get; set; }

        [field: SerializeField]
        private Button QuitGameButton { get; set; }

        private int HighestScore { get; set; }

        public void Setup(
            int highestScore,
            Action onClickStartGame,
            Action onClickSettings,
            Action onClickQuitGame)
        {
            HighestScore = highestScore;

            ContentContainer.SetActive(true);
            StartGameButton.onClick.AddListener(() => onClickStartGame());
            SettingsMenuButton.onClick.AddListener(() => onClickSettings());
            QuitGameButton.onClick.AddListener(() => onClickQuitGame());

            UpdateString(null);
            HighestScoreString.StringChanged += UpdateString;
        }

        private void UpdateString(string s)
        {
            HighestScoreText.text = HighestScoreString.GetLocalizedString(HighestScore);
        }

        public void ToggleActiveState(bool active)
        {
            ContentContainer.SetActive(active);
        }

        private void OnDestroy()
        {
            HighestScoreString.StringChanged -= UpdateString;
        }
    }
}
