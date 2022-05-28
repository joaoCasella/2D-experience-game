using Runner.Scripts.Service;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.SceneManagement;

namespace Runner.Scripts.Manager
{
    public class GameManager : Inputter.Inputter
    {
        public const float NativeGameHeight = 1080f;
        public const float NativeGameWidth = 1920f;

        private float s_previousTimescale = 1f;
        public float HalfVerticalScreenSize { get; private set; }
        public float HalfHorizontalScreenSize { get; private set; }
        public float CameraScaleFactor { get; private set; } = 1f;
        public bool IsGameOn { get; set; } = false;
        public bool GamePaused { get; private set; } = false;

        public int Pontuation { get; set; } = 0;
        public int HighestScore
        {
            get
            {
                return PlayerService.GetSavedHighestScore();
            }
            private set
            {
                PlayerService.SaveHighestScore(value);
            }
        }

        public delegate void GamePausedEvent(bool paused);
        public event GamePausedEvent OnGamePaused;

        private static GameManager _gameManager;
        public static GameManager Instance => _gameManager;

        // Use this for initialization
        private void Awake()
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
            var aspectRatio = (float)Screen.width / Screen.height;
            var nativeAspectRatio = NativeGameWidth / NativeGameHeight;

            // If current aspect ratio is lesser than the native one,
            // the game will match the game width and insert black bars
            // on the vertical spaces left
            if (aspectRatio < nativeAspectRatio)
            {
                // Camera ortographic size should be modified to contain
                // the width, expanding the height accordingly
                CameraScaleFactor = nativeAspectRatio / aspectRatio;
            }

            var mainCamera = Camera.main;

            // Solution based on the thread: https://answers.unity.com/questions/620699/scaling-my-background-sprite-to-fill-screen-2d-1.html
            // Acessed in: 22/08/2020
            // Original screen size (keeps the proportions)
            HalfVerticalScreenSize = mainCamera.orthographicSize;
            HalfHorizontalScreenSize = HalfVerticalScreenSize * nativeAspectRatio;

            mainCamera.orthographicSize *= CameraScaleFactor;
        }

        private void Update()
        {
            // On Android, will only make sense if there is a connected keyboard
            if (Input.GetKeyDown(KeyCode.Space))
            {
                InputDetected(Inputter.InputAction.Action);
            }
            // On Android, detects system back presses
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                InputDetected(Inputter.InputAction.BackOrPause);
            }
        }

        public void LoadScene(string sceneName, Action onComplete)
        {
            // Loading screen
            Instance.StartCoroutine(LoadSceneAsync(sceneName, () =>
            {
                Debug.Log($"[GameManager] Scene {sceneName} loaded");
                onComplete();
            }));
        }

        private IEnumerator LoadSceneAsync(string sceneName, Action onComplete)
        {
            var asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (!asyncOp.isDone)
            {
                Debug.Log($"[GameManager] Loading {sceneName}, progress: {asyncOp.progress * 100f}%");
                yield return null;
            }

            onComplete();
        }

        public void OnPausePress()
        {
            Instance.GamePaused = !Instance.GamePaused;

            var currentTimescale = Time.timeScale;
            var newTimescale = Instance.GamePaused ? 0f : s_previousTimescale;
            s_previousTimescale = currentTimescale;
            Time.timeScale = newTimescale;

            OnGamePaused(Instance.GamePaused);
        }

        public bool IsGamePaused()
        {
            return !Instance.IsGameOn || Instance.GamePaused;
        }

        public void OnGameQuit()
        {
            Application.Quit();
        }

        public void UpdateHighestScore()
        {
            if (Instance.Pontuation <= Instance.HighestScore)
                return;

            Instance.HighestScore = Instance.Pontuation;
        }

        public void CurrentGameStarted()
        {
            Instance.Pontuation = 0;
            Instance.IsGameOn = true;
        }

        public void CurrentGameEnded()
        {
            Instance.IsGameOn = false;
            Instance.UpdateHighestScore();
        }
    }
}
