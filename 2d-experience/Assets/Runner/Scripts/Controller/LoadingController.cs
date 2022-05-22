using UnityEngine;
using UnityEngine.UI;

namespace Runner.Scripts.Controller
{
    public class LoadingController : MonoBehaviour
    {
        private static LoadingController _loadingController;
        public static LoadingController Instance => _loadingController;

        [field: SerializeField]
        private GameObject TextContainer { get; set; }

        [field: SerializeField]
        private Animator TextAnimator { get; set; }

        [field: SerializeField]
        private GameObject ProgressBarContainer { get; set; }

        [field: SerializeField]
        private Slider ProgressBar { get; set; }

        private void Awake()
        {
            if (_loadingController == null)
            {
                _loadingController = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_loadingController != this)
            {
                Destroy(gameObject);
                return;
            }

            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            TextContainer.SetActive(true);
            ProgressBarContainer.SetActive(false);
            TextAnimator.Play("Loading");
        }

        public void ShowProgressBar(float startProgress)
        {
            gameObject.SetActive(true);
            TextContainer.SetActive(false);
            ProgressBarContainer.SetActive(true);
            UpdateProgressBar(startProgress);
        }

        public void UpdateProgressBar(float progress)
        {
            ProgressBar.value = Mathf.Clamp01(progress);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
