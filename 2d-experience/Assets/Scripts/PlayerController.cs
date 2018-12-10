using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float jumpSpeed = 22f;
    public float jumpDuration = 3.2f;
    public float jumpHeight = 1.6f;
    public float jumpAnimationSpeed = 0.45f;
    public float runSpeed = 1f;
    private bool isJumping = false;
    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        animator.SetFloat("runSpeed", runSpeed);
        animator.SetFloat("jumpSpeed", jumpAnimationSpeed);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            animator.SetTrigger("jumpTrigger");
            StartCoroutine(Jump());
        }
    }

    IEnumerator Jump()
    {
        isJumping = true;

        float horizontalCoordinate = transform.position.x;
        float currentPositionY = transform.position.y;

        yield return StartCoroutine(MoveCharacter(new Vector2(horizontalCoordinate, currentPositionY), new Vector2(horizontalCoordinate, jumpHeight + currentPositionY), jumpSpeed, jumpDuration));

        yield return StartCoroutine(MoveCharacter(new Vector2(horizontalCoordinate, jumpHeight + currentPositionY), new Vector2(horizontalCoordinate, currentPositionY), jumpSpeed, jumpDuration));

        isJumping = false;
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
}
