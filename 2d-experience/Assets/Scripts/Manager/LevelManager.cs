using Runner.Scripts.Controller;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runner.Scripts.Manager
{
    public class LevelManager : MonoBehaviour
    {
        // static variables
        public static float verticalScreenSize;
        public static float horizontalScreenSize;
        public static bool isGameOn = false;
        public static bool gamePaused = false;

        [Header("Prefabs")]
        [SerializeField]
        private FloorManager _floorController = null;
        [SerializeField]
        private EnemyManager _enemyController = null;
        [SerializeField]
        private PlayerManager _playerController = null;

        [Header("Canvases")]
        [SerializeField]
        private GameObject _mainMenu = null;
        [SerializeField]
        private GameObject _pauseMenu = null;
        [SerializeField]
        private GameObject _playerHud = null;

        [Header("Texts")]
        [SerializeField]
        private TextMeshProUGUI _highScoreText = null;
        [SerializeField]
        private TextMeshProUGUI _pontuationText = null;

        [Header("Game parameters")]
        [SerializeField]
        private int _floorSpeedIncreaseInterval = 25;

        public int Pontuation { get; set; } = 0;
        private FloorManager Floor { get; set; }
        private PlayerManager Player { get; set; }
        private EnemyManager Enemies { get; set; }
        private int PreviousFloorIncreaseStep { get; set; } = 0;
        public int HighestScore { get; private set; } = 0;

        private static LevelManager _levelManager;

        public static LevelManager Instance
        {
            get
            {
                if (_levelManager == null)
                {
                    var levelManagerGameObject = new GameObject("Level Manager");
                    _levelManager = levelManagerGameObject.AddComponent<LevelManager>();
                    DontDestroyOnLoad(_levelManager);
                }

                return _levelManager;
            }
            private set
            {
                _levelManager = value;
            }
        }

        // Use this for initialization
        void Awake()
        {
            // Camera detected size
            verticalScreenSize = Camera.main.orthographicSize;
            horizontalScreenSize = (verticalScreenSize * Screen.width) / Screen.height;

            Floor = Instantiate(_floorController);
            Enemies = Instantiate(_enemyController);
            Player = Instantiate(_playerController);

            FloorManager.OnFloorEnd += OnFloorMovement;
            EnemyController.OnPlayerCollision += OnPlayerDeath;
        }

        public void LoadScene(string sceneName, Action onComplete)
        {
            // Loading screen
            _levelManager.StartCoroutine(LoadSceneAsync(sceneName, () =>
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

        public void OnGameStart()
        {
            Enemies.DestroyAllEnemies();
            Floor.ResetFloorSpeed();
            Player.SetupPlayerOnScene();

            Pontuation = 0;
            isGameOn = true;

            ToggleMainMenuVisibility(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPausePress();
            }
            else if (!isGameOn)
            {
                //ToggleMainMenuVisibility(true);
            }
        }

        public void ToggleMainMenuVisibility(bool visibility)
        {
            _mainMenu.SetActive(visibility);
            _playerHud.SetActive(!visibility);
        }

        public void OnPausePress()
        {
            gamePaused = !gamePaused;
            _pauseMenu.SetActive(gamePaused);
        }

        public static bool IsGamePaused()
        {
            return !isGameOn || gamePaused;
        }

        private void OnFloorMovement(float tilePositionX, float tilePositionY)
        {
            Enemies.SetupEnemies(tilePositionX, tilePositionY);
            Pontuation++;
            _pontuationText.text = $"Points: : {Pontuation}";

            if (Pontuation == PreviousFloorIncreaseStep + _floorSpeedIncreaseInterval)
            {
                //FloorManager.IncreaseFloorSpeed();
                PreviousFloorIncreaseStep = Pontuation;
            }
        }

        private void OnPlayerDeath()
        {
            isGameOn = false;
            Player.KillPlayer();

            if (Pontuation > HighestScore)
            {
                HighestScore = Pontuation;
            }

            _highScoreText.text = $"High Score: {HighestScore}";
        }

        public void OnGameQuit()
        {
            Application.Quit();
        }
    }
}
