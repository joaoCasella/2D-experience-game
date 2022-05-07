using Runner.Scripts.Controller.UI;
using Runner.Scripts.Inputter;
using Runner.Scripts.Manager;
using Runner.Scripts.View;
using UnityEngine;

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
            Camera.main.orthographicSize *= GameManager.cameraScaleFactor;

            GameUi.Setup(GameManager.nativeGameWidth / GameManager.nativeGameHeight, GameManager.Instance.Pontuation);
            PauseMenu.Setup(OnPausePress);
            ToggleUiVisibility(UiVisibility.InGameUi);

            LevelManager.Setup(GameUi.UpdatePontuationText);

            InputManager.Instance.RegisterInputListener(this);
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
