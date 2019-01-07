using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float jumpSpeed = 17f;
    public float jumpDuration = 4.5f;
    public float jumpHeight = 2f;
    public float jumpAnimationSpeed = 0.23f;
    public float runSpeed = 1f;
    private bool isBlocked = false;
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip audioClip;
    public static float jumpSoundVolume = .5f;
    public float playerDeathAnimationHeight = 5;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        animator.SetFloat("runSpeed", runSpeed);
        animator.SetFloat("jumpSpeed", jumpAnimationSpeed);
    }
	
	// Update is called once per frame
	void Update () {
        ToggleRunningState();

        if (Input.GetKeyDown(KeyCode.Space) && !isBlocked)
        {
            audioSource.PlayOneShot(audioClip, jumpSoundVolume);
            
            StartCoroutine(Jump());
        }
    }

    IEnumerator Jump()
    {
        isBlocked = true;
        animator.SetTrigger("jumpTrigger");

        float horizontalCoordinate = transform.position.x;
        float currentPositionY = transform.position.y;

        yield return StartCoroutine(MoveCharacter(new Vector2(horizontalCoordinate, currentPositionY), new Vector2(horizontalCoordinate, jumpHeight + currentPositionY), jumpSpeed, jumpDuration));

        yield return StartCoroutine(MoveCharacter(new Vector2(horizontalCoordinate, jumpHeight + currentPositionY), new Vector2(horizontalCoordinate, currentPositionY), jumpSpeed, jumpDuration));

        isBlocked = false;
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
        isBlocked = LevelManager.IsGamePaused();
        animator.SetBool("playerIdle", LevelManager.IsGamePaused());
    }

    public void OnDeath()
    {
        StopAllCoroutines();
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        animator.SetTrigger("deathTrigger");

        transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerDeathAnimationHeight, ForceMode2D.Impulse);

        Destroy(GetComponent<BoxCollider2D>());

        while (transform.position.y > -LevelManager.verticalScreenSize)
        {
            yield return null;
        }

        Destroy(transform.gameObject);
    }
}
