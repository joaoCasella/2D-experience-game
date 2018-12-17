using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float jumpSpeed = 22f;
    public float jumpDuration = 3.2f;
    public float jumpHeight = 1.6f;
    public float jumpAnimationSpeed = 0.45f;
    public float runSpeed = 1f;
    private bool isBlocked = false;
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip audioClip;
    public static float jumpSoundVolume = .5f;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        animator.SetFloat("runSpeed", runSpeed);
        animator.SetFloat("jumpSpeed", jumpAnimationSpeed);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && !isBlocked)
        {
            audioSource.PlayOneShot(audioClip, jumpSoundVolume);
            
            StartCoroutine(Jump());
        }
        ToggleRunningState();
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

    private void FixedUpdate()
    {
       
    }

    public void ToggleRunningState()
    {
        isBlocked = !LevelManager.isGameOn;
        if(LevelManager.isGameOn)
        {
            animator.Play("PlayerWalk");
        } else
        {
            animator.Play("PlayerIdle");
        }
    }

    public void OnDeath()
    {
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        isBlocked = true;

        transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 100), ForceMode2D.Impulse);

        yield return null;

        Destroy(GetComponent<Rigidbody2D>());
        animator.SetTrigger("deathTrigger");

        isBlocked = false;
    }
}
