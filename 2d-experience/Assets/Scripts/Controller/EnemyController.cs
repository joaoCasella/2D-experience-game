using Runner.Scripts.Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runner.Scripts.Controller
{
    public class EnemyController : MonoBehaviour
    {
        public Vector2 Size => new Vector2(
            BoxCollider.size.x * transform.localScale.x,
            BoxCollider.size.y * transform.localScale.y);
        public Vector2 PositionOffset => new Vector2(
            BoxCollider.offset.x * transform.localScale.x,
            BoxCollider.offset.y * transform.localScale.y);

        [field: SerializeField]
        private BoxCollider2D BoxCollider { get; set; }

        [field: SerializeField]
        [field: FormerlySerializedAs("_animator")]
        private Animator Animator { get; set; }

        public delegate void OnCollision();
        public static event OnCollision OnPlayerCollision;

        // Update is called once per frame
        void Update()
        {
            Animator.speed = GameManager.IsGamePaused() ? 0.6f : 1f;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                OnPlayerCollision();
            }
        }
    } 
}
