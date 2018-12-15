using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour {
    private float speed = .05f;
    public static bool isMoving;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            transform.Translate(speed * Vector2.left);
        }
        
    }
}
