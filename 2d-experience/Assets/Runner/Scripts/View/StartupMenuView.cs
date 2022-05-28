using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Scripts.View
{
    public class StartupMenuView : MonoBehaviour
    {
        [field: SerializeField]
        private GameObject ContentContainer { get; set; }

        [field: SerializeField]
        private Button StartGameButton { get; set; }

        [field: SerializeField]
        private Button SettingsMenuButton { get; set; }

        [field: SerializeField]
        private Button QuitGameButton { get; set; }

        public void Setup(Action onClickStartGame, Action onClickSettings, Action onClickQuitGame)
        {
            ContentContainer.SetActive(true);
            StartGameButton.onClick.AddListener(() => onClickStartGame());
            SettingsMenuButton.onClick.AddListener(() => onClickSettings());
            QuitGameButton.onClick.AddListener(() => onClickQuitGame());
        }

        public void ToggleActiveState(bool active)
        {
            ContentContainer.SetActive(active);
        }
    }
}
