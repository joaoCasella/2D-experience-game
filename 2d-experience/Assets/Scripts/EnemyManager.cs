using System.Collections.Generic;
using UnityEngine;

namespace Runner.Scripts.Manager
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _enemies;

        private Queue<Transform> EnemiesQueue { get; set; } = new Queue<Transform>();
        private int NumberTilesToSpawnEnemy { get; set; } = 1;
        private System.Random RandomNumberGenerator { get; set; } = new System.Random();
        private int NumberTilesBetweenEnemies { get; set; } = 5;

        public void SetupEnemies(float tilePositionX, float tilePositionY)
        {
            DeleteEnemiesOffScreen();
            SpawnNewEnemy(tilePositionX, tilePositionY);
        }

        private void DeleteEnemiesOffScreen()
        {
            if (EnemiesQueue.Count > 0 &&
                EnemiesQueue.Peek().position.x < (-LevelManager.horizontalScreenSize - (EnemiesQueue.Peek().GetComponent<BoxCollider2D>().size.x * 1.5f)))
            {
                Transform enemyDead = EnemiesQueue.Dequeue();

                if (enemyDead != null)
                {
                    Destroy(enemyDead.gameObject);
                }
            }
        }

        private void SpawnNewEnemy(float tilePositionX, float tilePositionY)
        {
            if (NumberTilesToSpawnEnemy >= NumberTilesBetweenEnemies)
            {
                NumberTilesToSpawnEnemy = 1;
                int nextEnemyIndex = RandomNumberGenerator.Next(2);
                Transform enemySpawn = _enemies[nextEnemyIndex];
                Vector2 enemySize = enemySpawn.GetComponent<BoxCollider2D>().size;
                Transform enemy = Instantiate(
                    enemySpawn,
                    new Vector2(
                        tilePositionX,
                        tilePositionY + (enemySize.y * 3f)
                    ),
                    Quaternion.identity
                );
                EnemiesQueue.Enqueue(enemy);
            }
            else
            {
                NumberTilesToSpawnEnemy++;
            }
        }

        public void DestroyAllEnemies()
        {
            foreach (Transform enemy in EnemiesQueue)
            {
                Destroy(enemy.gameObject);
            }
            EnemiesQueue.Clear();
        }
    }
}
