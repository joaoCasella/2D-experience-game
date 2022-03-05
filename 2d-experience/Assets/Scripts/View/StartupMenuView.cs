using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Scripts.View
{
    public class StartupMenuView : MonoBehaviour
    {
        [Header("Texts")]
        [SerializeField]
        private TextMeshProUGUI _gameNameText = null;

        [SerializeField]
        private TextMeshProUGUI _highScoreText = null;

        [SerializeField]
        private Button _startGameButton = null;

        [SerializeField]
        private Button _quitGameButton = null;

        public void Setup(string gameName, string highScore, Action onClickStartGame, Action onClickQuitGame)
        {
            _gameNameText.text = gameName;
            _highScoreText.text = highScore;
            _startGameButton.onClick.AddListener(() => onClickStartGame());
            _quitGameButton.onClick.AddListener(() => onClickQuitGame());
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
