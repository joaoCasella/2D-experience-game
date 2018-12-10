using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public Queue<Transform> floor = new Queue<Transform>();
    public Transform floorPrefab;
    private Transform lastElement;
    private float floorPrefabHorizontalSize;
    private float verticalSize;
    private float horizontalSize;
    private Transform firstTile;

    // Use this for initialization
    void Start () {
        // Camera detected size
        verticalSize = (float) Camera.main.orthographicSize;
        horizontalSize = (verticalSize * (float) Screen.width) / (float) Screen.height;

        // Floor tile size
        Vector2 floorSize = floorPrefab.GetComponent<BoxCollider2D>().size;
        floorPrefabHorizontalSize = floorSize.x * 5f;
        float tilePositionHorizontalOffset = (-1f * horizontalSize) + (floorSize.x * 2.5f);
        float tilePositionVerticalOffset = (-1f * verticalSize) + (floorSize.y * 2f);

        float sumGameObjectHorizontalSize = 0f;

        // Added two more tiles to guarantee that the replacement does not show on screen
        float totalScreenSize = (horizontalSize * 2f) + (2f * floorPrefabHorizontalSize);

        Transform currentElement = null;

        while (sumGameObjectHorizontalSize < totalScreenSize) {
            float newTileHorizontalPosition = tilePositionHorizontalOffset + sumGameObjectHorizontalSize;

            currentElement = Instantiate(floorPrefab, new Vector2(newTileHorizontalPosition, tilePositionVerticalOffset), Quaternion.identity);

            floor.Enqueue(currentElement);

            sumGameObjectHorizontalSize += floorPrefabHorizontalSize;
        }

        lastElement = currentElement;
        firstTile = floor.Peek();
	}
	
	// Update is called once per frame
	void Update () {
        if(firstTile.position.x < -(horizontalSize + floorPrefabHorizontalSize))
        {
            MoveFloor();
        }
	}

    void MoveFloor()
    {
        Transform gameObject = floor.Dequeue();
        gameObject.transform.position = new Vector2(
            lastElement.transform.position.x + floorPrefabHorizontalSize,
            lastElement.transform.position.y
        );

        floor.Enqueue(gameObject);

        lastElement = gameObject;
        firstTile = floor.Peek();
    }
}
