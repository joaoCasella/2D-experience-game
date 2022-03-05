using Runner.Scripts.Manager;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Scripts.Controller
{
    public class FloorManager : MonoBehaviour
    {
        public static Vector2 floorSize;

        [Header("Prefab")]
        [SerializeField]
        private Transform _floorPrefab;

        private Queue<Transform> Floor { get; set; } = new Queue<Transform>();
        private Transform LastFloorTile { get; set; }
        private float FloorPrefabHorizontalSize { get; set; }
        private Transform FirstFloorTile { get; set; }

        // Delegates
        public delegate void OnFloorCycle(float xPosition, float yPosition);
        public static event OnFloorCycle OnFloorEnd;

        // Use this for initialization
        void Awake()
        {
            // Floor tile size
            floorSize = _floorPrefab.GetComponent<BoxCollider2D>().size;
            FloorPrefabHorizontalSize = floorSize.x * 5f;
            float tilePositionHorizontalOffset = (floorSize.x * 2.5f) - GameManager.horizontalScreenSize;
            float tilePositionVerticalOffset = (floorSize.y * 2f) - GameManager.verticalScreenSize;

            float sumGameObjectHorizontalSize = 0f;

            // Added two more tiles to guarantee that the replacement does not show on screen
            float totalScreenSize = (GameManager.horizontalScreenSize + FloorPrefabHorizontalSize) * 2f;

            Transform currentElement = null;

            while (sumGameObjectHorizontalSize < totalScreenSize)
            {
                float newTileHorizontalPosition = tilePositionHorizontalOffset + sumGameObjectHorizontalSize;

                currentElement = Instantiate(_floorPrefab, new Vector2(newTileHorizontalPosition, tilePositionVerticalOffset), Quaternion.identity, transform);

                Floor.Enqueue(currentElement);

                sumGameObjectHorizontalSize += FloorPrefabHorizontalSize;
            }

            LastFloorTile = currentElement;
            FirstFloorTile = Floor.Peek();
        }

        // Update is called once per frame
        void Update()
        {
            if (FirstFloorTile.position.x < -(GameManager.horizontalScreenSize + floorSize.x * 2f) && !GameManager.IsGamePaused())
            {
                RecycleMovingFloorComponent();
            }
        }

        private void RecycleMovingFloorComponent()
        {
            Transform gameObject = Floor.Dequeue();

            float endPositionX = LastFloorTile.transform.position.x + FloorPrefabHorizontalSize;
            float endPositionY = LastFloorTile.transform.position.y;

            OnFloorEnd(endPositionX, endPositionY + floorSize.y * 2.5f);

            gameObject.transform.position = new Vector2(endPositionX, endPositionY);

            Floor.Enqueue(gameObject);

            LastFloorTile = gameObject;
            FirstFloorTile = Floor.Peek();
        }

        public static void IncreaseFloorSpeed()
        {
            FloorController.IncreaseFloorSpeed();
        }

        public void ResetFloorSpeed()
        {
            FloorController.SetupInitialFloorSpeed();
        }
    }
}
