using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Transform player;
    private GameObject currentPlayer;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void SetupPlayerOnScene()
    {
        Vector2 playerSize = player.GetComponent<BoxCollider2D>().size;
        currentPlayer = Instantiate(
            player.gameObject, 
            new Vector2(
                -LevelManager.horizontalScreenSize + 9f * playerSize.x, 
                -LevelManager.verticalScreenSize + FloorController.floorSize.y * 2.5f + playerSize.y * 6f
            ), 
            Quaternion.identity
        );
    }

    public void KillPlayer()
    {
        currentPlayer.GetComponent<Player>().OnDeath();
    }
}
