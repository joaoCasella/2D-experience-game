using Runner.Scripts.Manager;
using Runner.Scripts.View;
using UnityEngine;

namespace Runner.Scripts.Controller.Scene
{
    public class MainSceneController : MonoBehaviour
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
        private PauseMenuView PauseMenu { get; set; }

        private void Start()
        {
            // TODO: make an automatic setup
            Camera.main.orthographicSize *= GameManager.cameraScaleFactor;

            GameUi.Setup(GameManager.Instance.Pontuation);
            PauseMenu.Setup(OnPausePress, GameManager.Instance.OnGameQuit);
            ToggleUiVisibility(UiVisibility.InGameUi);

            LevelManager.Setup(GameUi.UpdatePontuationText);

            InputManager.Instance.OnInputPause += OnPausePress;
        }

        public void OnPausePress()
        {
            GameManager.Instance.OnPausePress();
            ToggleUiVisibility(GameManager.gamePaused ? UiVisibility.PauseMenu : UiVisibility.InGameUi);
        }

        private void ToggleUiVisibility(UiVisibility uiVisibility)
        {
            PauseMenu.gameObject.SetActive(uiVisibility == UiVisibility.PauseMenu);
            GameUi.gameObject.SetActive(uiVisibility == UiVisibility.InGameUi);
        }

        private void OnDestroy()
        {
            InputManager.Instance.OnInputPause -= OnPausePress;
        }
    }
}
