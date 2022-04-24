using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Scripts.View
{
    public class PauseMenuView : MonoBehaviour
    {
        [field: SerializeField]
        private Button ContinueButton { get; set; }

        [field: SerializeField]
        private Button QuitButton { get; set; }

        public void Setup(Action onClickContinue, Action onClickQuit)
        {
            ContinueButton.onClick.AddListener(() => onClickContinue());
            QuitButton.onClick.AddListener(() => onClickQuit());
        }
    }
}
