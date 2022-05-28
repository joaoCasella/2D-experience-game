using Runner.Scripts.Inputter;
using Runner.Scripts.Manager;
using Runner.Scripts.View;
using UnityEngine;

namespace Runner.Scripts.Controller.UI
{
    public class StartupMenuController : MonoBehaviour, IInputListener
    {
        public enum StartupMenuState
        {
            Loading = 0,
            Default = 1,
            Configurations = 2,
        }

        [field: Header("References")]
        [field: SerializeField]
        private StartupMenuView StartMenu { get; set; }

        [field: SerializeField]
        private SettingsMenuController SettingsMenu { get; set; }

        private StartupMenuState MenuState { get; set; }
        public InputListenerPriority Priority => InputListenerPriority.Overlay;

        public void Setup()
        {
            StartMenu.Setup(GameManager.Instance.HighestScore, OnClickStart, OnClickSettings, OnClickQuit);
            SettingsMenu.Setup(OnBackPress);
        }

        public void HideContent()
        {
            ChangeState(StartupMenuState.Loading);
        }

        public void OnClickStart()
        {
            // Load gameplay scene
            ChangeState(StartupMenuState.Loading);
            LoadingController.Instance.Show();
            GameManager.Instance.LoadScene("MainScene", () =>
            {
                LoadingController.Instance.Hide();
            });
        }

        public void OnClickSettings()
        {
            ChangeState(StartupMenuState.Configurations);
        }

        public void OnClickQuit()
        {
            GameManager.Instance.OnGameQuit();
        }

        public bool ConsumeInput(InputAction inputAction)
        {
            if (inputAction != InputAction.BackOrPause || this == null || gameObject == null || !gameObject.activeInHierarchy)
                return false;

            OnBackPress();
            return true;
        }

        private void OnBackPress()
        {
            if (MenuState != StartupMenuState.Configurations)
                return;

            ChangeState(StartupMenuState.Default);
        }

        private void ChangeState(StartupMenuState menuState)
        {
            MenuState = menuState;
            StartMenu.ToggleActiveState(MenuState == StartupMenuState.Default);
            SettingsMenu.ToggleActiveState(MenuState == StartupMenuState.Configurations);
        }
    }
}
