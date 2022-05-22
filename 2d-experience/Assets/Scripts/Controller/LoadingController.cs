using UnityEngine;

namespace Runner.Scripts.Controller
{
    public class LoadingController : MonoBehaviour
    {
        private static LoadingController _loadingController;
        public static LoadingController Instance => _loadingController;

        [field: SerializeField]
        private Animator Animator { get; set; }

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
            Animator.Play("Loading");
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
