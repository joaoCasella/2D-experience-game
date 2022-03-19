using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runner.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        public const string gameName = "Runner";
        public const float nativeGameHeight = 1080f;
        public const float nativeGameWidth = 1920f;

        // TODO: see if, with this class as a singleton, we should
        // still use static variables
        // static variables
        public static float halfVerticalScreenSize;
        public static float halfHorizontalScreenSize;
        public static float cameraScaleFactor = 1f;
        public static bool isGameOn = false;
        public static bool gamePaused = false;
        public static float GameTimescale => Time.timeScale;
        private static float previousTimescale = 1f;

        public int Pontuation { get; set; } = 0;
        public int HighestScore { get; set; } = 0;

        public delegate void GamePaused(bool paused);
        public event GamePaused OnGamePaused;

        private static GameManager _gameManager;
        public static GameManager Instance => _gameManager;

        // Use this for initialization
        void Awake()
        {
            if (_gameManager == null)
            {
                _gameManager = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_gameManager != this)
            {
                Destroy(gameObject);
                return;
            }

            // Adjust the camera ortographic size to prioritize the screen length
            var aspectRatio = (float) Screen.width / Screen.height;
            var nativeAspectRatio = nativeGameWidth / nativeGameHeight;

            // If current aspect ratio is lesser than the native one,
            // the game will match the game width and insert black bars
            // on the vertical spaces left
            if (aspectRatio < nativeAspectRatio)
            {
                // Camera ortographic size should be modified to contain
                // the width, expanding the height accordingly
                cameraScaleFactor = nativeAspectRatio / aspectRatio;
            }

            var mainCamera = Camera.main;

            // Solution based on the thread: https://answers.unity.com/questions/620699/scaling-my-background-sprite-to-fill-screen-2d-1.html
            // Acessed in: 22/08/2020
            // Original screen size (keeps the proportions)
            halfVerticalScreenSize = mainCamera.orthographicSize;
            halfHorizontalScreenSize = halfVerticalScreenSize * nativeAspectRatio;

            mainCamera.orthographicSize *= cameraScaleFactor;
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

            var currentTimescale = Time.timeScale;
            var newTimescale = gamePaused ? 0f : previousTimescale;
            previousTimescale = currentTimescale;
            Time.timeScale = newTimescale;

            OnGamePaused(gamePaused);
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
