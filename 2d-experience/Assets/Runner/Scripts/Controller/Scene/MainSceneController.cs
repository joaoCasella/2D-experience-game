﻿using Runner.Scripts.Controller.UI;
using Runner.Scripts.Inputter;
using Runner.Scripts.Manager;
using Runner.Scripts.Service;
using Runner.Scripts.View;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Runner.Scripts.Controller.Scene
{
    public class MainSceneController : MonoBehaviour, IInputListener
    {
        private enum UiVisibility
        {
            InGameUi = 0,
            PauseMenu = 1,
        }

        [field: Header("References")]
        [field: SerializeField]
        private LevelManager LevelManager { get; set; }

        [field: SerializeField]
        private InGameUiView GameUi { get; set; }

        [field: SerializeField]
        private PauseMenuController PauseMenu { get; set; }

        public InputListenerPriority Priority => InputListenerPriority.Gameplay;

        private void Start()
        {
            // TODO: make an automatic setup
            Camera.main.orthographicSize *= GameManager.Instance.CameraScaleFactor;

            LevelManager.Setup(OnPontuationChanged);

            GameUi.Setup(GameManager.Instance.Pontuation, GameManager.NativeGameWidth / GameManager.NativeGameHeight, (float) Screen.width / Screen.height);
            PauseMenu.Setup(OnPausePress);
            ToggleUiVisibility(UiVisibility.InGameUi);

            InputManager.Instance.RegisterInputListener(this);
        }

        private void OnPontuationChanged(int pontuation)
        {
            GameUi.UpdateCurrentScore(pontuation);
        }

        public void OnPausePress()
        {
            GameManager.Instance.OnPausePress();
            ToggleUiVisibility(GameManager.Instance.GamePaused ? UiVisibility.PauseMenu : UiVisibility.InGameUi);
        }

        private void ToggleUiVisibility(UiVisibility uiVisibility)
        {
            PauseMenu.gameObject.SetActive(uiVisibility == UiVisibility.PauseMenu);
            GameUi.gameObject.SetActive(uiVisibility == UiVisibility.InGameUi);
        }

        public bool ConsumeInput(InputAction inputAction)
        {
            if (inputAction != InputAction.BackOrPause)
                return false;

            OnPausePress();
            return true;
        }

        private void OnDestroy()
        {
            InputManager.Instance.DeregisterInputListener(this);
        }
    }
}
