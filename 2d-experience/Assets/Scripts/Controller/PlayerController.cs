using Runner.Scripts.Manager;
using System;
using System.Collections;
using UnityEngine;

namespace Runner.Scripts.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private const float JumpSoundVolume = 0.5f;
        public Vector2 Size => new Vector2(
            BoxCollider.size.x * transform.localScale.x,
            BoxCollider.size.y * transform.localScale.y);
        public Vector2 PositionOffset => new Vector2(
            BoxCollider.offset.x * transform.localScale.x,
            BoxCollider.offset.y * transform.localScale.y);

        [field: Header("Components")]
        [field: SerializeField]
        private Animator Animator { get; set; }

        [field: SerializeField]
        private AudioSource AudioSource { get; set; }

        [field: SerializeField]
        public BoxCollider2D BoxCollider { get; private set; }


        [field: Header("Audio clip")]
        [field: SerializeField]
        private AudioClip JumpSound;

        [field: Header("Player parameters")]
        [field: SerializeField]
        private float JumpHeight { get; set; } = 2.8f;

        [field: SerializeField]
        private float JumpTime { get; set; } = 0.4f;

        [field: SerializeField]
        private float FallGravityMultiplier { get; set; } = 2f;

        [field: SerializeField]
        private float RunSpeed { get; set; } = 1f;

        private bool IsBlocked { get; set; }

        private Coroutine JumpCoroutine { get; set; }

        // Use this for initialization
        void Start()
        {
            Animator.SetFloat("runSpeed", RunSpeed);

            GameManager.Instance.OnGamePaused += OnGamePaused;
        }

        // Update is called once per frame
        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Space) || IsBlocked)
                return;

            StartJump();
        }

        private void StartJump()
        {
            IsBlocked = true;

            AudioSource.PlayOneShot(JumpSound, JumpSoundVolume);
            Animator.SetTrigger("jumpTrigger");

            if (JumpCoroutine != null)
                StopCoroutine(JumpCoroutine);

            JumpCoroutine = StartCoroutine(Jump());
        }

        // (06/03/2022) Based on "Math for Game Programmers: Building a Better Jump", available at: https://www.youtube.com/watch?v=hG9SzQxaCm8
        private IEnumerator Jump()
        {
            // th is the time will take to take the player to the top of the parabola
            // This is the reciprocal of that value
            var oneOverTime = 1f / JumpTime;

            // v0 = 2 * JumpHeight / th
            var verticalVelocity = 2f * JumpHeight * oneOverTime;
            // g = -2 * JumpHeight / (th ^ 2)
            var gravity = -(verticalVelocity) * oneOverTime;

            var initialPosition = transform.position;
            var updatedFallGravity = false;

            do
            {
                var deltaTime = Time.deltaTime;
                var heightIncrement = (verticalVelocity * deltaTime) + (gravity * deltaTime * deltaTime * 0.5f);

                // Prevents miscalculation
                if (transform.position.y + heightIncrement <= initialPosition.y)
                {
                    transform.position = initialPosition;
                    break;
                }

                transform.position += heightIncrement * Vector3.up;
                verticalVelocity += gravity * deltaTime;

                if (!updatedFallGravity
                    && verticalVelocity <= 0f)
                {
                    Animator.SetTrigger("jumpFall");
                    updatedFallGravity = true;
                    gravity *= FallGravityMultiplier;
                }

                yield return null;
            }
            while (transform.position.y > initialPosition.y);

            Animator.SetTrigger("jumpEnd");
            IsBlocked = false;
            JumpCoroutine = null;
        }

        private void OnGamePaused(bool paused)
        {
            if (JumpCoroutine != null)
                return;

            IsBlocked = paused;
            Animator.SetBool("playerIdle", paused);
        }

        public void OnDeath(Action onComplete)
        {
            StopAllCoroutines();
            Death(onComplete);
        }

        private void Death(Action onComplete)
        {
            IsBlocked = true;

            Animator.SetTrigger("deathTrigger");

            Destroy(BoxCollider);

            StartCoroutine(FallOnDeath(
                0.5f,
                1.8f,
                new Vector3(transform.position.x, -GameManager.halfVerticalScreenSize - 0.5f, transform.position.z),
                () =>
                {
                    Destroy(gameObject);
                    onComplete();
                }));

        }

        // TODO: refactor jump to represent parabolic movement so that we can use that function here
        private IEnumerator FallOnDeath(
            float fallTime,
            float descendGravityMultiplier,
            Vector3 finalPosition,
            Action onComplete)
        {
            var oneOverTime = 1f / fallTime;

            // v0 = 2 * JumpHeight / th
            var verticalVelocity = 2f * JumpHeight * oneOverTime;
            // g = -2 * JumpHeight / (th ^ 2)
            var gravity = -(verticalVelocity) * oneOverTime;

            var updatedFallGravity = false;

            do
            {
                var deltaTime = Time.deltaTime;
                var heightIncrement = (verticalVelocity * deltaTime) + (gravity * deltaTime * deltaTime * 0.5f);

                // Prevents miscalculation
                if (transform.position.y + heightIncrement <= finalPosition.y)
                {
                    transform.position = finalPosition;
                    break;
                }

                transform.position += heightIncrement * Vector3.up;
                verticalVelocity += gravity * deltaTime;

                if (!updatedFallGravity
                    && verticalVelocity <= 0f)
                {
                    updatedFallGravity = true;
                    gravity *= descendGravityMultiplier;
                }

                yield return null;
            }
            while (transform.position.y > finalPosition.y);

            onComplete();
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnGamePaused -= OnGamePaused;

            if (JumpCoroutine != null)
                StopCoroutine(JumpCoroutine);
        }
    }
}
