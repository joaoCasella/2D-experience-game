using Runner.Scripts.Manager;
using UnityEngine;

namespace Runner.Scripts.Controller
{
    public class FloorController : MonoBehaviour
    {
        public Vector2 Size => new Vector2(BoxCollider.size.x * transform.localScale.x, BoxCollider.size.y * transform.localScale.y);

        private const float InitialSpeed = 6f;
        private const float MaxSpeed = 16.2f;
        private const float SpeedStep = 0.6f;

        // Being static, all the floors can change velocity at once
        private static float s_speed = InitialSpeed;

        [field: SerializeField]
        private BoxCollider2D BoxCollider { get; set; }

        // Update is called once per frame
        private void Update()
        {
            if (GameManager.Instance.IsGamePaused())
                return;

            transform.position += s_speed * Time.deltaTime * Vector3.left;
        }

        public static void IncreaseFloorSpeed()
        {
            if (s_speed >= MaxSpeed)
                return;

            s_speed += SpeedStep;
        }

        public static void SetupInitialFloorSpeed()
        {
            s_speed = InitialSpeed;
        }
    }
}
