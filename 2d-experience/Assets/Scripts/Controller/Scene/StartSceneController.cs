using Runner.Scripts.Manager;
using Runner.Scripts.View;
using UnityEngine;

namespace Runner.Scripts.Controller.Scene
{
    public class StartSceneController : MonoBehaviour
    {
        [SerializeField]
        private StartupMenuView _startupMenu = null;

        // Use this for initialization
        void Start()
        {
            _startupMenu.Setup(
                GameManager.gameName,
                $"High Score: {GameManager.Instance.HighestScore}",
                OnClickStart,
                OnClickQuit);
        }

        public void OnClickStart()
        {
            // Load gameplay scene
            _startupMenu.Hide();
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
