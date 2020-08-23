using Runner.Scripts.Manager;
using UnityEngine;

namespace Runner.Scripts.Controller
{
    public class FloorController : MonoBehaviour
    {
        public static float speed;

        private static float maxSpeed = 0.01f;
        private static float speedStep = 0.01f;

        // Update is called once per frame
        void Update()
        {
            if (!LevelManager.IsGamePaused())
            {
                transform.Translate(speed * Vector2.left);
            }

        }

        public static void IncreaseFloorSpeed()
        {
            if (speed < maxSpeed)
            {
                speed += speedStep;
            }
        }

        public static void SetupInitialFloorSpeed()
        {
            speed = 0.01f;
        }
    }
}
