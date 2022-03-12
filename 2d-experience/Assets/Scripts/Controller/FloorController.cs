using Runner.Scripts.Manager;
using UnityEngine;

namespace Runner.Scripts.Controller
{
    public class FloorController : MonoBehaviour
    {
        public static float speed;

        private static readonly float initialSpeed = 0.015f;
        private static readonly float maxSpeed = 0.06f;
        private static readonly float speedStep = 0.0025f;

        // Update is called once per frame
        void Update()
        {
            if (GameManager.IsGamePaused())
                return;

            transform.position += speed * Vector3.left;
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
