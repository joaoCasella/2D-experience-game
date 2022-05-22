using Runner.Scripts.Manager;
using Runner.Scripts.View;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Runner.Scripts.Controller.UI
{
    public class StartupMenuController : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField]
        private StartupMenuView StartMenu { get; set; }

        public void Setup()
        {
            StartMenu.Setup(OnClickStart, OnClickQuit);
            SetupLanguagesDropdown();
        }

        public void HideContent()
        {
            StartMenu.HideContent();
        }

        // (14/05/2022) Following unity tutorial, available at: https://docs.unity3d.com/Packages/com.unity.localization@0.4/manual/index.html
        private void SetupLanguagesDropdown()
        {
            StartMenu.SetupLanguagesDropdown(
                LocalizationSettings.AvailableLocales.Locales.Select(locale => locale.LocaleName).ToList(),
                LocalizationSettings.SelectedLocale.LocaleName,
                (selectedIndex) => LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selectedIndex]);
        }

        public void OnClickStart()
        {
            // Load gameplay scene
            StartMenu.HideContent();
            LoadingController.Instance.Show();
            GameManager.Instance.LoadScene("MainScene", () =>
            {
                LoadingController.Instance.Hide();
            });
        }

        public void OnClickQuit()
        {
            GameManager.Instance.OnGameQuit();
        }
    }
}
