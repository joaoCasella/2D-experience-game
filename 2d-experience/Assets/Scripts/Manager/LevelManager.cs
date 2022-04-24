using Runner.Scripts.Controller;
using System;
using TMPro;
using UnityEngine;

namespace Runner.Scripts.Manager
{
    public class LevelManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private SpriteMask _gameMask = null;
        [SerializeField]
        private BackgroundController _backgroundController = null;
        [SerializeField]
        private Transform _levelContainer = null;

        [Header("Prefabs")]
        [SerializeField]
        private FloorManager _floorController = null;
        [SerializeField]
        private EnemyManager _enemyController = null;
        [SerializeField]
        private PlayerManager _playerController = null;

        [Header("Game parameters")]
        [SerializeField]
        private int _floorSpeedIncreaseInterval = 25;

        private FloorManager Floor { get; set; }
        private PlayerManager Player { get; set; }
        private EnemyManager Enemies { get; set; }
        private int PreviousFloorIncreaseStep { get; set; } = 0;
        private Action<int> OnPontuationChanged { get; set; }

        public void Setup(Action<int> onPontuationChanged)
        {
            OnPontuationChanged = onPontuationChanged;

            var gameMaskBoundsSize = _gameMask.sprite.bounds.size;

            _gameMask.transform.localScale = new Vector3(
                2f * GameManager.halfHorizontalScreenSize / gameMaskBoundsSize.x,
                2f * GameManager.halfVerticalScreenSize / gameMaskBoundsSize.y,
                _gameMask.transform.localScale.z);

            _levelContainer.localScale = new Vector3(
                2f * GameManager.halfHorizontalScreenSize,
                2f * GameManager.halfVerticalScreenSize,
                _levelContainer.localScale.z);

            _backgroundController.SetupBackgroundSize();

            Floor = Instantiate(_floorController, _levelContainer, true);
            Enemies = Instantiate(_enemyController, _levelContainer, true);
            Player = Instantiate(_playerController, _levelContainer, true);

            FloorManager.OnFloorEnd += OnFloorMovement;
            EnemyController.OnPlayerCollision += OnPlayerDeath;

            Enemies.DestroyAllEnemies();
            Floor.Setup();
            Floor.ResetFloorSpeed();
            Player.SetupPlayerOnScene();

            GameManager.Instance.Pontuation = 0;
            GameManager.isGameOn = true;
        }

        private void OnFloorMovement(Transform floor, float floorVerticalSize)
        {
            Enemies.SetupEnemies(floor, floorVerticalSize);
            GameManager.Instance.Pontuation++;
            OnPontuationChanged?.Invoke(GameManager.Instance.Pontuation);

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
