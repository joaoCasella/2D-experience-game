using Runner.Scripts.Controller.UI;
using Runner.Scripts.Manager;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Runner.Scripts.Controller.Scene
{
    public class StartSceneController : MonoBehaviour
    {
        [field: SerializeField]
        private StartupMenuController StartupMenu { get; set; }

        [field: SerializeField]
        private AudioSource StartupMenuAudioSource { get; set; }

        private void Awake()
        {
            SoundManager.Instance.RegisterMusicSource(Domain.MusicType.MainMenu, StartupMenuAudioSource);
        }

        private IEnumerator Start()
        {
            StartupMenu.HideContent();
            LoadingController.Instance.ShowProgressBar(0f);

            var asyncOp = LocalizationSettings.InitializationOperation;

            while (!asyncOp.IsDone)
            {
                LoadingController.Instance.UpdateProgressBar(asyncOp.PercentComplete);
                yield return null;
            }

            LoadingController.Instance.Hide();
            StartupMenu.Setup();
        }

        private void OnDestroy()
        {
            SoundManager.Instance.DeregisterMusicSource(Domain.MusicType.MainMenu, StartupMenuAudioSource);
        }
    }
}
