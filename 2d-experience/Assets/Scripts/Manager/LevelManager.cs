using Runner.Scripts.Controller;
using TMPro;
using UnityEngine;

namespace Runner.Scripts.Manager
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField]
        private FloorManager _floorController = null;
        [SerializeField]
        private EnemyManager _enemyController = null;
        [SerializeField]
        private PlayerManager _playerController = null;

        [Header("Canvases")]
        [SerializeField]
        private GameObject _pauseMenu = null;
        [SerializeField]
        private GameObject _playerHud = null;

        [Header("Texts")]
        [SerializeField]
        private TextMeshProUGUI _pontuationText = null;

        [Header("Game parameters")]
        [SerializeField]
        private int _floorSpeedIncreaseInterval = 25;

        private FloorManager Floor { get; set; }
        private PlayerManager Player { get; set; }
        private EnemyManager Enemies { get; set; }
        private int PreviousFloorIncreaseStep { get; set; } = 0;

        public void Setup()
        {
            Floor = Instantiate(_floorController);
            Enemies = Instantiate(_enemyController);
            Player = Instantiate(_playerController);

            FloorManager.OnFloorEnd += OnFloorMovement;
            EnemyController.OnPlayerCollision += OnPlayerDeath;

            Enemies.DestroyAllEnemies();
            Floor.ResetFloorSpeed();
            Player.SetupPlayerOnScene();

            GameManager.Instance.Pontuation = 0;
            GameManager.isGameOn = true;

            ToggleMainMenuVisibility(false);
        }

        // TODO: decide if this will stay here
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPausePress();
            }
            else if (!GameManager.isGameOn)
            {
                ToggleMainMenuVisibility(true);
            }
        }

        public void ToggleMainMenuVisibility(bool visibility)
        {
            _playerHud.SetActive(!visibility);
        }

        public void OnPausePress()
        {
            GameManager.Instance.OnPausePress();
            _pauseMenu.SetActive(GameManager.gamePaused);
        }

        private void OnFloorMovement(float tilePositionX, float tilePositionY)
        {
            Enemies.SetupEnemies(tilePositionX, tilePositionY);
            GameManager.Instance.Pontuation++;
            _pontuationText.text = $"Points: : {GameManager.Instance.Pontuation}";

            if (GameManager.Instance.Pontuation == PreviousFloorIncreaseStep + _floorSpeedIncreaseInterval)
            {
                FloorManager.IncreaseFloorSpeed();
                PreviousFloorIncreaseStep = GameManager.Instance.Pontuation;
            }
        }

        private void OnPlayerDeath()
        {
            GameManager.isGameOn = false;

            if (GameManager.Instance.Pontuation > GameManager.Instance.HighestScore)
            {
                GameManager.Instance.HighestScore = GameManager.Instance.Pontuation;
            }

            Player.KillPlayer(() =>
            {
                LoadingController.Instance.Show();
                GameManager.Instance.LoadScene("StartScene", () =>
                {
                    LoadingController.Instance.Hide();
                });
            });
        }

        private void OnDestroy()
        {
            FloorManager.OnFloorEnd -= OnFloorMovement;
            EnemyController.OnPlayerCollision -= OnPlayerDeath;
        }
    }
}
