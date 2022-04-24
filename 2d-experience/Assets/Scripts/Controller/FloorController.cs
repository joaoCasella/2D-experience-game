using Runner.Scripts.Manager;
using UnityEngine;

namespace Runner.Scripts.Controller
{
    public class FloorController : MonoBehaviour
    {
        public Vector2 Size => new Vector2(BoxCollider.size.x * transform.localScale.x, BoxCollider.size.y * transform.localScale.y);

        public static float speed = initialSpeed;
        private static readonly float initialSpeed = 6f;
        private static readonly float maxSpeed = 16.2f;
        private static readonly float speedStep = 0.6f;

        [field: SerializeField]
        private BoxCollider2D BoxCollider { get; set; }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.IsGamePaused())
                return;

            transform.position += speed * Time.deltaTime * Vector3.left;
        }

        public static void IncreaseFloorSpeed()
        {
            if (speed >= maxSpeed)
                return;

            speed += speedStep;
        }

        public static void SetupInitialFloorSpeed()
        {
            speed = initialSpeed;
        }
    }
}
