using Runner.Scripts.Manager;
using System;
using System.Collections;
using UnityEngine;

namespace Runner.Scripts.Controller
{
    public class PlayerController : MonoBehaviour
    {
        public static float jumpSoundVolume = .5f;

        [Header("Components")]
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _audioClip;
        [SerializeField]
        private Rigidbody2D _rigidbody2D;

        [Header("Player parameters")]
        [SerializeField]
        private float _jumpDuration = 4.5f;
        [SerializeField]
        private float _jumpHeight = 2f;
        [SerializeField]
        private float _jumpAnimationSpeed = 0.23f;
        [SerializeField]
        private float _runSpeed = 1f;

        private bool _isBlocked = false;
        private readonly float _playerDeathAnimationHeight = 5f;

        // Allows to accelerate the jump duration
        private float JumpTimescale { get; set; } = 1f;
        private float JumpDuration => _jumpDuration;
        private float JumpHeight => _jumpHeight;
        private float JumpAnimationSpeed => _jumpAnimationSpeed;
        private float RunSpeed => _runSpeed;

        // Use this for initialization
        void Start()
        {
            _animator.SetFloat("runSpeed", RunSpeed);
            _animator.SetFloat("jumpSpeed", JumpAnimationSpeed);
        }

        // Update is called once per frame
        void Update()
        {
            ToggleRunningState();

            if (Input.GetKeyDown(KeyCode.Space) && !_isBlocked)
            {
                _audioSource.PlayOneShot(_audioClip, jumpSoundVolume);

                StartJump();
            }
        }

        private void StartJump()
        {
            _isBlocked = true;
            _animator.SetTrigger("jumpTrigger");

            StartCoroutine(Jump());
        }

        // TODO: refactor jump logic to make it fell better
        // (06/03/2022) Based on "Math for Game Programmers: Building a Better Jump", available at: https://www.youtube.com/watch?v=hG9SzQxaCm8
        // pos += velocity * deltaTime + ((acceleration * (deltaTime ^ 2)) / 2)
        // velocity  += acceleration * deltaTime
        // f(t) = (((g * t) ^ 2) / 2) + v0 * t + p0
        // v0 = 2 * Height * Vx / xh
        // g = -2 * Height * (Vx ^ 2) / (xh ^ 2)
        // where:
        // p0 is the initial position (usually, 0)
        // v0 is the vertical velocity
        // Height is the vertical position in the top of the parabola
        // vx is the initial horizontal velocity
        // xh is the horizontal position in the top of the parabola
        private IEnumerator Jump()
        {
            Vector2 currentPosition = transform.position;
            Vector2 destinationPosition = currentPosition + JumpHeight * Vector2.up;

            float step = JumpTimescale / JumpDuration;
            float t = 0;
            while (t < 1f)
            {
                t += step * Time.deltaTime;
                transform.position = Vector2.Lerp(currentPosition, destinationPosition, t);
                yield return null;
            }
            transform.position = destinationPosition;

            t = 0;
            while (t < 1f)
            {
                t += (step * 1.5f * Time.deltaTime);
                transform.position = Vector2.Lerp(destinationPosition, currentPosition, t);
                yield return null;
            }
            transform.position = currentPosition;

            _isBlocked = false;
        }

        public void ToggleRunningState()
        {
            bool isGamePaused = GameManager.IsGamePaused();
            _isBlocked = isGamePaused;
            _animator.SetBool("playerIdle", isGamePaused);
        }

        public void OnDeath(Action onComplete)
        {
            StopAllCoroutines();
            StartCoroutine(Death(onComplete));
        }

        private IEnumerator Death(Action onComplete)
        {
            _animator.SetTrigger("deathTrigger");

            transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * _playerDeathAnimationHeight, ForceMode2D.Impulse);

            Destroy(GetComponent<BoxCollider2D>());

            while (transform.position.y > -GameManager.verticalScreenSize)
            {
                yield return null;
            }

            Destroy(transform.gameObject);

            onComplete();
        }
    }
}
