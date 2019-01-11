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
    private GameObject floor;
    private GameObject player;
    private GameObject enemies;
    public int pontuation = 0;
    public Text pontuationText;
    public int floorSpeedIncreaseInterval = 25;
    private int previousFloorIncreaseStep = 0;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject playerHud;
    private int maxPoints = 0;
    public Text highScoreText;

    // Use this for initialization
    void Awake () {
        // Camera detected size
        verticalScreenSize = (float) Camera.main.orthographicSize;
        horizontalScreenSize = (verticalScreenSize * (float) Screen.width) / (float) Screen.height;

        floor = Instantiate(floorController.gameObject);
        enemies = Instantiate(enemyController.gameObject);
        player = Instantiate(playerController.gameObject);

        FloorController.OnFloorEnd += OnFloorMovement;
        Enemy.OnPlayerCollision += OnPlayerDeath;
    }

    public void OnGameStart()
    {
        enemies.GetComponent<EnemyController>().DestroyAllEnemies();
        floor.GetComponent<FloorController>().ResetFloorSpeed();

        player.GetComponent<PlayerController>().SetupPlayerOnScene();

        pontuation = 0;

        isGameOn = true;
        DisplayMainMenu(false);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPausePress();
        } else if(!isGameOn)
        {
            DisplayMainMenu(true);
        }
    }

    public void DisplayMainMenu(bool isGameOver)
    {
        mainMenu.SetActive(isGameOver);
        playerHud.SetActive(!isGameOver);
    }

    public void OnPausePress()
    {
        gamePaused = !gamePaused;
        pauseMenu.SetActive(gamePaused);
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

        if(pontuation > maxPoints)
        {
            maxPoints = pontuation;
        }

        highScoreText.text = "High Score: " + maxPoints;
    }

    public void OnGameQuit()
    {
        Application.Quit();
    }
}
