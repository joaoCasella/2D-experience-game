using Runner.Scripts.Manager;
using UnityEngine;

namespace Runner.Scripts.Controller
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        public delegate void OnCollision();
        public static event OnCollision OnPlayerCollision;

        // Update is called once per frame
        void Update()
        {
            _animator.speed = GameManager.IsGamePaused() ? 0.6f : 1f;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                OnPlayerCollision();
            }
            else if (collision.gameObject.CompareTag("Floor"))
            {
                transform.parent = collision.transform;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                transform.parent = null;
            }
        }
    } 
}
