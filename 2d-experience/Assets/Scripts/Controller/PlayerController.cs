using Runner.Scripts.Inputter;
using Runner.Scripts.Manager;
using System;
using System.Collections;
using UnityEngine;

namespace Runner.Scripts.Controller
{
    public class PlayerController : MonoBehaviour, IInputListener
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

        public InputListenerPriority Priority => InputListenerPriority.Gameplay;

        // Use this for initialization
        private void Start()
        {
            Animator.SetFloat("runSpeed", RunSpeed);

            GameManager.Instance.OnGamePaused += OnGamePaused;
            InputManager.Instance.RegisterInputListener(this);
            SoundManager.Instance.RegisterSoundFXSource(AudioSource);
        }

        public bool ConsumeInput(InputAction inputAction)
        {
            if (inputAction != InputAction.Action)
                return false;

            if (IsBlocked)
                return false;

            StartJump();
            return true;
        }

        private void StartJump()
        {
            IsBlocked = true;

            AudioSource.PlayOneShot(JumpSound, JumpSoundVolume);
            Animator.SetTrigger("jumpTrigger");

            if (JumpCoroutine != null)
                StopCoroutine(JumpCoroutine);

            JumpCoroutine = StartCoroutine(ParabolicMovement(
                time: JumpTime,
                descendGravityMultiplier: FallGravityMultiplier,
                finalPosition: transform.position,
                onMovementPeak: () => Animator.SetTrigger("jumpFall"),
                onComplete: () =>
                {
                    Animator.SetTrigger("jumpEnd");
                    IsBlocked = false;
                    JumpCoroutine = null;
                }));
        }

        // (06/03/2022) Based on "Math for Game Programmers: Building a Better Jump", available at: https://www.youtube.com/watch?v=hG9SzQxaCm8
        private IEnumerator ParabolicMovement(
            float time,
            float descendGravityMultiplier,
            Vector3 finalPosition,
            Action onMovementPeak,
            Action onComplete)
        {
            // th is the time will take to take the player to the top of the parabola
            // This is the reciprocal of that value
            var oneOverTime = 1f / time;

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
                    onMovementPeak?.Invoke();
                    updatedFallGravity = true;
                    gravity *= descendGravityMultiplier;
                }

                yield return null;
            }
            while (transform.position.y > finalPosition.y);

            onComplete?.Invoke();
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

            StartCoroutine(ParabolicMovement(
                0.5f,
                1.8f,
                new Vector3(transform.position.x, -GameManager.halfVerticalScreenSize - 0.5f, transform.position.z),
                null,
                () =>
                {
                    Destroy(gameObject);
                    onComplete();
                }));

        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnGamePaused -= OnGamePaused;

            if (InputManager.Instance != null)
                InputManager.Instance.DeregisterInputListener(this);

            if (JumpCoroutine != null)
                StopCoroutine(JumpCoroutine);

            if (AudioSource != null)
                SoundManager.Instance.DeregisterSoundFXSource(AudioSource);
        }
    }
}
