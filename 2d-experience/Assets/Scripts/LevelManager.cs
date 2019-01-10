using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    public static float verticalScreenSize;
    public static float horizontalScreenSize;
    public static bool isGameOn = false;
    public static bool gamePaused = false;
    public Transform floorController;
    public Transform enemyController;
    public Transform playerController;
    private GameObject player;
    private GameObject enemies;
    public static int pontuation = 0;
    public Text pontuationText;
    public int floorSpeedIncreaseInterval = 25;
    private int previousFloorIncreaseStep = 0;

    // Use this for initialization
    void Awake () {
        // Camera detected size
        verticalScreenSize = (float) Camera.main.orthographicSize;
        horizontalScreenSize = (verticalScreenSize * (float) Screen.width) / (float) Screen.height;

        Instantiate(floorController.gameObject);
        
        enemies = Instantiate(enemyController.gameObject);

        FloorController.OnFloorEnd += OnFloorMovement;
        Enemy.OnPlayerCollision += OnPlayerDeath;
    }

    void Start()
    {
        player = Instantiate(playerController.gameObject);
        player.GetComponent<PlayerController>().SetupPlayerOnScene();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
        }
        // provisory to test the separated logic between pause and new game
        else if (Input.GetKeyDown(KeyCode.A))
        {
            isGameOn = !isGameOn;
        }
    }

    public static bool IsGamePaused()
    {
        return !isGameOn || gamePaused;
    }

    void OnFloorMovement(float tilePositionX, float tilePositionY)
    {
        enemies.GetComponent<EnemyController>().SetupEnemies(tilePositionX, tilePositionY);
        pontuation++;
        pontuationText.text = "Points: " + pontuation;

        if (pontuation == previousFloorIncreaseStep + floorSpeedIncreaseInterval)
        {
            FloorController.IncreaseFloorSpeed();
            previousFloorIncreaseStep = pontuation;
        }
    }

    void OnPlayerDeath()
    {
        isGameOn = false;
        player.GetComponent<PlayerController>().KillPlayer();
    }
}
