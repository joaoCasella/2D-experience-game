using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static float verticalScreenSize;
    public static float horizontalScreenSize;
    public static bool isGameOn = true;
    public Transform floorController;
    public Transform enemyController;

    // Use this for initialization
    void Start () {
        // Camera detected size
        verticalScreenSize = (float) Camera.main.orthographicSize;
        horizontalScreenSize = (verticalScreenSize * (float) Screen.width) / (float) Screen.height;

        GameObject floor = Instantiate(floorController.gameObject);
        GameObject enemies = Instantiate(enemyController.gameObject);

        FloorController.OnFloorEnd += enemies.GetComponent<EnemyController>().SetupEnemies;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGameOn = !isGameOn;
        }
    }
}
