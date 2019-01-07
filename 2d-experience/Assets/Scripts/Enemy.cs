using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private Animator animator;
    public delegate void OnCollision();
    public static event OnCollision OnPlayerCollision;

	// Use this for initialization
	void Start () {
        animator = transform.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        animator.speed = LevelManager.IsGamePaused() ? 0.6f : 1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            OnPlayerCollision();
        } else if (collision.gameObject.tag == "Floor")
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            transform.parent = null;
        }
    }
}
