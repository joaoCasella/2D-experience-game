using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Queue<Transform> enemiesQueue = new Queue<Transform>();
    public Transform[] enemies;
    private int numberTilesToSpawnEnemy = 1;
    private System.Random randomNumberGenerator = new System.Random();
    private int numberTilesBetweenEnemies = 5;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void SetupEnemies(float tilePositionX, float tilePositionY)
    {
        DeleteEnemiesOffScreen();
        SpawnNewEnemy(tilePositionX, tilePositionY);
    }

    private void DeleteEnemiesOffScreen()
    {
        if (enemiesQueue.Count > 0 && 
            enemiesQueue.Peek().position.x < (-LevelManager.horizontalScreenSize - (enemiesQueue.Peek().GetComponent<BoxCollider2D>().size.x * 1.5f)))
        {
            Transform enemyDead = enemiesQueue.Dequeue();

            if(enemyDead != null)
            {
                Destroy(enemyDead.gameObject);
            }
        }
    }

    private void SpawnNewEnemy(float tilePositionX, float tilePositionY)
    {
        if (numberTilesToSpawnEnemy >= numberTilesBetweenEnemies)
        {
            numberTilesToSpawnEnemy = 1;
            int nextEnemyIndex = randomNumberGenerator.Next(2);
            Transform enemySpawn = enemies[nextEnemyIndex];
            Vector2 enemySize = enemySpawn.GetComponent<BoxCollider2D>().size;
            Transform enemy = Instantiate(
                enemySpawn,
                new Vector2(
                    tilePositionX,
                    tilePositionY + (enemySize.y * 3f)
                ),
                Quaternion.identity
            );
            enemiesQueue.Enqueue(enemy);
        }
        else
        {
            numberTilesToSpawnEnemy++;
        }
    }

    public void DestroyAllEnemies()
    {
        foreach(Transform enemy in enemiesQueue)
        {
            Destroy(enemy.gameObject);
        }
        enemiesQueue.Clear();
    }
}
