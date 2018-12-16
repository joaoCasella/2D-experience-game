using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour {
    private Queue<Transform> floor = new Queue<Transform>();
    public Transform floorPrefab;
    private Transform lastFloorTile;
    private float floorPrefabHorizontalSize;
    private Transform firstFloorTile;
    private Vector2 floorSize;
    public delegate void OnFloorCycle(float xPosition, float yPosition);
    public static event OnFloorCycle OnFloorEnd;

    // Use this for initialization
    void Start () {
        // Floor tile size
        floorSize = floorPrefab.GetComponent<BoxCollider2D>().size;
        floorPrefabHorizontalSize = floorSize.x * 5f;
        float tilePositionHorizontalOffset = (-1f * LevelManager.horizontalScreenSize) + (floorSize.x * 2.5f);
        float tilePositionVerticalOffset = (-1f * LevelManager.verticalScreenSize) + (floorSize.y * 2f);

        float sumGameObjectHorizontalSize = 0f;

        // Added two more tiles to guarantee that the replacement does not show on screen
        float totalScreenSize = (LevelManager.horizontalScreenSize * 2f) + (2f * floorPrefabHorizontalSize);

        Transform currentElement = null;

        while (sumGameObjectHorizontalSize < totalScreenSize) {
            float newTileHorizontalPosition = tilePositionHorizontalOffset + sumGameObjectHorizontalSize;

            currentElement = Instantiate(floorPrefab, new Vector2(newTileHorizontalPosition, tilePositionVerticalOffset), Quaternion.identity);

            floor.Enqueue(currentElement);

            sumGameObjectHorizontalSize += floorPrefabHorizontalSize;
        }

        lastFloorTile = currentElement;
        firstFloorTile = floor.Peek();
	}
	
	// Update is called once per frame
	void Update () {
        if(firstFloorTile.position.x < -(LevelManager.horizontalScreenSize + floorSize.x * 2f) && LevelManager.isGameOn)
        {
            RecycleMovingFloorComponent();
        }
    }

    void RecycleMovingFloorComponent()
    {
        Transform gameObject = floor.Dequeue();

        float endPositionX = lastFloorTile.transform.position.x + floorPrefabHorizontalSize;
        float endPositionY = lastFloorTile.transform.position.y;

        OnFloorEnd(endPositionX, endPositionY + floorSize.y * 2.5f);

        gameObject.transform.position = new Vector2(endPositionX, endPositionY);

        floor.Enqueue(gameObject);

        lastFloorTile = gameObject;
        firstFloorTile = floor.Peek();
    }
}
