using Runner.Scripts.Controller;
using System;
using UnityEngine;

namespace Runner.Scripts.Manager
{
    public class LevelManager : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField]
        private SpriteMask GameMask { get; set; }

        [field: SerializeField]
        private BackgroundController BackgroundController { get; set; }

        [field: SerializeField]
        private Transform LevelContainer { get; set; }

        [field: Header("Prefabs")]
        [field: SerializeField]
        private FloorManager FloorController { get; set; }

        [field: SerializeField]
        private EnemyManager EnemyController { get; set; }

        [field: SerializeField]
        private PlayerManager PlayerController { get; set; }

        [field: Header("Game parameters")]
        [field: SerializeField]
        private int FloorSpeedIncreaseInterval { get; set; } = 25;

        private FloorManager Floor { get; set; }
        private PlayerManager Player { get; set; }
        private EnemyManager Enemies { get; set; }
        private int PreviousFloorIncreaseStep { get; set; } = 0;
        private Action<int> OnPontuationChanged { get; set; }

        public void Setup(Action<int> onPontuationChanged)
        {
            OnPontuationChanged = onPontuationChanged;

            var gameMaskBoundsSize = GameMask.sprite.bounds.size;

            GameMask.transform.localScale = new Vector3(
                2f * GameManager.Instance.HalfHorizontalScreenSize / gameMaskBoundsSize.x,
                2f * GameManager.Instance.HalfVerticalScreenSize / gameMaskBoundsSize.y,
                GameMask.transform.localScale.z);

            LevelContainer.localScale = new Vector3(
                2f * GameManager.Instance.HalfHorizontalScreenSize,
                2f * GameManager.Instance.HalfVerticalScreenSize,
                LevelContainer.localScale.z);

            BackgroundController.SetupBackgroundSize();

            Floor = Instantiate(FloorController, LevelContainer, true);
            Enemies = Instantiate(EnemyController, LevelContainer, true);
            Player = Instantiate(PlayerController, LevelContainer, true);

            FloorManager.OnFloorEnd += OnFloorMovement;
            Controller.EnemyController.OnPlayerCollision += OnPlayerDeath;

            Enemies.DestroyAllEnemies();
            Floor.Setup();
            Floor.ResetFloorSpeed();
            Player.SetupPlayerOnScene();

            GameManager.Instance.CurrentGameStarted();
        }

        private void OnFloorMovement(Transform floor, float floorVerticalSize)
        {
            Enemies.SetupEnemies(floor, floorVerticalSize);
            GameManager.Instance.Pontuation++;
            OnPontuationChanged?.Invoke(GameManager.Instance.Pontuation);

            if (GameManager.Instance.Pontuation == PreviousFloorIncreaseStep + FloorSpeedIncreaseInterval)
            {
                FloorManager.IncreaseFloorSpeed();
                PreviousFloorIncreaseStep = GameManager.Instance.Pontuation;
            }
        }

        private void OnPlayerDeath()
        {
            GameManager.Instance.CurrentGameEnded();

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
            Controller.EnemyController.OnPlayerCollision -= OnPlayerDeath;
        }
    }
}
