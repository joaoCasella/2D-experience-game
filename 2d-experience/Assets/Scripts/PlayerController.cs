﻿using Runner.Scripts.Manager;
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

        [Header("Player parameters")]
        [SerializeField]
        private float _jumpSpeed = 17f;
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

        private float JumpSpeed { get => _jumpSpeed; set => _jumpSpeed = value; }
        private float JumpDuration { get => _jumpDuration; set => _jumpDuration = value; }
        private float JumpHeight { get => _jumpHeight; set => _jumpHeight = value; }
        private float JumpAnimationSpeed { get => _jumpAnimationSpeed; set => _jumpAnimationSpeed = value; }
        private float RunSpeed { get => _runSpeed; set => _runSpeed = value; }

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

                StartCoroutine(Jump());
            }
        }

        private IEnumerator Jump()
        {
            _isBlocked = true;
            _animator.SetTrigger("jumpTrigger");

            float horizontalCoordinate = transform.position.x;
            float currentPositionY = transform.position.y;

            yield return StartCoroutine(MoveCharacter(new Vector2(horizontalCoordinate, currentPositionY), new Vector2(horizontalCoordinate, JumpHeight + currentPositionY), JumpSpeed, JumpDuration));

            yield return StartCoroutine(MoveCharacter(new Vector2(horizontalCoordinate, JumpHeight + currentPositionY), new Vector2(horizontalCoordinate, currentPositionY), JumpSpeed, JumpDuration));

            _isBlocked = false;
        }

        IEnumerator MoveCharacter(Vector2 currentPosition, Vector2 destinationPosition, float speed, float duration)
        {
            float step = (speed / (currentPosition - destinationPosition).magnitude) * Time.deltaTime;
            float t = 0;

            while (t <= duration)
            {
                t += step;
                transform.position = Vector2.Lerp(currentPosition, destinationPosition, t);
                yield return null;
            }
            transform.position = destinationPosition;
        }

        public void ToggleRunningState()
        {
            _isBlocked = LevelManager.IsGamePaused();
            _animator.SetBool("playerIdle", LevelManager.IsGamePaused());
        }

        public void OnDeath()
        {
            StopAllCoroutines();
            StartCoroutine(Death());
        }

        private IEnumerator Death()
        {
            _animator.SetTrigger("deathTrigger");

            transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * _playerDeathAnimationHeight, ForceMode2D.Impulse);

            Destroy(GetComponent<BoxCollider2D>());

            while (transform.position.y > -LevelManager.verticalScreenSize)
            {
                yield return null;
            }

            Destroy(transform.gameObject);
        }
    }
}
