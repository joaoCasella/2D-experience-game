using Runner.Scripts.Manager;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Scripts.Controller
{
    public class FloorManager : MonoBehaviour
    {
        public static Vector2 floorSize;

        [field: Header("Prefab")]
        [field: SerializeField]
        private FloorController FloorPrefab { get; set; }

        private Queue<Transform> Floor { get; set; } = new Queue<Transform>();
        private Transform LastFloorTile { get; set; }
        private float FloorPrefabHorizontalSize { get; set; }
        private Transform FirstFloorTile { get; set; }

        // Delegates
        public delegate void OnFloorCycle(Transform floor, float floorVerticalSize);
        public static event OnFloorCycle OnFloorEnd;

        public void Setup()
        {
            // Cache floor tile size
            floorSize = FloorPrefab.Size;
            FloorPrefabHorizontalSize = floorSize.x;
            float tilePositionHorizontalOffset = (floorSize.x * 0.5f) - (GameManager.halfHorizontalScreenSize);
            float tilePositionVerticalOffset = (floorSize.y * 0.5f) - (GameManager.halfVerticalScreenSize);

            float sumGameObjectHorizontalSize = 0f;

            // Added two more tiles to guarantee that the replacement does not show on screen
            float totalScreenSize = (GameManager.halfHorizontalScreenSize + FloorPrefabHorizontalSize) * 2f;

            Transform currentElement = null;

            while (sumGameObjectHorizontalSize < totalScreenSize)
            {
                float newTileHorizontalPosition = tilePositionHorizontalOffset + sumGameObjectHorizontalSize;

                currentElement = Instantiate(FloorPrefab,
                    new Vector2(newTileHorizontalPosition, tilePositionVerticalOffset),
                    Quaternion.identity,
                    transform).transform;

                Floor.Enqueue(currentElement);

                sumGameObjectHorizontalSize += FloorPrefabHorizontalSize;
            }

            LastFloorTile = currentElement;
            FirstFloorTile = Floor.Peek();
        }

        // Update is called once per frame
        void Update()
        {
            if (FirstFloorTile.position.x < -(GameManager.halfHorizontalScreenSize + floorSize.x) && !GameManager.IsGamePaused())
            {
                RecycleMovingFloorComponent();
            }
        }

        private void RecycleMovingFloorComponent()
        {
            Transform floor = Floor.Dequeue();

            float endPositionX = LastFloorTile.transform.position.x + FloorPrefabHorizontalSize;
            float endPositionY = LastFloorTile.transform.position.y;

            OnFloorEnd(floor, floorSize.y);

            floor.transform.position = new Vector2(endPositionX, endPositionY);

            Floor.Enqueue(floor);

            LastFloorTile = floor;
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
