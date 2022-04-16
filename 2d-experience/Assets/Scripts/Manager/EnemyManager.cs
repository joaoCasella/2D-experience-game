using Runner.Scripts.Controller;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Scripts.Manager
{
    public class EnemyManager : MonoBehaviour
    {
        [field: SerializeField]
        private EnemyController[] Enemies { get; set; }

        private Queue<EnemyController> EnemiesQueue { get; set; } = new Queue<EnemyController>();
        private int NumberTilesToSpawnEnemy { get; set; } = 1;
        private System.Random RandomNumberGenerator { get; set; } = new System.Random();
        private int NumberTilesBetweenEnemies { get; set; } = 5;

        public void SetupEnemies(Transform floor, float floorVerticalSize)
        {
            DeleteEnemiesOffScreen();
            SpawnNewEnemy(floor, floorVerticalSize);
        }

        private void DeleteEnemiesOffScreen()
        {
            if (EnemiesQueue.Count <= 0)
                return;

            var enemy = EnemiesQueue.Peek();

            if (enemy.transform.position.x >= -(GameManager.halfHorizontalScreenSize + enemy.Size.x))
                return;

            var enemyDead = EnemiesQueue.Dequeue();

            if (enemyDead != null)
            {
                Destroy(enemyDead.gameObject);
            }
        }

        private void SpawnNewEnemy(Transform floor, float floorVerticalSize)
        {
            if (NumberTilesToSpawnEnemy < NumberTilesBetweenEnemies)
            {
                NumberTilesToSpawnEnemy++;
                return;
            }

            NumberTilesToSpawnEnemy = 1;
            var nextEnemyIndex = RandomNumberGenerator.Next(Enemies.Length);
            var enemySpawned = Enemies[nextEnemyIndex];
            var enemy = Instantiate(enemySpawned, floor);
            enemy.transform.localPosition = new Vector3(
                -enemy.PositionOffset.x,
                (floorVerticalSize * 0.5f / floor.localScale.y) + enemy.Size.y * 0.5f - enemy.PositionOffset.y,
                0);
            EnemiesQueue.Enqueue(enemy);
        }

        public void DestroyAllEnemies()
        {
            foreach (var enemy in EnemiesQueue)
            {
                Destroy(enemy.gameObject);
            }
            EnemiesQueue.Clear();
        }
    }
}
