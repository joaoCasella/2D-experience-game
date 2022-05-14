using Runner.Scripts.Controller.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Runner.Scripts.Controller.Scene
{
    public class StartSceneController : MonoBehaviour
    {
        [field: SerializeField]
        private StartupMenuController StartupMenu { get; set; }

        private IEnumerator Start()
        {
            StartupMenu.HideContent();
            LoadingController.Instance.Show();

            yield return LocalizationSettings.InitializationOperation;

            LoadingController.Instance.Hide();
            StartupMenu.Setup();
        }
    }
}
