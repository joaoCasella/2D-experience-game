using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runner.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        public const string gameName = "Runner";

        // TODO: see if, with this class as a singleton, we should
        // still use static variables
        // static variables
        public static float verticalScreenSize;
        public static float horizontalScreenSize;
        public static bool isGameOn = false;
        public static bool gamePaused = false;

        public int Pontuation { get; set; } = 0;
        public int HighestScore { get; set; } = 0;

        private static GameManager _gameManager;

        public static GameManager Instance
        {
            get
            {
                if (_gameManager == null)
                {
                    var levelManagerGameObject = new GameObject("GameManager", typeof(GameManager));
                    _gameManager = levelManagerGameObject.GetComponent<GameManager>();
                    DontDestroyOnLoad(_gameManager.gameObject);
                }

                return _gameManager;
            }
        }

        // Use this for initialization
        void Awake()
        {
            // Camera detected size
            verticalScreenSize = Camera.main.orthographicSize;
            horizontalScreenSize = (verticalScreenSize * Screen.width) / Screen.height;
        }

        public void LoadScene(string sceneName, Action onComplete)
        {
            // Loading screen
            _gameManager.StartCoroutine(LoadSceneAsync(sceneName, () =>
            {
                Debug.Log($"[GameManager] Scene {sceneName} loaded");
                onComplete();
            }));
        }

        private IEnumerator LoadSceneAsync(string sceneName, Action onComplete)
        {
            var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (!async.isDone)
            {
                Debug.Log($"[GameManager] Loading {sceneName}, progress: {async.progress * 100f}%");
                yield return null;
            }

            onComplete();
        }

        public void OnPausePress()
        {
            gamePaused = !gamePaused;
        }

        public static bool IsGamePaused()
        {
            return !isGameOn || gamePaused;
        }

        public void OnGameQuit()
        {
            Application.Quit();
        }
    }
}
