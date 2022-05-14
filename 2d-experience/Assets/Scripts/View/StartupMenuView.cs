using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Scripts.View
{
    public class StartupMenuView : MonoBehaviour
    {
        [field: Header("Texts")]
        [field: SerializeField]
        private GameObject ContentContainer { get; set; }

        [field: SerializeField]
        private Button StartGameButton { get; set; }

        [field: SerializeField]
        private Button QuitGameButton { get; set; }

        [field: SerializeField]
        private TMP_Dropdown Dropdown { get; set; }

        public void Setup(Action onClickStartGame, Action onClickQuitGame)
        {
            ContentContainer.SetActive(true);
            StartGameButton.onClick.AddListener(() => onClickStartGame());
            QuitGameButton.onClick.AddListener(() => onClickQuitGame());
        }

        public void HideContent()
        {
            ContentContainer.SetActive(false);
        }

        // (14/05/2022) Following unity tutorial, available at: https://docs.unity3d.com/Packages/com.unity.localization@0.4/manual/index.html
        public void SetupLanguagesDropdown(List<string> localeNames, string selectedLocaleName, Action<int> onLocaleSelected)
        {
            // Generate list of available Locales
            var options = new List<TMP_Dropdown.OptionData>();
            int selected = 0;
            for (int i = 0; i < localeNames.Count; ++i)
            {
                var locale = localeNames[i];
                if (selectedLocaleName == locale)
                    selected = i;
                
                options.Add(new TMP_Dropdown.OptionData(locale));
            }
            Dropdown.options = options;

            Dropdown.value = selected;
            Dropdown.onValueChanged.AddListener((index) => onLocaleSelected(index));
        }

        private void OnDestroy()
        {
            if (Dropdown != null)
                Dropdown.onValueChanged.RemoveAllListeners();
        }
    }
}
