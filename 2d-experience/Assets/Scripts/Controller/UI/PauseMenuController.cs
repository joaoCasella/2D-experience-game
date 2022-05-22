using Runner.Scripts.Inputter;
using Runner.Scripts.Manager;
using Runner.Scripts.Service;
using Runner.Scripts.View;
using System;
using System.Collections;
using UnityEngine;

namespace Runner.Scripts.Controller.UI
{
    public class PauseMenuController : MonoBehaviour, IInputListener
    {
        public enum PauseMenuState
        {
            Default = 0,
            Configurations = 1,
            Cooldown = 2,
        }

        const int CooldownTimeInSeconds = 3;

        [field: Header("References")]
        [field: SerializeField]
        private PauseMenuView PauseMenu { get; set; }

        public InputListenerPriority Priority => InputListenerPriority.Overlay;
        private Action OnClickContinue { get; set; }
        private PauseMenuState MenuState { get; set; }
        private Coroutine CountdownCoroutine { get; set; }

        public void Setup(Action onClickContinue)
        {
            OnClickContinue = onClickContinue;

            PauseMenu.Setup(
                SoundManager.Instance.MusicVolume,
                SoundManager.Instance.SoundFXVolume,
                ConfigurationService.GetSavedShowCooldown(),
                OnContinue,
                GameManager.Instance.OnGameQuit,
                () => SetMenuState(PauseMenuState.Configurations),
                OnBackFromSettingsScreen,
                OnSoundConfigurationsChanged,
                OnCooldownConfigurationsChanged);

            SetMenuState(PauseMenuState.Default);

            InputManager.Instance.RegisterInputListener(this);
        }

        private void SetMenuState(PauseMenuState menuState)
        {
            PauseMenu.ShowScreen((PauseMenuVisualState) (int) menuState);
            MenuState = menuState;
        }

        private void OnBackFromSettingsScreen()
        {
            SaveConfigurations(PauseMenu.GetConfigurations());
            SetMenuState(PauseMenuState.Default);
        }

        private void SaveConfigurations(VisualConfigurations configurations)
        {
            SoundManager.Instance.SaveSoundConfigurations(configurations.MusicVolume, configurations.SoundFXVolume);
        }

        private void OnSoundConfigurationsChanged(VisualConfigurations configurations)
        {
            SoundManager.Instance.OnSoundConfigurationsChanged(configurations.MusicVolume, configurations.SoundFXVolume);
        }

        private void OnCooldownConfigurationsChanged(VisualConfigurations configurations)
        {
            ConfigurationService.SaveShowCooldown(configurations.ShowCooldown);
        }

        private void OnContinue()
        {
            void afterCooldown()
            {
                SetMenuState(PauseMenuState.Default);
                OnClickContinue();
            }

            if (!ConfigurationService.GetSavedShowCooldown())
            {
                afterCooldown();
                return;
            }

            SetMenuState(PauseMenuState.Cooldown);
            CountdownCoroutine = StartCoroutine(ShowCooldownAnimation(onComplete: afterCooldown));
        }

        private IEnumerator ShowCooldownAnimation(Action onComplete)
        {
            var wait = new WaitForSecondsRealtime(1f);
            for (int i = CooldownTimeInSeconds; i > 0; i--)
            {
                PauseMenu.SetCooldownText(i.ToString());
                yield return wait;
            }
            onComplete?.Invoke();
        }

        public bool ConsumeInput(InputAction inputAction)
        {
            if (inputAction != InputAction.BackOrPause || this == null || gameObject == null || !gameObject.activeInHierarchy)
                return false;

            OnBackPress();
            return true;
        }

        public void OnBackPress()
        {
            if (MenuState == PauseMenuState.Default)
            {
                OnContinue();
            }
            else if (MenuState == PauseMenuState.Configurations)
            {
                OnBackFromSettingsScreen();
            }
        }

        private void OnDestroy()
        {
            InputManager.Instance.DeregisterInputListener(this);

            if (CountdownCoroutine != null)
                StopCoroutine(CountdownCoroutine);
        }
    }
}
