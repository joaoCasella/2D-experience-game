using Runner.Scripts.Inputter;
using Runner.Scripts.Manager;
using Runner.Scripts.View;
using System;
using UnityEngine;

namespace Runner.Scripts.Controller.UI
{
    public class PauseMenuController : MonoBehaviour, IInputListener
    {
        private enum PauseMenuState
        {
            Default,
            Configurations
        }

        [field: Header("References")]
        [field: SerializeField]
        private PauseMenuView PauseMenu { get; set; }

        public InputListenerPriority Priority => InputListenerPriority.Overlay;
        private Action OnClickContinue { get; set; }
        private PauseMenuState MenuState { get; set; }

        public void Setup(Action onClickContinue)
        {
            OnClickContinue = onClickContinue;

            PauseMenu.Setup(
                SoundManager.Instance.MusicVolume,
                SoundManager.Instance.SoundFXVolume,
                OnClickContinue,
                GameManager.Instance.OnGameQuit,
                () => SetMenuState(PauseMenuState.Configurations),
                OnBackFromSettingsScreen,
                OnSoundConfigurationsChanged);

            SetMenuState(PauseMenuState.Default);

            InputManager.Instance.RegisterInputListener(this);
        }

        private void SetMenuState(PauseMenuState menuState)
        {
            PauseMenu.ShowScreen(defaultMenu: menuState == PauseMenuState.Default);
            MenuState = menuState;
        }

        private void OnBackFromSettingsScreen()
        {
            SaveConfigurations(PauseMenu.GetConfigurations());
            SetMenuState(PauseMenuState.Default);
        }

        private void SaveConfigurations(Configurations configurations)
        {
            SoundManager.Instance.SaveSoundConfigurations(configurations.MusicVolume, configurations.SoundFXVolume);
        }

        private void OnSoundConfigurationsChanged(Configurations configurations)
        {
            SoundManager.Instance.OnSoundConfigurationsChanged(configurations.MusicVolume, configurations.SoundFXVolume);
        }

        public bool ConsumeInput(InputAction inputAction)
        {
            if (inputAction != InputAction.BackOrPause)
                return false;

            OnBackPress();
            return true;
        }

        public void OnBackPress()
        {
            if (MenuState == PauseMenuState.Default)
            {
                OnClickContinue();
            }
            else if (MenuState == PauseMenuState.Configurations)
            {
                OnBackFromSettingsScreen();
            }
        }

        private void OnDestroy()
        {
            InputManager.Instance.DeregisterInputListener(this);
        }
    }
}
